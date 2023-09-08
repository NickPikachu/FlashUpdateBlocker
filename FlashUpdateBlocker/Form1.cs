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
            DialogResult result = MessageBox.Show("Are you sure you want to block the Flash live update check?", "Second confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                // 添加 hosts 屏蔽 Flash 在线更新检查
                string hostsFilePath = @"C:\Windows\System32\drivers\etc\hosts";
                string contentToAdd = "127.0.0.1 geo2.adobe.com\r\n127.0.0.1 fpdownload2.macromedia.com\r\n127.0.0.1 fpdownload.macromedia.com\r\n127.0.0.1 macromedia.com";

                    using (StreamWriter sw = File.AppendText(hostsFilePath))
                    {
                        sw.WriteLine(contentToAdd);
                    }

                    // 重载网络配置以应用 hosts 配置
                    RunCommand("ipconfig", "/flushdns");

                    DeleteSettingsSol();

                    MessageBox.Show("Blocking update successful!");

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 添加 hosts 屏蔽 FF新推荐 数据获取途径
            string hostsFilePath = @"C:\Windows\System32\drivers\etc\hosts";
            string contentToAdd = "0.0.0.0 mini.ffnews.cn\r\n0.0.0.0 tongji.flash.cn\r\n0.0.0.0 mini.flash.2144.com";

            using (StreamWriter sw = File.AppendText(hostsFilePath))
            {
                sw.WriteLine(contentToAdd);
            }

            // 

            RunCommand("sc", "config \"Flash Helper Service\" start= disabled");

            MessageBox.Show("Blocking FF News successful!");
        }

        private void RunCommand(string command, string arguments)
        {
            // 创建命令行执行命令
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = command,
                Arguments = arguments,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            using (Process process = new Process())
            {
                process.StartInfo = processStartInfo;
                process.Start();
                process.WaitForExit();
            }
        }

        private void DeleteSettingsSol()
        {
            // 删除并锁定 Flash 最新版本配置文件防止提示更新
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Macromedia\Flash Player\macromedia.com\support\flashplayer\sys");
            string file1Path = Path.Combine(folderPath, "settings.sol");
            string file2Path = Path.Combine(folderPath, "#local\\settings.sol");
            string file3Path = Path.Combine(folderPath, "settings.sxx");
            if (File.Exists(file1Path))
            {
                bool file1ReadOnly = (File.GetAttributes(file1Path) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
                if (!file1ReadOnly || (file1ReadOnly && new FileInfo(file1Path).Length > 0))
                {
                    File.Delete(file1Path);
                    using (FileStream fs = File.Create(file1Path))
                    {
                        fs.Close();
                    }
                    File.SetAttributes(file1Path, FileAttributes.ReadOnly);
                }
            }

            if (File.Exists(file2Path))
            {
                bool file2ReadOnly = (File.GetAttributes(file2Path) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
                if (!file2ReadOnly || (file2ReadOnly && new FileInfo(file2Path).Length > 0))
                {
                    File.Delete(file2Path);
                    using (FileStream fs = File.Create(file2Path))
                    {
                        fs.Close();
                    }
                    File.SetAttributes(file2Path, FileAttributes.ReadOnly);
                }
            }

            if (File.Exists(file3Path))
            {
                bool file3ReadOnly = (File.GetAttributes(file3Path) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
                if (!file3ReadOnly || (file3ReadOnly && new FileInfo(file3Path).Length > 0))
                {
                    File.Delete(file3Path);
                    using (FileStream fs = File.Create(file3Path))
                    {
                        fs.Close();
                    }
                    File.SetAttributes(file3Path, FileAttributes.ReadOnly);
                }
            }
        }


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // 觉得有用不妨 Star 一下
            System.Diagnostics.Process.Start("https://github.com/NickPikachu/FlashUpdateBlocker");

        }
    }
}
