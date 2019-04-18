using System;
using System.Collections.Generic;
using System.Text;

namespace publicApi.OC.Log
{
    /**
     * Trait RotationTrait
     *
     * @package OCP\Log
     *
     * @since 14.0.0
     */
    class RotationTrait
    {

    /**
	 * @var string
	 * @since 14.0.0
	 */
    protected string filePath;

	/**
	 * @var int
	 * @since 14.0.0
	 */
	protected int maxSize;

	/**
	 * @return string the resulting new filepath
	 * @since 14.0.0
	 */
	string rotate(){
            var rotatedFile = ""; //this.filePath.'.1';

        //rename($this->filePath, $rotatedFile);
		return rotatedFile;
    }

    /**
	 * @return bool
	 * @since 14.0.0
	 */
    bool shouldRotateBySize(){
		if (this.maxSize > 0) {
                var filesize = 1; // @filesize($this->filePath);
        if (filesize >= this.maxSize) {
            return true;
        }
    }
		return false;
    }

}

}
