using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using System.Collections.Generic;

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

        [UI] private SpinButton spButton = null;
        [UI] private Switch swSwitch = null;
        [UI] private ComboBox cbCombobox = null;

        [UI] private Grid gridDynamic = null;

        private int _counter;
        private uint timeoutHandler;

        public MyWindow() : this(new Builder("MyWindow.glade")) { }

        private MyWindow(Builder builder) : base(builder.GetObject("MyWindow").Handle)
        {
            builder.Autoconnect(this);

            DeleteEvent += Window_DeleteEvent;
            _button1.Clicked += Button1_Clicked;

            btnDialog.Clicked += btnDialog_clicked;
            swSwitch.StateChanged += StateChangedHandler;

            SetupCombobox();
            SetupButton();
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

        private void SetupCombobox()
        {
            List<string> items = new List<string>() { "One", "Two", "Three" };
            var combo = new ComboBox(items.ToArray());
            combo.Changed += ComboChangedEventHandler;
            combo.Show();
            gridDynamic.Attach(combo, 0, 0, 1, 1);
        }

        private void SetupButton()
        {
            Box box = new Box(Orientation.Vertical, 2);
            Button btn1 = new Button("gtk-about");
            box.Add(btn1);
            box.ShowAll();
            gridDynamic.Attach(box, 1, 0, 1, 1);
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
            Gtk.Application.Invoke(
                delegate
                {
                    _label1.Text = "Test Button Clicked";
                }
            );
        }

        private Widget FindChild(Container container, string widgetName)
        {
            Widget result = null;
            foreach (var child in container.Children)
            {
                if (child.Name == widgetName)
                {
                    result = child;
                    break;
                }
            }
            return result;
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

        private void on_rbtnYes_toggled(object sender, EventArgs a)
        {
            RadioButton rb = sender as RadioButton;
            Console.WriteLine($"on_rbtnYes_toggled {rb.Active}");
        }

        private void on_rbtnNo_toggled(object sender, EventArgs a)
        {
            RadioButton rb = sender as RadioButton;
            Console.WriteLine($"on_rbtnNo_toggled {rb.Active}");
        }
        private void on_rbtnNone_toggled(object sender, EventArgs a)
        {
            RadioButton rb = sender as RadioButton;
            Console.WriteLine($"on_rbtnNone_toggled {rb.Active}");
        }
        private void on_chkHaha_toggled(object sender, EventArgs a)
        {
            CheckButton cb = sender as CheckButton;
            Console.WriteLine($"on_chkHaha_toggled {cb.Active}");
        }
        private void on_tgButton_toggled(object sender, EventArgs a)
        {
            ToggleButton tg = sender as ToggleButton;
            Console.WriteLine($"on_tgButton_toggled {tg.Active}");
            Console.WriteLine($"spButton value: {spButton.Value}");
            swSwitch.State = !swSwitch.State;
        }

        // Note: 
        // StateSetHandler doesn't work propertly
        // StateChangedHandler is called when the control gets focus.
        private void StateChangedHandler(object sender, StateChangedArgs args)
        {
            Switch sw = sender as Switch;
            Console.WriteLine($"StateChangedHandler swSwitch State {sw.State}");
        }

        private void ComboChangedEventHandler(object? sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            Console.WriteLine($"ComboChangedEventHandler ComboBox seelcted index = {cb.Active}");
        }

        private void on_cbCombobox_changed(object? sender, EventArgs e)
        {
            // https://en.wikibooks.org/wiki/GTK%2B_By_Example/Tree_View/Tree_Models
            ComboBox cb = sender as ComboBox;
            Gtk.TreeIter iter;
            GLib.Value title = new GLib.Value();
            GLib.Value id = new GLib.Value();
            var item = cb.Model.IterNthChild(out iter, cb.Active);
            cb.Model.GetValue(iter, 0, ref title);
            cb.Model.GetValue(iter, 1, ref id);

            Console.WriteLine($"on_cbCombobox_changed ComboBox selcted index = {cb.Active} title={title.Val} id={id.Val}");
        }
        // 
        private void on_cbEntry_changed(object sender, EventArgs e)
        {
            Entry en = sender as Entry;
            Console.WriteLine($"on_cbEntry_changed ComboBox selcted Text={en.Text}");
        }

        private void on_colorButton_color_set(object sender, EventArgs e)
        {
            ColorButton cb = sender as ColorButton;
            Console.WriteLine($"on_colorButton_color_set ComboBox selcted Blue={cb.Color.Blue}");
        }


        private void on_fileChoose_file_set(object sender, EventArgs e)
        {
            FileChooserButton file = sender as FileChooserButton;
            Console.WriteLine($"on_fileChoose_file_set file ={file.File}");
        }
        //  

        private void on_actAlert_activate(object sender, EventArgs e)
        {
            //             FileChooserButton file = sender as FileChooserButton;
            Console.WriteLine($"on_actAlert_activate ");
        }

        //private void on_btnColor_color_set(object sender, EventArgs e)
        private void on_btnColor_color_set(object sender, EventArgs e)
        {
            ColorButton btn = sender as ColorButton;
            var rgba = btn.Rgba;
            var parent = btn.Parent;
            var parentFix = btn.Parent as Fixed;
            var lbColor = FindChild(parentFix, "lbColor"); 
            if (lbColor != null)
                lbColor.ModifyBg(StateType.Normal, new Gdk.Color() { Red = (ushort)rgba.Red, Green = (ushort)rgba.Green, Blue = (ushort)rgba.Blue });
            parent.ModifyBg(StateType.Normal, new Gdk.Color() { Red = (ushort)rgba.Red, Green = (ushort)rgba.Green, Blue = (ushort)rgba.Blue });
            Console.WriteLine($"on_btnColor_color_set {rgba.Red} {rgba.Green} {rgba.Blue} {rgba.Alpha}");
        }

    }
}
