using OCP.ContactsNs.ContactsMenu;

namespace OC.Contacts.ContactsMenu.Actions
{
    class LinkAction : ILinkAction {

    /** @var string */
    private string icon;

    /** @var string */
    private string name;

    /** @var string */
    private string href;

    /** @var int */
    private int priority = 10;

    /**
     * @param string icon absolute URI to an icon
     */
    public void setIcon(string icon) {
        this.icon = icon;
    }

    /**
     * @param string name
     */
    public void setName(string name) {
        this.name = name;
    }

    /**
     * @return string
     */
    public string getName() {
        return this.name;
    }

    /**
     * @param int priority
     */
    public void setPriority(int priority) {
        this.priority = priority;
    }

    /**
     * @return int
     */
    public int getPriority() {
        return this.priority;
    }

    /**
     * @param string href
     */
    public void setHref(string href) {
        this.href = href;
    }

    /**
     * @return string
     */
    public string getHref() {
        return this.href;
    }
//    /**
//     * @return array
//     */
//    public function jsonSerialize() {
//        return [
//        "title" => this.name,
//        "icon" => this.icon,
//        "hyperlink" => this.href,
//            ];
//    }

    }
}