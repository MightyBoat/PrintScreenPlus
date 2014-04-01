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
       
        
        


        private void CaptureMyScreen()
        {

            try
            {
                DateTime theDate1 = DateTime.Now;

                theDate1.ToString("D");

                string targetPath = (@"C:\Users\Mike\Projects\Visual Studio projects\icon test\");

                Bitmap captureBitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);


                Graphics captureGraphics = Graphics.FromImage(captureBitmap as Image);

                captureGraphics.CopyFromScreen(0, 0, 0, 0, captureBitmap.Size);

                captureBitmap.Save(targetPath);

                MessageBox.Show("Screen Captured");

            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }

        }
        private void SaveLocation()
        {
            Stream myStream = null;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.InitialDirectory = "c:\\";
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = saveFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            // Insert code to read the stream here.
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
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