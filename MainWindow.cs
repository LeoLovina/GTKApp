using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;

namespace GTKApp
{
    class MainWindow : Window
    {
        [UI] private Label _label1 = null;
        [UI] private Button _button1 = null;

        [UI] private Box MainBox = null;
        private int _counter;

        public MainWindow() : this(new Builder("MainWindow.glade")) { }

        private MainWindow(Builder builder) : base(builder.GetObject("MainWindow").Handle)
        {
            builder.Autoconnect(this);

            DeleteEvent += Window_DeleteEvent;
            _button1.Clicked += Button1_Clicked;
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }

        private void Button1_Clicked(object sender, EventArgs a)
        {
            var myWindow = new MyWindow();
            myWindow.Show();
            MainBox.Add(myWindow);

            _counter++;
            _label1.Text = "Hello World! This button has been clicked " + _counter + " time(s).";
        }
    }
}
