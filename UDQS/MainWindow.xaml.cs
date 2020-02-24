using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UDQS.Control;
using UDQS.Models;

namespace UDQS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<FileInformation> fileInformation = new List<FileInformation>();
        string url = System.IO.Path.GetDirectoryName(Application.ResourceAssembly.Location);
        ConectionControl conectionControl = new ConectionControl();
        public MainWindow()
        {
            InitializeComponent();
            GetAllFile();
            fileInformation = conectionControl.ComparNewFile(fileInformation);
            foreach (var item in fileInformation)
            {
                conectionControl.DonwloadFile(item, url);
            }
            //System.Diagnostics.ProcessStartInfo processStartInfo = new System.Diagnostics.ProcessStartInfo(url + "\\wpf.exe");
            if(File.Exists(url + "\\wpf.exe"))
                System.Diagnostics.Process.Start(url + "\\wpf.exe");
            Environment.Exit(0);
        }
        private void GetAllFile()
        {
            string[] allfiles = Directory.GetFiles(url, "*.*", SearchOption.AllDirectories);
            foreach (string item in allfiles)
            {
                if (File.Exists(item))
                {
                    var fi1 = new FileInfo(item);
                    fileInformation.Add(new FileInformation()
                    {
                        Name = item.Replace(url, ""),
                        DateModified = fi1.LastWriteTime,
                        length = fi1.Length
                    }); 
                }
            }
        }

    }
}
