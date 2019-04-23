using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Console
{
    /**
     * Class ConsoleEvent
     *
     * @package OCP\Console
     * @since 9.0.0
     */
    public class ConsoleEvent : ext.Event
    {

	const string EVENT_RUN = @"OC\Console\Application::run";

    /** @var string */
    protected string @event;

    /** @var string[] */
    protected IList<string> arguments;

	/**
	 * DispatcherEvent constructor.
	 *
	 * @param string event
	 * @param string[] arguments
	 * @since 9.0.0
	 */
	public ConsoleEvent(string @event, IList<string> arguments)
    {
            this.@event = @event;
            this.arguments = arguments;
    }

    /**
	 * @return string
	 * @since 9.0.0
	 */
    public string getEvent()
    {
        return this.@event;
    }

    /**
	 * @return string[]
	 * @since 9.0.0
	 */
    public IList<string> getArguments()
    {
        return this.arguments;
    }
}

}
