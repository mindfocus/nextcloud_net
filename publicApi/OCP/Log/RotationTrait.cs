using System.IO;

namespace OCP.Log
{
    /**
 * Trait RotationTrait
 *
 * @package OCP\Log
 *
 * @since 14.0.0
 */
    public class RotationTrait
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
        protected string rotate(){
            var rotatedFile = this.filePath + ".1";
            File.Move(this.filePath, rotatedFile);
//            rename(this.filePath, rotatedFile);
            return rotatedFile;
        }

        /**
         * @return bool
         * @since 14.0.0
         */
        protected bool shouldRotateBySize() {
            if ((int)this.maxSize > 0)
            {
                var filesize = new FileInfo(this.filePath).Length;
                if (filesize >= (int)this.maxSize) {
                    return true;
                }
            }
            return false;
        }
    }
}