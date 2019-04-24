using System;
using System.Collections.Generic;
using System.Text;
using OCP.FullTextSearch.Model;

namespace OCP.FullTextSearch.Service
{
/**
 * Interface IIndexService
 *
 * @since 15.0.0
 *
 * @package OCP\FullTextSearch\Service
 */
    public interface IIndexService {


        /**
         * Create an Index
         *
         * @since 15.0.1
         *
         * @param string providerId
         * @param string documentId
         * @param string userId
         * @param int status
         * @return IIndex
         */
        IIndex createIndex(string providerId, string documentId, string userId, int status);


        /**
         * Retrieve an Index from the database, based on the Id of the Provider
         * and the Id of the Document
         *
         * @since 15.0.0
         *
         * @param string providerId
         * @param string documentId
         *
         * @return IIndex
         */
        IIndex getIndex(string providerId, string documentId);


        /**
         * Update the status of an Index. status is a bit flag, setting reset to
         * true will reset the status to the value defined in the parameter.
         *
         * @since 15.0.0
         *
         * @param string providerId
         * @param string documentId
         * @param int status
         * @param bool reset
         */
        void updateIndexStatus(string providerId, string documentId, int status, bool reset = false);


        /**
         * Update the status of an array of Index. status is a bit flag, setting reset to
         * true will reset the status to the value defined in the parameter.
         *
         * @since 15.0.0
         *
         * @param string providerId
         * @param array documentIds
         * @param int status
         * @param bool reset
         */
        void updateIndexesStatus(string providerId, IList<string> documentIds, int status, bool reset = false);


        /**
         * Update an array of Index.
         *
         * @since 15.0.0
         *
         * @param array indexes
         */
        void updateIndexes(IList<string> indexes);

    }


}
