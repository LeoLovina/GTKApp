using System;
using Gtk;

namespace GTKApp
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Application.Init();

            var app = new Application("org.GTKApp.GTKApp", GLib.ApplicationFlags.None);
            app.Register(GLib.Cancellable.Current);

            // setup global handler 
            GLib.ExceptionManager.UnhandledException +=  OnGlobalException;
            var win = new MainWindow();
            app.AddWindow(win);

            win.Show();
            Application.Run();
        }

        static void OnGlobalException (GLib.UnhandledExceptionArgs args)
        {
            Console.Write("OnGlobalException");
        }
    }
}
