using System;
namespace OCP.FullTextSearch.Model
{
    /**
     * @since 16.0.0
     *
     * Interface ISearchOption
     *
     * @package OCP\FullTextSearch\Model
     */
    interface ISearchOption
    {


        /**
         * @since 16.0.0
         */
        const CHECKBOX = 'checkbox';

        /**
         * @since 16.0.0
         */
        const INPUT = 'input';

        /**
         * @since 16.0.0
         */
        const INPUT_SMALL = 'small';


        /**
         * Set the name/key of the option.
         * The string should only contains alphanumerical chars and underscore.
         * The key can be retrieve when using ISearchRequest::getOption
         *
         * @see ISearchRequest::getOption
         *
         * @since 16.0.0
         *
         * @param string name
         *
         * @return ISearchOption
         */
        public function setName(string name): ISearchOption;

    /**
     * Get the name/key of the option.
     *
     * @since 16.0.0
     *
     * @return string
     */
    public function getName(): string;


    /**
     * Set the title/display name of the option.
     *
     * @since 16.0.0
     *
     * @param string title
     *
     * @return ISearchOption
     */
    public function setTitle(string title): ISearchOption;

    /**
     * Get the title of the option.
     *
     * @since 16.0.0
     *
     * @return string
     */
    public function getTitle(): string;


    /**
     * Set the type of the option.
     * type can be ISearchOption::CHECKBOX or ISearchOption::INPUT
     *
     * @since 16.0.0
     *
     * @param string type
     *
     * @return ISearchOption
     */
    public function setType(string type): ISearchOption;

    /**
     * Get the type of the option.
     *
     * @since 16.0.0
     *
     * @return string
     */
    public function getType(): string;


    /**
     * In case of Type is INPUT, set the size of the input field.
     * Value can be ISearchOption::INPUT_SMALL or not defined.
     *
     * @since 16.0.0
     *
     * @param string size
     *
     * @return ISearchOption
     */
    public function setSize(string size): ISearchOption;

    /**
     * Get the size of the INPUT.
     *
     * @since 16.0.0
     *
     * @return string
     */
    public function getSize(): string;


    /**
     * In case of Type is , set the placeholder to be displayed in the input
     * field.
     *
     * @since 16.0.0
     *
     * @param string placeholder
     *
     * @return ISearchOption
     */
    public function setPlaceholder(string placeholder): ISearchOption;

    /**
     * Get the placeholder.
     *
     * @since 16.0.0
     *
     * @return string
     */
    public function getPlaceholder(): string;

}

}
