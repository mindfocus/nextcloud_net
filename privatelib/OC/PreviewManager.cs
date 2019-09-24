﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ext;
using OCP;
using OCP.Files;
using OCP.Files.SimpleFS;
using OCP.Sym;

namespace OC
{
class PreviewManager : IPreview {
	/** @var IConfig */
	protected IConfig config;

	/** @var IRootFolder */
	protected IRootFolder rootFolder;

	/** @var IAppData */
	protected IAppData appData;

	/** @var EventDispatcherInterface */
	protected EventDispatcherInterface eventDispatcher;

	/** @var Generator */
	private OC.Preview.Generator generator;

	/** @var bool */
	protected bool providerListDirty = false;

	/** @var bool */
	protected bool registeredCoreProviders = false;

	/** @var array */
    protected IDictionary<string, IList<Action>> providers = new Dictionary<string, IList<Action>>();

	/** @var array mime type => support status */
	protected IDictionary<string,bool> mimeTypeSupportMap = new Dictionary<string,bool>();

	/** @var array */
	protected IList<string> defaultProviders;

	/** @var string */
	protected string userId;

	/**
	 * PreviewManager constructor.
	 *
	 * @param IConfig config
	 * @param IRootFolder rootFolder
	 * @param IAppData appData
	 * @param EventDispatcherInterface eventDispatcher
	 * @param string userId
	 */
	public PreviewManager(IConfig config,
								IRootFolder rootFolder,
								IAppData appData,
								EventDispatcherInterface eventDispatcher,
								string userId)
    {
        this.config = config;
        this.rootFolder = rootFolder;
        this.appData = appData;
        this.eventDispatcher = eventDispatcher;
        this.userId = userId;
    }

	/**
	 * In order to improve lazy loading a closure can be registered which will be
	 * called in case preview providers are actually requested
	 *
	 * callable has to return an instance of .OCP.Preview.IProvider
	 *
	 * @param string mimeTypeRegex Regex with the mime types that are supported by this provider
	 * @param .Closure callable
	 * @return void
	 */
	public void registerProvider(string mimeTypeRegex, Action callable) {
		if (!this.config.getSystemValueBool("enable_previews", true)) {
			return;
		}

		if ( !this.providers.ContainsKey(mimeTypeRegex)) {
			this.providers[mimeTypeRegex] = new List<Action>();
		}
		this.providers[mimeTypeRegex].Add(callable);
		this.providerListDirty = true;
	}

	/**
	 * Get all providers
	 * @return array
	 */
	public IDictionary<string, IList<Action>> getProviders() {
		if (!this.config.getSystemValueBool("enable_previews", true)) {
			return new Dictionary<string, IList<Action>>();
		}

		this.registerCoreProviders();
		if (this.providerListDirty)
		{
			var sortProviders = from provider in this.providers orderby provider.Key.Length descending select provider;
			this.providers = sortProviders.ToDictionary( pairKey => pairKey.Key, pairValue => pairValue.Value);
			this.providerListDirty = false;
		}

		return this.providers;
	}

	/**
	 * Does the manager have any providers
	 * @return bool
	 */
	public bool hasProviders() {
        this.registerCoreProviders();
        return this.providers.IsNotEmpty();
	}


	/**
	 * Returns a preview of a file
	 *
	 * The cache is searched first and if nothing usable was found then a preview is
	 * generated by one of the providers
	 *
	 * @param File file
	 * @param int width
	 * @param int height
	 * @param bool crop
	 * @param string mode
	 * @param string mimeType
	 * @return ISimpleFile
	 * @throws NotFoundException
	 * @throws .InvalidArgumentException if the preview would be invalid (in case the original image is invalid)
	 * @since 11.0.0 - .InvalidArgumentException was added in 12.0.0
	 */
	public ISimpleFile getPreview(File file,int width = -1,int  height = -1,bool crop = false, string mode = Constants.PREVIEW_MODE_FILE, string mimeType = null) {
		if (this.generator == null) {
			this.generator = new Generator(
				this.config,
				this,
				this.appData,
				new GeneratorHelper(
					this.rootFolder,
					this.config
				),
				this.eventDispatcher
			);
		}

		return this.generator.getPreview(file, width, height, crop, mode, mimeType);
	}

