//Creating a System Tray Application with C#
//http://alanbondo.wordpress.com/2008/06/22/creating-a-system-tray-app-with-c/

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintScreenPlus
{
    public class SysTrayApp : Form
    {
        string SaveFolder;
                           
        public void CaptureMyScreen()
        {

            try
            {
                if (string.IsNullOrEmpty(SaveFolder)) 
                {
                    SaveFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                };
                
                string targetPath = string.Format("{0}{1}text-{2:yyyy-MM-dd_hh-mm-ss-tt}.png", SaveFolder, Path.DirectorySeparatorChar, DateTime.Now);

                Bitmap captureBitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);


                Graphics captureGraphics = Graphics.FromImage(captureBitmap as Image);

                captureGraphics.CopyFromScreen(0, 0, 0, 0, captureBitmap.Size);

                captureBitmap.Save(targetPath);

            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }

        }
        public string SaveLocation()
        { 
            
            // This line calls the folder dialog
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            // This is what will execute if the user selects a folder and hits OK (File if you change to FileBrowserDialog)
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SaveFolder = fbd.SelectedPath;
                            
            }
            else
            {
               // This prevents a crash when you close out of the window with nothing
               SaveFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
            
            return SaveFolder;
            
        }

        [STAThread]
        public static void Main()
        {
            Application.Run(new SysTrayApp());
        }

        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;

        public SysTrayApp()
        {
            // Create a simple tray menu with only one item.
            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Capture", CaptureIt);
            trayMenu.MenuItems.Add("Config", Config);
            trayMenu.MenuItems.Add("Exit", OnExit);
            

            // Create a tray icon. In this example we use a
            // standard system icon for simplicity, but you
            // can of course use your own custom icon too.
            trayIcon = new NotifyIcon();
            trayIcon.Text = "PrintScreenPlus";
            trayIcon.Icon = new Icon("monitor_plus.ico");

            // Add menu to tray icon and show it.
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.

            base.OnLoad(e);
        }

        private void CaptureIt(object sender, EventArgs e)
        {
            CaptureMyScreen();
        }
        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void Config(object sender, System.EventArgs e)
        {
            SaveLocation();
        }
        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                // Release the icon resource.
                trayIcon.Dispose();
            }

            base.Dispose(isDisposing);
        }

    }
}