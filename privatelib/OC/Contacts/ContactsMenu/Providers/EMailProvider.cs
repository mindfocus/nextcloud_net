using ext;
using OCP;
using OCP.ContactsNs.ContactsMenu;

namespace OC.Contacts.ContactsMenu.Providers
{
    public class EMailProvider : IProvider {

    /** @var IActionFactory */
    private IActionFactory actionFactory;

    /** @var IURLGenerator */
    private IURLGenerator urlGenerator;

    /**
     * @param IActionFactory actionFactory
     * @param IURLGenerator urlGenerator
     */
    public EMailProvider(IActionFactory actionFactory, IURLGenerator urlGenerator) {
        this.actionFactory = actionFactory;
            this.urlGenerator = urlGenerator;
    }

    /**
     * @param IEntry entry
     */
    public void process(IEntry entry) {
        var iconUrl = this.urlGenerator.getAbsoluteURL(this.urlGenerator.imagePath("core", "actions/mail.svg"));
        foreach (var address in entry.getEMailAddresses() ) {
            if ( address.IsEmpty()) {
                // Skip
                continue;
            }
            var action = this.actionFactory.newEMailAction(iconUrl, address, address);
            entry.addAction(action);
        }
    }

    }
}