	/**
	 * returns true if the passed mime type is supported
	 *
	 * @param string mimeType
	 * @return boolean
	 */
	public function isMimeSupported(mimeType = "*") {
		if (!this.config.getSystemValue("enable_previews", true)) {
			return false;
		}

		if (isset(this.mimeTypeSupportMap[mimeType])) {
			return this.mimeTypeSupportMap[mimeType];
		}

		this.registerCoreProviders();
		providerMimeTypes = array_keys(this.providers);
		foreach (providerMimeTypes as supportedMimeType) {
			if (preg_match(supportedMimeType, mimeType)) {
				this.mimeTypeSupportMap[mimeType] = true;
				return true;
			}
		}
		this.mimeTypeSupportMap[mimeType] = false;
		return false;
	}

	/**
	 * Check if a preview can be generated for a file
	 *
	 * @param .OCP.Files.FileInfo file
	 * @return bool
	 */
	public function isAvailable(.OCP.Files.FileInfo file) {
		if (!this.config.getSystemValue("enable_previews", true)) {
			return false;
		}

		this.registerCoreProviders();
		if (!this.isMimeSupported(file.getMimetype())) {
			return false;
		}

		mount = file.getMountPoint();
		if (mount and !mount.getOption("previews", true)){
			return false;
		}

		foreach (this.providers as supportedMimeType => providers) {
			if (preg_match(supportedMimeType, file.getMimetype())) {
				foreach (providers as closure) {
					provider = closure();
					if (!(provider instanceof IProvider)) {
						continue;
					}

					/** @var provider IProvider */
					if (provider.isAvailable(file)) {
						return true;
					}
				}
			}
		}
		return false;
	}

	/**
	 * List of enabled default providers
	 *
	 * The following providers are enabled by default:
	 *  - OC.Preview.PNG
	 *  - OC.Preview.JPEG
	 *  - OC.Preview.GIF
	 *  - OC.Preview.BMP
	 *  - OC.Preview.HEIC
	 *  - OC.Preview.XBitmap
	 *  - OC.Preview.MarkDown
	 *  - OC.Preview.MP3
	 *  - OC.Preview.TXT
	 *
	 * The following providers are disabled by default due to performance or privacy concerns:
	 *  - OC.Preview.Font
	 *  - OC.Preview.Illustrator
	 *  - OC.Preview.Movie
	 *  - OC.Preview.MSOfficeDoc
	 *  - OC.Preview.MSOffice2003
	 *  - OC.Preview.MSOffice2007
	 *  - OC.Preview.OpenDocument
	 *  - OC.Preview.PDF
	 *  - OC.Preview.Photoshop
	 *  - OC.Preview.Postscript
	 *  - OC.Preview.StarOffice
	 *  - OC.Preview.SVG
	 *  - OC.Preview.TIFF
	 *
	 * @return array
	 */
	protected function getEnabledDefaultProvider() {
		if (this.defaultProviders !== null) {
			return this.defaultProviders;
		}

		imageProviders = [
			Preview.PNG::class,
			Preview.JPEG::class,
			Preview.GIF::class,
			Preview.BMP::class,
			Preview.HEIC::class,
			Preview.XBitmap::class
		];

		this.defaultProviders = this.config.getSystemValue("enabledPreviewProviders", array_merge([
			Preview.MarkDown::class,
			Preview.MP3::class,
			Preview.TXT::class,
		], imageProviders));

		if (in_array(Preview.Image::class, this.defaultProviders)) {
			this.defaultProviders = array_merge(this.defaultProviders, imageProviders);
		}
		this.defaultProviders = array_unique(this.defaultProviders);
		return this.defaultProviders;
	}

	/**
	 * Register the default providers (if enabled)
	 *
	 * @param string class
	 * @param string mimeType
	 */
	protected function registerCoreProvider(class, mimeType, options = []) {
		if (in_array(trim(class, ".."), this.getEnabledDefaultProvider())) {
			this.registerProvider(mimeType, function () use (class, options) {
				return new class(options);
			});
		}
	}

