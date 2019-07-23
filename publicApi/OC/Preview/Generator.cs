﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OC.Preview
{
class Generator {

	/** @var IPreview */
	private $previewManager;
	/** @var IConfig */
	private $config;
	/** @var IAppData */
	private $appData;
	/** @var GeneratorHelper */
	private $helper;
	/** @var EventDispatcherInterface */
	private $eventDispatcher;

	/**
	 * @param IConfig $config
	 * @param IPreview $previewManager
	 * @param IAppData $appData
	 * @param GeneratorHelper $helper
	 * @param EventDispatcherInterface $eventDispatcher
	 */
	public function __construct(
		IConfig $config,
		IPreview $previewManager,
		IAppData $appData,
		GeneratorHelper $helper,
		EventDispatcherInterface $eventDispatcher
	) {
		$this.config = $config;
		$this.previewManager = $previewManager;
		$this.appData = $appData;
		$this.helper = $helper;
		$this.eventDispatcher = $eventDispatcher;
	}

	/**
	 * Returns a preview of a file
	 *
	 * The cache is searched first and if nothing usable was found then a preview is
	 * generated by one of the providers
	 *
	 * @param File $file
	 * @param int $width
	 * @param int $height
	 * @param bool $crop
	 * @param string $mode
	 * @param string $mimeType
	 * @return ISimpleFile
	 * @throws NotFoundException
	 * @throws \InvalidArgumentException if the preview would be invalid (in case the original image is invalid)
	 */
	public function getPreview(File $file, $width = -1, $height = -1, $crop = false, $mode = IPreview::MODE_FILL, $mimeType = null) {
		//Make sure that we can read the file
		if (!$file.isReadable()) {
			throw new NotFoundException('Cannot read file');
		}


		$this.eventDispatcher.dispatch(
			IPreview::EVENT,
			new GenericEvent($file,[
				'width' => $width,
				'height' => $height,
				'crop' => $crop,
				'mode' => $mode
			])
		);

		if ($mimeType === null) {
			$mimeType = $file.getMimeType();
		}
		if (!$this.previewManager.isMimeSupported($mimeType)) {
			throw new NotFoundException();
		}

		$previewFolder = $this.getPreviewFolder($file);

		// Get the max preview and infer the max preview sizes from that
		$maxPreview = $this.getMaxPreview($previewFolder, $file, $mimeType);
		if ($maxPreview.getSize() === 0) {
			$maxPreview.delete();
			throw new NotFoundException('Max preview size 0, invalid!');
		}

		list($maxWidth, $maxHeight) = $this.getPreviewSize($maxPreview);

		// If both width and heigth are -1 we just want the max preview
		if ($width === -1 && $height === -1) {
			$width = $maxWidth;
			$height = $maxHeight;
		}

		// Calculate the preview size
		list($width, $height) = $this.calculateSize($width, $height, $crop, $mode, $maxWidth, $maxHeight);

		// No need to generate a preview that is just the max preview
		if ($width === $maxWidth && $height === $maxHeight) {
			return $maxPreview;
		}

		// Try to get a cached preview. Else generate (and store) one
		try {
			try {
				$preview = $this.getCachedPreview($previewFolder, $width, $height, $crop, $maxPreview.getMimeType());
			} catch (NotFoundException $e) {
				$preview = $this.generatePreview($previewFolder, $maxPreview, $width, $height, $crop, $maxWidth, $maxHeight);
			}
		} catch (\InvalidArgumentException $e) {
			throw new NotFoundException();
		}

		if ($preview.getSize() === 0) {
			$preview.delete();
			throw new NotFoundException('Cached preview size 0, invalid!');
		}

		return $preview;
	}

	/**
	 * @param ISimpleFolder $previewFolder
	 * @param File $file
	 * @param string $mimeType
	 * @return ISimpleFile
	 * @throws NotFoundException
	 */
	private function getMaxPreview(ISimpleFolder $previewFolder, File $file, $mimeType) {
		$nodes = $previewFolder.getDirectoryListing();

		foreach ($nodes as $node) {
			if (strpos($node.getName(), 'max')) {
				return $node;
			}
		}

		$previewProviders = $this.previewManager.getProviders();
		foreach ($previewProviders as $supportedMimeType => $providers) {
			if (!preg_match($supportedMimeType, $mimeType)) {
				continue;
			}

			foreach ($providers as $provider) {
				$provider = $this.helper.getProvider($provider);
				if (!($provider instanceof IProvider)) {
					continue;
				}

				if (!$provider.isAvailable($file)) {
					continue;
				}

				$maxWidth = (int)$this.config.getSystemValue('preview_max_x', 4096);
				$maxHeight = (int)$this.config.getSystemValue('preview_max_y', 4096);

				$preview = $this.helper.getThumbnail($provider, $file, $maxWidth, $maxHeight);

				if (!($preview instanceof IImage)) {
					continue;
				}

				// Try to get the extention.
				try {
					$ext = $this.getExtention($preview.dataMimeType());
				} catch (\InvalidArgumentException $e) {
					// Just continue to the next iteration if this preview doesn't have a valid mimetype
					continue;
				}

				$path = (string)$preview.width() . '-' . (string)$preview.height() . '-max.' . $ext;
				try {
					$file = $previewFolder.newFile($path);
					$file.putContent($preview.data());
				} catch (NotPermittedException $e) {
					throw new NotFoundException();
				}

				return $file;
			}
		}

		throw new NotFoundException();
	}

	/**
	 * @param ISimpleFile $file
	 * @return int[]
	 */
	private function getPreviewSize(ISimpleFile $file) {
		$size = explode('-', $file.getName());
		return [(int)$size[0], (int)$size[1]];
	}

	/**
	 * @param int $width
	 * @param int $height
	 * @param bool $crop
	 * @param string $mimeType
	 * @return string
	 */
	private function generatePath($width, $height, $crop, $mimeType) {
		$path = (string)$width . '-' . (string)$height;
		if ($crop) {
			$path .= '-crop';
		}

		$ext = $this.getExtention($mimeType);
		$path .= '.' . $ext;
		return $path;
	}



	/**
	 * @param int $width
	 * @param int $height
	 * @param bool $crop
	 * @param string $mode
	 * @param int $maxWidth
	 * @param int $maxHeight
	 * @return int[]
	 */
	private function calculateSize($width, $height, $crop, $mode, $maxWidth, $maxHeight) {

		/*
		 * If we are not cropping we have to make sure the requested image
		 * respects the aspect ratio of the original.
		 */
		if (!$crop) {
			$ratio = $maxHeight / $maxWidth;

			if ($width === -1) {
				$width = $height / $ratio;
			}
			if ($height === -1) {
				$height = $width * $ratio;
			}

			$ratioH = $height / $maxHeight;
			$ratioW = $width / $maxWidth;

			/*
			 * Fill means that the $height and $width are the max
			 * Cover means min.
			 */
			if ($mode === IPreview::MODE_FILL) {
				if ($ratioH > $ratioW) {
					$height = $width * $ratio;
				} else {
					$width = $height / $ratio;
				}
			} else if ($mode === IPreview::MODE_COVER) {
				if ($ratioH > $ratioW) {
					$width = $height / $ratio;
				} else {
					$height = $width * $ratio;
				}
			}
		}

		if ($height !== $maxHeight && $width !== $maxWidth) {
			/*
			 * Scale to the nearest power of four
			 */
			$pow4height = 4 ** ceil(log($height) / log(4));
			$pow4width = 4 ** ceil(log($width) / log(4));

			// Minimum size is 64
			$pow4height = max($pow4height, 64);
			$pow4width = max($pow4width, 64);

			$ratioH = $height / $pow4height;
			$ratioW = $width / $pow4width;

			if ($ratioH < $ratioW) {
				$width = $pow4width;
				$height /= $ratioW;
			} else {
				$height = $pow4height;
				$width /= $ratioH;
			}
		}

		/*
 		 * Make sure the requested height and width fall within the max
 		 * of the preview.
 		 */
		if ($height > $maxHeight) {
			$ratio = $height / $maxHeight;
			$height = $maxHeight;
			$width /= $ratio;
		}
		if ($width > $maxWidth) {
			$ratio = $width / $maxWidth;
			$width = $maxWidth;
			$height /= $ratio;
		}

		return [(int)round($width), (int)round($height)];
	}

	/**
	 * @param ISimpleFolder $previewFolder
	 * @param ISimpleFile $maxPreview
	 * @param int $width
	 * @param int $height
	 * @param bool $crop
	 * @param int $maxWidth
	 * @param int $maxHeight
	 * @return ISimpleFile
	 * @throws NotFoundException
	 * @throws \InvalidArgumentException if the preview would be invalid (in case the original image is invalid)
	 */
	private function generatePreview(ISimpleFolder $previewFolder, ISimpleFile $maxPreview, $width, $height, $crop, $maxWidth, $maxHeight) {
		$preview = $this.helper.getImage($maxPreview);

		if (!$preview.valid()) {
			throw new \InvalidArgumentException('Failed to generate preview, failed to load image');
		}

		if ($crop) {
			if ($height !== $preview.height() && $width !== $preview.width()) {
				//Resize
				$widthR = $preview.width() / $width;
				$heightR = $preview.height() / $height;

				if ($widthR > $heightR) {
					$scaleH = $height;
					$scaleW = $maxWidth / $heightR;
				} else {
					$scaleH = $maxHeight / $widthR;
					$scaleW = $width;
				}
				$preview.preciseResize((int)round($scaleW), (int)round($scaleH));
			}
			$cropX = (int)floor(abs($width - $preview.width()) * 0.5);
			$cropY = 0;
			$preview.crop($cropX, $cropY, $width, $height);
		} else {
			$preview.resize(max($width, $height));
		}


		$path = $this.generatePath($width, $height, $crop, $preview.dataMimeType());
		try {
			$file = $previewFolder.newFile($path);
			$file.putContent($preview.data());
		} catch (NotPermittedException $e) {
			throw new NotFoundException();
		}

		return $file;
	}

	/**
	 * @param ISimpleFolder $previewFolder
	 * @param int $width
	 * @param int $height
	 * @param bool $crop
	 * @param string $mimeType
	 * @return ISimpleFile
	 *
	 * @throws NotFoundException
	 */
	private function getCachedPreview(ISimpleFolder $previewFolder, $width, $height, $crop, $mimeType) {
		$path = $this.generatePath($width, $height, $crop, $mimeType);

		return $previewFolder.getFile($path);
	}

	/**
	 * Get the specific preview folder for this file
	 *
	 * @param File $file
	 * @return ISimpleFolder
	 */
	private function getPreviewFolder(File $file) {
		try {
			$folder = $this.appData.getFolder($file.getId());
		} catch (NotFoundException $e) {
			$folder = $this.appData.newFolder($file.getId());
		}

		return $folder;
	}

	/**
	 * @param string $mimeType
	 * @return null|string
	 * @throws \InvalidArgumentException
	 */
	private function getExtention($mimeType) {
		switch ($mimeType) {
			case 'image/png':
				return 'png';
			case 'image/jpeg':
				return 'jpg';
			case 'image/gif':
				return 'gif';
			default:
				throw new \InvalidArgumentException('Not a valid mimetype');
		}
	}
}

}
