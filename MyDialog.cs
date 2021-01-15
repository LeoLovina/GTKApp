using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;

namespace GTKApp
{
    class MyDialog : Window
    {

        public MyDialog() : this(new Builder("MyDialog.glade")) { }

        private MyDialog(Builder builder) : base(builder.GetObject("MyDialog").Handle)
        {
            builder.Autoconnect(this);

            DeleteEvent += Window_DeleteEvent;
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
        }
    }
}
