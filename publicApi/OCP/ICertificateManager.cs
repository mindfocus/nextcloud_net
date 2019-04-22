using System;
using System.Collections.Generic;
using System.Text;

namespace OCP
{
    /**
     * Manage trusted certificates for users
     * @since 8.0.0
     */
    public interface ICertificateManager
    {
        /**
         * Returns all certificates trusted by the user
         *
         * @return \OCP\ICertificate[]
         * @since 8.0.0
         */
        IList<ICertificate> listCertificates();

        /**
         * @param string certificate the certificate data
         * @param string name the filename for the certificate
         * @return \OCP\ICertificate
         * @throws \Exception If the certificate could not get added
         * @since 8.0.0 - since 8.1.0 throws exception instead of returning false
         */
        ICertificate addCertificate(string certificate, string name);

        /**
         * @param string name
         * @since 8.0.0
         */
        void removeCertificate(string name);

        /**
         * Get the path to the certificate bundle for this user
         *
         * @param string uid (optional) user to get the certificate bundle for, use `null` to get the system bundle (since 9.0.0)
         * @return string
         * @since 8.0.0
         */
        string getCertificateBundle(string uid = "");

        /**
         * Get the full local path to the certificate bundle for this user
         *
         * @param string uid (optional) user to get the certificate bundle for, use `null` to get the system bundle
         * @return string
         * @since 9.0.0
         */
        string getAbsoluteBundlePath(string uid = "");
    }

}
