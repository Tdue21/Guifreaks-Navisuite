using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Linq;
using Guifreaks.Navisuite;

namespace Guifreaks.NavisuiteDemo
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            var colors = new NaviColorTableOff10Black();

            var root = new XElement("NaviColorTable");
            var document = new XDocument(new XDeclaration("1.0", "utf-8", null), root);

            var properties = colors.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
            var values = new Dictionary<string, object>();
            foreach (var prop in properties)
            {
                var name = prop.Name;
                var value = prop.GetValue(colors);
                values.Add(name, value);
            }


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }
    }
}