	/**
	 * Register the default providers (if enabled)
	 */
	protected void registerCoreProviders() {
		if (this.registeredCoreProviders) {
			return;
		}
		this.registeredCoreProviders = true;

		this.registerCoreProvider(Preview.TXT::class, "/text./plain/");
		this.registerCoreProvider(Preview.MarkDown::class, "/text./(x-)?markdown/");
		this.registerCoreProvider(Preview.PNG::class, "/image./png/");
		this.registerCoreProvider(Preview.JPEG::class, "/image./jpeg/");
		this.registerCoreProvider(Preview.GIF::class, "/image./gif/");
		this.registerCoreProvider(Preview.BMP::class, "/image./bmp/");
		this.registerCoreProvider(Preview.XBitmap::class, "/image./x-xbitmap/");
		this.registerCoreProvider(Preview.MP3::class, "/audio./mpeg/");

		// SVG, Office and Bitmap require imagick
		if (extension_loaded("imagick")) {
			checkImagick = new .Imagick();

			imagickProviders = [
				"SVG"	=> ["mimetype" => "/image./svg.+xml/", "class" => Preview.SVG::class],
				"TIFF"	=> ["mimetype" => "/image./tiff/", "class" => Preview.TIFF::class],
				"PDF"	=> ["mimetype" => "/application./pdf/", "class" => Preview.PDF::class],
				"AI"	=> ["mimetype" => "/application./illustrator/", "class" => Preview.Illustrator::class],
				"PSD"	=> ["mimetype" => "/application./x-photoshop/", "class" => Preview.Photoshop::class],
				"EPS"	=> ["mimetype" => "/application./postscript/", "class" => Preview.Postscript::class],
				"TTF"	=> ["mimetype" => "/application./(?:font-sfnt|x-font)/", "class" => Preview.Font::class],
				"HEIC"  => ["mimetype" => "/image./hei(f|c)/", "class" => Preview.HEIC::class],
			];

			foreach (imagickProviders as queryFormat => provider) {
				class = provider["class"];
				if (!in_array(trim(class, ".."), this.getEnabledDefaultProvider())) {
					continue;
				}

				if (count(checkImagick.queryFormats(queryFormat)) === 1) {
					this.registerCoreProvider(class, provider["mimetype"]);
				}
			}

			if (count(checkImagick.queryFormats("PDF")) === 1) {
				if (.OC_Helper::is_function_enabled("shell_exec")) {
					officeFound = is_string(this.config.getSystemValue("preview_libreoffice_path", null));

					if (!officeFound) {
						//let"s see if there is libreoffice or openoffice on this machine
						whichLibreOffice = shell_exec("command -v libreoffice");
						officeFound = !empty(whichLibreOffice);
						if (!officeFound) {
							whichOpenOffice = shell_exec("command -v openoffice");
							officeFound = !empty(whichOpenOffice);
						}
					}

					if (officeFound) {
						this.registerCoreProvider(Preview.MSOfficeDoc::class, "/application./msword/");
						this.registerCoreProvider(Preview.MSOffice2003::class, "/application./vnd.ms-.*/");
						this.registerCoreProvider(Preview.MSOffice2007::class, "/application./vnd.openxmlformats-officedocument.*/");
						this.registerCoreProvider(Preview.OpenDocument::class, "/application./vnd.oasis.opendocument.*/");
						this.registerCoreProvider(Preview.StarOffice::class, "/application./vnd.sun.xml.*/");
					}
				}
			}
		}

		// Video requires avconv or ffmpeg
		if (in_array(Preview.Movie::class, this.getEnabledDefaultProvider())) {
			avconvBinary = .OC_Helper::findBinaryPath("avconv");
			ffmpegBinary = avconvBinary ? null : .OC_Helper::findBinaryPath("ffmpeg");

			if (avconvBinary || ffmpegBinary) {
				// FIXME // a bit hacky but didn"t want to use subclasses
				.OC.Preview.Movie::avconvBinary = avconvBinary;
				.OC.Preview.Movie::ffmpegBinary = ffmpegBinary;

				this.registerCoreProvider(Preview.Movie::class, "/video./.*/");
			}
		}
	}
}

}