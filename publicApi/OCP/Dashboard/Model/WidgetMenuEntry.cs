using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Dashboard.Model
{
    public sealed class WidgetMenuEntry
    {
        public string function { get; internal set; }
        public string icon{ get; internal set; }
        public string text{ get; internal set; }

        public WidgetMenuEntry(string function, string icon, string text)
        {
            this.function = function;
            this.icon = icon;
            this.text = text;
        }
        public WidgetMenuEntry()
        {
            this.function = "";
            this.text = "";
            this.icon = "";
        }
    }
}
