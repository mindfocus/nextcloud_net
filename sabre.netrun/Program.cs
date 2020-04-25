using Pchp.Core.Utilities;
using System;

namespace sabre.netrun
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var ctx = ContextExtensions.CurrentContext;
            var runc = new Sabre.Uri.UriFunc(ctx);
            var result = runc.normalize("baidu.com");
            Console.WriteLine(result);
            var vcalendar = new Sabre.VObject.ComponentNs.VCalendar();
            Console.WriteLine(vcalendar.serialize().AsString(ctx));
        }
    }
}