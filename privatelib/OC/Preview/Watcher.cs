using System.Collections;
using System.Collections.Generic;
using OCP.Files;

namespace OC.Preview
{
/**
 * Class Watcher
 *
 * @package OC\Preview
 *
 * Class that will watch filesystem activity and remove previews as needed.
 */
    public class Watcher {
        /** @var IAppData */
        private IAppData appData;

        /**
         * Watcher constructor.
         *
         * @param IAppData $appData
         */
        public Watcher(IAppData appData) {
            this.appData = appData;
        }

        public void postWrite(Node node) {
            this.deleteNode(node);
        }

        protected void deleteNode(Node node) {
            // We only handle files
            if (node is Folder) {
                return;
            }

            try {
                var folder = this.appData.getFolder(node.getId().ToString());
                    folder.delete();
            } catch (NotFoundException e) {
                //Nothing to do
            }
        }

        public void versionRollback(IDictionary<string,Node> data) {
            if (data.ContainsKey("node"))
            {
                this.deleteNode(data["node"]);
            }
        }
    }
}