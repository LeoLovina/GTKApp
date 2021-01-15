using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;

namespace GTKApp
{
    public class MyWindow : Window
    {
        [UI] private Label _label1 = null;
        [UI] private Button _button1 = null;

        [UI] private Box MainBox = null;
        [UI] private Statusbar myStatusbar = null;

        [UI] private MenuButton mbtnMenuButton = null;

        [UI] private Button btnDialog = null;
        private int _counter;
        private uint timeoutHandler;

        public MyWindow() : this(new Builder("MyWindow.glade")) { }

        private MyWindow(Builder builder) : base(builder.GetObject("MyWindow").Handle)
        {
            builder.Autoconnect(this);

            DeleteEvent += Window_DeleteEvent;
            _button1.Clicked += Button1_Clicked;

            btnDialog.Clicked += btnDialog_clicked;

            // create a timer to update status bar
            timeoutHandler = GLib.Timeout.Add(1000, new GLib.TimeoutHandler(UpdateStatus));

            // var menuButton = new Gtk.MenuButton();
            // menuButton.SetSizeRequest(80,35);

            // var menumodel = new Gtk.MenuItem();
            // menumodel.Label = "menu item";
            // menumodel.Activated += mbtnMenuButton_clicked;
            // menuButton.Add(menumodel);
            // menuButton.Show();
            // MainBox.Add(menuButton);

            // setup mbtnMenuButton
            // a menu with two actions
            //var menumodel = new Gio.Menu();
            // menumodel.Add("New", "app.new");
            // menumodel.Append("About", "win.about");

            // mbtnMenuButton.
        }

        private bool UpdateStatus()
        {
            var contextId = myStatusbar.GetContextId("message");
            myStatusbar.Pop(contextId);
            myStatusbar.Push(contextId, $"Current:{DateTime.Now.ToString()}");
            return true;
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            GLib.Timeout.Remove(timeoutHandler);
            //Application.Quit();
        }

        private void Button1_Clicked(object sender, EventArgs a)
        {
            // create a new button
            var button = new Button("btnTest");
            button.Clicked += btnTest_Clicked;
            button.Show();
            MainBox.Add(button);

            _counter++;
            _label1.Text = "Hello World! This button has been clicked " + _counter + " time(s).";
        }

        private void btnTest_Clicked(object sender, EventArgs a)
        {
            FindChild("haha");
            Gtk.Application.Invoke(
                delegate
                {
                    _label1.Text = "Test Button Clicked";
                }
            );
        }

        private void FindChild(string id)
        {
            foreach (var child in MainBox.Children)
            {
                Console.WriteLine($"Name = {child.Name}");
            }
        }

        private void mbtnMenuButton_clicked(object sender, EventArgs a)
        {
            Console.WriteLine("mbtnMenuButton_clicked");
        }

        private void btnDialog_clicked(object sender, EventArgs a)
        {
            using (var dialog = new Gtk.Dialog("This is a dialog",
                                         this,
                                         DialogFlags.Modal
                                         , Gtk.Stock.Open))
            {
                dialog.AddButton("Cancel", ResponseType.Cancel);
                dialog.AddButton("Ok", ResponseType.Ok);
                dialog.SetSizeRequest(400, 200);

                var checkbox = new Gtk.CheckButton("This is a checkbox");
                checkbox.Show();
                var lable = new Gtk.Label("NIce label");
                lable.Show();
                
                dialog.ContentArea.Add(lable);
                // dialog.ActionArea.PackStart(checkbox, false, false, 0);
                //dialog.ActionArea.PackEnd(checkbox, false, false,0);
                var response = dialog.Run();
                Console.WriteLine($"btnDialog_clicked response={response}");
            }
        }

    }
}
