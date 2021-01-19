using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;

namespace GTKApp
{
    class MainWindow : Window
    {
        // [UI] private Label _label1 = null;
        // [UI] private Button btnTest = null;

        [UI] private Box MainBox = null;
        [UI] private Statusbar ssStatusbar = null;
        // private int _counter;

        public MainWindow() : this(new Builder("MainWindow.glade")) { }

        private MainWindow(Builder builder) : base(builder.GetObject("MainWindow").Handle)
        {
            builder.Autoconnect(this);

            DeleteEvent += Window_DeleteEvent;
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }

        private void mniOpen_Click(object sender, EventArgs a)
        {
            Console.WriteLine("mniOpen_Click");
        }

        private void on_btnTest_clicked(object sender, EventArgs a)
        {

            // var myDialog = new MyDialog();
            // myDialog.Show();
            // MainBox.Add(myDialog);
            var myWindow = new MyWindow();
            myWindow.Show();
            MainBox.Add(myWindow);

            // _counter++;
            // _label1.Text = "Hello World! This button has been clicked " + _counter + " time(s).";
        }
    }
}
