using Encrypter.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Encrypter
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            File.Delete("Guna.UI2.dll");
            File.WriteAllBytes("Guna.UI2.dll", Resources.Guna_UI2);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Encrypter());
        }
    }
}
