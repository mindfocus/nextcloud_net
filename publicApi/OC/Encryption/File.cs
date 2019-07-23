using System.Collections.Generic;
using OCP.Files;

namespace OC.Encryption
{
    public class File : OCP.Encryption.IFile
    {
        protected Util util;
        private IRootFolder rootFolder;
        private OCP.ShareNS.IManager shareManager;

        public File(Util util, IRootFolder rootFolder, OCP.ShareNS.IManager shareManager)
        {
            this.util = util;
            this.rootFolder = rootFolder;
            this.shareManager = shareManager;
        }
        public IList<string> getAccessList(string path)
        {
            throw new System.NotImplementedException();
        }
    }
}