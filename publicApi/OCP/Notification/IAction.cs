using System;
namespace OCP.Notification
{
    /**
     * Interface IAction
     *
     * @package OCP\Notification
     * @since 9.0.0
     */
    public interface IAction
    {
        /**
         * @param string label
         * @return this
         * @throws \InvalidArgumentException if the label is invalid
         * @since 9.0.0
         */
        IAction setLabel(string label);

        /**
         * @return string
         * @since 9.0.0
         */
        string getLabel();

        /**
         * @param string label
         * @return this
         * @throws \InvalidArgumentException if the label is invalid
         * @since 9.0.0
         */
        IAction setParsedLabel(string label);

        /**
         * @return string
         * @since 9.0.0
         */
        string getParsedLabel();

        /**
         * @param primary bool
         * @return this
         * @throws \InvalidArgumentException if primary is invalid
         * @since 9.0.0
         */
        IAction setPrimary(bool primary);

        /**
         * @return bool
         * @since 9.0.0
         */
        bool isPrimary();

        /**
         * @param string link
         * @param string requestType
         * @return this
         * @throws \InvalidArgumentException if the link is invalid
         * @since 9.0.0
         */
        IAction setLink(string link, string requestType);

        /**
         * @return string
         * @since 9.0.0
         */
        string getLink();

        /**
         * @return string
         * @since 9.0.0
         */
        string getRequestType();

        /**
         * @return bool
         * @since 9.0.0
         */
        bool isValid();

        /**
         * @return bool
         * @since 9.0.0
         */
        bool isValidParsed();
    }

}
