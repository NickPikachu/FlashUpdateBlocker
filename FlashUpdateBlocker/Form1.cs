using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace FlashUpdateBlocker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 添加 hosts 屏蔽 Flash 在线更新检查
            string hostsFilePath = @"C:\Windows\System32\drivers\etc\hosts";
            string contentToAdd = "127.0.0.1 geo2.adobe.com\r\n127.0.0.1 fpdownload2.macromedia.com\r\n127.0.0.1 fpdownload.macromedia.com\r\n127.0.0.1 macromedia.com";

            try
            {
                using (StreamWriter sw = File.AppendText(hostsFilePath))
                {
                    sw.WriteLine(contentToAdd);
                }

                // 重载网络配置以应用 hosts 配置
                RunCommand("ipconfig", "/flushdns");

                DeleteAndCreateSettingsSol();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unknown error occurred: " + ex.Message);
            }
        }

        private void RunCommand(string command, string arguments)
        {
            // 创建命令行执行命令
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = command;
            processStartInfo.Arguments = arguments;
            processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            using (Process process = new Process())
            {
                process.StartInfo = processStartInfo;
                process.Start();
                process.WaitForExit();
            }
        }

        private void DeleteAndCreateSettingsSol()
        {
            // 删除并锁定 Flash 最新版本配置文件防止提示更新
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Macromedia\Flash Player\macromedia.com\support\flashplayer\sys");
            string file1Path = Path.Combine(folderPath, "settings.sol");
            string file2Path = Path.Combine(folderPath, "#local\\settings.sol");

            try
            {
                if (File.Exists(file1Path))
                {
                    File.Delete(file1Path);
                }

                if (File.Exists(file2Path))
                {
                    File.Delete(file2Path);
                }

                using (FileStream fs = File.Create(file1Path))
                {
                    fs.Close();
                }

                File.SetAttributes(file1Path, FileAttributes.ReadOnly);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unknown error occurred: " + ex.Message);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // 觉得有用不妨 Star 一下
            System.Diagnostics.Process.Start("https://github.com/NickPikachu/FlashUpdateBlocker");

        }
    }
}
