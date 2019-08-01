using System.Collections.Generic;

namespace OC.Authentication.TwoFactorAuth
{
    public class EnforcementState
    {
        /** @var bool */
        private bool enforced;

        /** @var array */
        private IList<string> enforcedGroups;

        /** @var array */
        private IList<string> excludedGroups;

        /**
         * EnforcementState constructor.
         *
         * @param bool enforced
         * @param string[] enforcedGroups
         * @param string[] excludedGroups
         */
        public EnforcementState(bool enforced,
            IList<string> enforcedGroups,
            IList<string> excludedGroups)
        {
            this.enforced = enforced;
            this.enforcedGroups = enforcedGroups;
            this.excludedGroups = excludedGroups;
        }

        /**
         * @return string[]
         */
        public bool isEnforced()
        {
            return this.enforced;
        }

        /**
         * @return string[]
         */
        public IList<string> getEnforcedGroups()
        {
            return this.enforcedGroups;
        }

        /**
         * @return string[]
         */
        public IList<string> getExcludedGroups()
        {
            return this.excludedGroups;
        }

//        public function jsonSerialize(): array {
//            return [
//            'enforced' => this.enforced,
//            'enforcedGroups' => this.enforcedGroups,
//            'excludedGroups' => this.excludedGroups,
//                ];
//        }
    }
}