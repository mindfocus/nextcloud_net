namespace OCP.Collaboration.Collaborators
{
/**
 * Class SearchResultType
 *
 * @package OCP\Collaboration\Collaborators
 * @since 13.0.0
 */
    public class SearchResultType {
        /** @var string  */
        protected string label;

        /**
         * SearchResultType constructor.
         *
         * @param string label
         * @since 13.0.0
         */
        public SearchResultType(string label)
        {
            this.label = this.getValidatedType(label);
        }

        /**
         * @return string
         * @since 13.0.0
         */
        public string getLabel()
        {
            return this.label;
        }

        /**
         * @param type
         * @return string
         * @throws \InvalidArgumentException
         * @since 13.0.0
         */
        protected string getValidatedType(string type)
        {
            type = type.Trim();
            if(type == "") {
                throw new InvalidArgumentException("Type must not be empty");
            }

            if(type == "exact") {
                throw new InvalidArgumentException("Provided type is a reserved word");
            }

            return type;
        }
    }

}