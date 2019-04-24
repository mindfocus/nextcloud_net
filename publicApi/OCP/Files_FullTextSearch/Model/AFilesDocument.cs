using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Files_FullTextSearch.Model
{
/**
 * Abstract Class AFilesDocument
 * 
   use OC\FullTextSearch\Model\IndexDocument;
   use OCP\FullTextSearch\Model\IIndexDocument;
   
 * This is mostly used by 3rd party apps that want to complete the IIndexDocument
 * with more information about a file before its index:
 *
 *    \OC::server->getEventDispatcher()->addListener(
 *        '\OCA\Files_FullTextSearch::onFileIndexing',
 *        function(GenericEvent e) {
 *            //@var \OCP\Files\Node file
 *            file = e->getArgument('file');
 *
 *            // @var \OCP\Files_FullTextSearch\Model\AFilesDocument document
 *            document = e->getArgument('document');
 *        }
 *    );
 *
 * @since 15.0.0
 *
 * @package OCP\Files_FullTextSearch\Model
 */
    abstract class AFilesDocument : IndexDocument {


    /**
     * Returns the owner of the document/file.
     *
     * @since 15.0.0
     *
     * @return string
     */
    abstract public string getOwnerId();


    /**
     * Returns the current viewer of the document/file.
     *
     * @since 15.0.0
     *
     * @return string
     */
    abstract public string getViewerId();


    /**
     * Returns the type of the document/file.
     *
     * @since 15.0.0
     *
     * @return string \OCP\Files\FileInfo::TYPE_FILE|\OCP\Files\FileInfo::TYPE_FOLDER
     */
    abstract public string getType();


    /**
     * Returns the mimetype of the document/file.
     *
     * @since 15.0.0
     *
     * @return string
     */
    abstract public string getMimetype();

    /**
     * Returns the path of the document/file.
     *
     * @since 15.0.0
     *
     * @return string
     */
    abstract public string getPath();

    }


}
