using System;
using System.Windows.Forms;
using NecromancerGame.Model;
using NecromancerGame.View;

namespace NecromancerGame
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MyForm(new Game()));
        }
    }
}