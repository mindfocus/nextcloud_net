namespace OCP.AppFramework.Http.Template
{
/**
 * Interface IMenuAction
 *
 * @package OCP\AppFramework\Http\Template
 * @since 14.0
 */
    interface IMenuAction {

        /**
         * @since 14.0.0
         * @return string
         */
        public function getId(): string;

        /**
         * @since 14.0.0
         * @return string
         */
        public function getLabel(): string;

        /**
         * @since 14.0.0
         * @return string
         */
        public function getLink(): string;

        /**
         * @since 14.0.0
         * @return int
         */
        public function getPriority(): int;

        /**
         * @since 14.0.0
         * @return string
         */
        public function render(): string;

    }
}