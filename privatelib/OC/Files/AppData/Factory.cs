using System.Collections.Generic;
using OCP.Files;

namespace OC.Files.AppData
{
    public class Factory {

        /** @var IRootFolder */
        private IRootFolder rootFolder;

        /** @var SystemConfig */
        private SystemConfig config;

        private IDictionary<string,AppData> folders = new Dictionary<string, AppData>();

        public Factory(IRootFolder rootFolder,
        SystemConfig systemConfig) {

            this.rootFolder = rootFolder;
                this.config = systemConfig;
        }

        /**
         * @param string appId
         * @return AppData
         */
        public AppData get(string appId) {
            if (!this.folders.ContainsKey(appId))
            {
                this.folders[appId] = new AppData(this.rootFolder, this.config, appId);
            }

            return this.folders[appId];
        }
    }
}