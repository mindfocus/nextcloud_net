using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Dashboard.Model
{
    public class WidgetDelayedJob
    {
        public string function { get; internal set; }
        public int delay { get; internal set; }

        public WidgetDelayedJob(string function, int delay)
        {
            this.function = function;
            this.delay = delay;
        }
    }
}
