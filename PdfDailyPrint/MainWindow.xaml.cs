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
using System.Windows.Threading;

namespace PdfDailyPrint
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private string alreadyPrintedFilesTxtName = "打印历史记录.txt";
        private List<string> alreadyPrintedFiles = new List<string>();
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PrintPdf(string fileName)
        {
            try
            {
                using (Process p = new Process())
                {
                    string currentFile = fileName;
                    this.txtCurrentFileName.Text = currentFile;
                    p.StartInfo = new ProcessStartInfo()
                    {
                        CreateNoWindow = true,
                        Verb = "print",
                        FileName = fileName
                    };
                    p.Start();
                    this.alreadyPrintedFiles.Add(fileName);
                    File.AppendAllLines(this.alreadyPrintedFilesTxtName, new string[] { fileName });
                    p.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                this.txtMsg.Text = ex.Message;
            }
        }


        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            string[] pdfFiles = Directory.GetFiles(Environment.CurrentDirectory, "*.pdf");
            if (pdfFiles.Length <= 0)
            {
                this.txtMsg.Text = "未找到任何pdf文件";

            }
            else
            {
                var toPrintFiles = pdfFiles.Except(this.alreadyPrintedFiles);
                if (toPrintFiles.Any())
                {
                    this.txtMsg.Text = $"正在准备依次打印{toPrintFiles.Count()}个pdf文件";
                    foreach (string pdf in pdfFiles.Except(this.alreadyPrintedFiles))
                    {
                        PrintPdf(pdf);
                    }
                }
                this.txtMsg.Text = "正在等待下次扫描（5秒一次）";
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

#if DEBUG
            if (DateTime.Now > new DateTime(2020, 7, 11))
                Application.Current.Shutdown();
#endif
            if (File.Exists(this.alreadyPrintedFilesTxtName))
                this.alreadyPrintedFiles = File.ReadAllLines(this.alreadyPrintedFilesTxtName).Select(x => x.Trim()).ToList();

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            dispatcherTimer.Start();
        }
    }
}
