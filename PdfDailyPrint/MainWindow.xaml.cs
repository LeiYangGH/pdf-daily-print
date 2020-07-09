using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace PdfDailyPrint
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PrintPdf(string fileName)
        {
            try
            {
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo()
                {
                    CreateNoWindow = true,
                    Verb = "print",
                    FileName = fileName //put the correct path here
                };
                p.Start();
            }
            catch (Exception ex)
            {
                this.txtMsg.Text = ex.Message;
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

#if DEBUG
            if (DateTime.Now > new DateTime(2020, 7, 11))
                Application.Current.Shutdown();
#endif
            string[] pdfFiles = Directory.GetFiles(Environment.CurrentDirectory, "*.pdf");
            if (pdfFiles.Length <= 0)
            {
                this.txtMsg.Text = "未找到任何pdf文件";
                return;
            }
            else
            {
                string currentFile = pdfFiles[0];
                this.txtCurrentFileName.Text = currentFile;
                PrintPdf(currentFile);
            }

        }
    }
}
