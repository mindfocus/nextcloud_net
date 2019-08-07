using OCP.Comments;
using OCP.Files;
using OCP.Files.SimpleFS;

namespace OC.Files.SimpleFS
{

    public class SimpleFolder : ISimpleFolder   {

    /** @var Folder */
    private Folder folder;

    /**
     * Folder constructor.
     *
     * @param Folder folder
     */
    public SimpleFolder(Folder folder) {
        this.folder = folder;
    }

    public string getName() {
        return this.folder.getName();
    }

    public function getDirectoryListing() {
        listing = this.folder.getDirectoryListing();

            fileListing = array_map(function(Node file) {
            if (file instanceof File) {
                return new SimpleFile(file);
            }
            return null;
        }, listing);

        fileListing = array_filter(fileListing);

        return array_values(fileListing);
    }

    public function delete() {
        this.folder.delete();
    }

    public function fileExists(name) {
        return this.folder.nodeExists(name);
    }

    public function getFile(name) {
        file = this.folder.get(name);

        if (!(file instanceof File)) {
            throw new NotFoundException();
        }

        return new SimpleFile(file);
    }

    public function newFile(name) {
        file = this.folder.newFile(name);

        return new SimpleFile(file);
    }
    }

}