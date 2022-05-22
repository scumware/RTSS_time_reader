using System;
using System.Diagnostics;
using System.Windows.Forms;
using RTSS_time_reader.RTSS_interop;

namespace RTSS_time_reader
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if DEBUG
                        var osd = new OSD("debug && test");
                        var exitingOSDEntries = osd.GetExitingOSDEntries();
                        foreach (var exitingOSDEntry in exitingOSDEntries)
                        {
                            Debug.WriteLine($"owner={exitingOSDEntry.Owner}\ttext={exitingOSDEntry.Text}");
                        }
                        Debug.WriteLine("");

                        var appEntries = osd.GetAppEntries();
                        foreach (var appEntry in appEntries)
                        {
                            Debug.WriteLine($"{appEntry.Name}\t{appEntry.Flags}");
                        }

                        Debugger.Break();
#endif

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var mainForm = new MainForm();
            Application.Run(mainForm);
        }
    }
}
