using System;
using System.Net;
using System.Windows.Forms;

namespace DesktopStreamDownloader
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                // Tls11 = 768, Tls12 = 3072, Tls13 = 12288 (.NET 3.5 only names Tls/1.0)
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls
                    | (SecurityProtocolType)768
                    | (SecurityProtocolType)3072
                    | (SecurityProtocolType)12288;
            }
            catch
            {
                // Keep system defaults on hosts that reject newer protocols.
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
