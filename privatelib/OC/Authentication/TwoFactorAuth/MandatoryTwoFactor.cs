using System.Collections.Generic;
using System.Linq;
using OCP;

namespace OC.Authentication.TwoFactorAuth
{
    public class MandatoryTwoFactor
    {
        /** @var IConfig */
        private IConfig config;

        /** @var IGroupManager */
        private IGroupManager groupManager;

        public MandatoryTwoFactor(IConfig config, IGroupManager groupManager)
        {
            this.config = config;
            this.groupManager = groupManager;
        }

        /**
         * Get the state of enforced two-factor auth
         */
        public EnforcementState getState()
        {
            return new EnforcementState(
                (bool) this.config.getSystemValue("twofactor_enforced", false),
                (IList<string>) this.config.getSystemValue("twofactor_enforced_groups", new List<string>()),
                (IList<string>) this.config.getSystemValue("twofactor_enforced_excluded_groups", new List<string>())
            );
        }

        /**
         * Set the state of enforced two-factor auth
         */
        public void setState(EnforcementState state)
        {
            this.config.setSystemValue("twofactor_enforced", state.isEnforced() ? "true" : "false");
            this.config.setSystemValue("twofactor_enforced_groups", state.getEnforcedGroups());
            this.config.setSystemValue("twofactor_enforced_excluded_groups", state.getExcludedGroups());
        }

        /**
         * Check if two-factor auth is enforced for a specific user
         *
         * The admin(s) can enforce two-factor auth system-wide, for certain groups only
         * and also have the option to exclude users of certain groups. This method will
         * check their membership of those groups.
         *
         * @param IUser user
         *
         * @return bool
         */
        public bool isEnforcedFor(IUser user)
        {
            var state = this.getState();
            if (!state.isEnforced())
            {
                return false;
            }

            var uid = user.getUID();

            /*
             * If there is a list of enforced groups, we only enforce 2FA for members of those groups.
             * For all the other users it is not enforced (overruling the excluded groups list).
             */
            if (state.getEnforcedGroups().Any())
            {
                foreach (var group in state.getEnforcedGroups())
                {
                    if (this.groupManager.isInGroup(uid, group))
                    {
                        return true;
                    }
                }

                // Not a member of any of these groups . no 2FA enforced
                return false;
            }

            /**
             * If the user is member of an excluded group, 2FA won"t be enforced.
             */
            foreach (var group in state.getExcludedGroups())
            {
                if (this.groupManager.isInGroup(uid, group))
                {
                    return false;
                }
            }

            /**
             * No enforced groups configured and user not member of an excluded groups,
             * so 2FA is enforced.
             */
            return true;
        }
    }
}