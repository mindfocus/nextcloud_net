using System;
using System.Collections.Generic;
using System.Text;

namespace OCP
{
    /**
     * Interface ICertificate
     *
     * @package OCP
     * @since 8.0.0
     */
    public interface ICertificate
    {
        /**
         * @return string
         * @since 8.0.0
         */
        string getName();

        /**
         * @return string
         * @since 8.0.0
         */
        string getCommonName();

        /**
         * @return string
         * @since 8.0.0
         */
        string getOrganization();

        /**
         * @return \DateTime
         * @since 8.0.0
         */
        string getIssueDate();

        /**
         * @return \DateTime
         * @since 8.0.0
         */
        string getExpireDate();

        /**
         * @return bool
         * @since 8.0.0
         */
        string isExpired();

        /**
         * @return string
         * @since 8.0.0
         */
        string getIssuerName();

        /**
         * @return string
         * @since 8.0.0
         */
        string getIssuerOrganization();
    }

}
