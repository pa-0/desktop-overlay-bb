using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;

namespace oos
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //[DllImport("user32.dll")]
        //public static extern int FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern int SendMessage(int hWnd, uint Msg, int wParam, int lParam);
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(String lpClassName, String lpWindowName);

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public static void bringToFront(Process process)
        {
            IntPtr handle = process.MainWindowHandle;
            
            if (handle == IntPtr.Zero)
            {
                return;
            }
            
            SetForegroundWindow(handle);
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(int hWnd, uint Msg, int wParam, int lParam);
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CloseExplorer();
            FullScreen();
            InitAppList();
        }

        List<ApplicationClass> aclist = new List<ApplicationClass>();
        private void InitAppList()
        {
            //tijdelijke opvulling
            aclist.Add(new ApplicationClass("XAMPP", "C:\\xampp\\xampp-control.exe", "xampp-control"));
            aclist.Add(new ApplicationClass("Notepad", "C:\\Windows\\System32\\notepad.exe", "notepad"));

            //Toevoegen aan de tijdelijke listbox
            appListBox.Items.Clear();
            foreach (ApplicationClass ac in aclist)
            {
                appListBox.Items.Add(ac.Title);
            }
        }

        private void FullScreen()
        {
            this.Top = 0;
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Left = 0;
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
        }

        private void CloseExplorer()
        {
            int hwnd;
            hwnd = (int)FindWindow("Progman", null);
            PostMessage(hwnd, /*WM_QUIT*/ 0x12, 0, 0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(System.IO.Path.Combine(Environment.GetEnvironmentVariable("windir"), "explorer.exe"));
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Process.Start("C:\\xampp\\xampp-control.exe");
            foreach (var process in Process.GetProcessesByName("xampp-control"))
            {
                bringToFront(process);
            }
        }

        private void appListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ListBoxItem_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            ApplicationClass ac = aclist[appListBox.SelectedIndex];
            //bool alreadyExists = false;
            foreach (var process in Process.GetProcessesByName(ac.ProcessName))
            {
                bringToFront(process);
                return;
            }
            Process.Start(ac.ExecPath);
            return;
        }
    }
}
