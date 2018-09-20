using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Covariant_Script_Deployer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string GetMD5HashFromFile(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, System.IO.FileMode.Open);
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }

        private string RemoveSpace(string str)
        {
            string nstr = "";
            StringReader reader = new StringReader(str);
            while (reader.Peek() > -1)
                nstr += reader.ReadLine();
            return nstr;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                textBox2.Text = folderBrowserDialog1.SelectedPath;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                textBox3.Text = openFileDialog1.FileName;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
            Environment.Exit(0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
            string[] dist_info = RemoveSpace(File.ReadAllText(textBox3.Text)).Split(';');
            double progress = 0;
            foreach (string info in dist_info)
            {
                string path = info.Split('@')[0].Replace('/', '\\');
                if (path.Length != 0)
                {
                    label6.Text = "正在部署..." + (progress / dist_info.Length) * 100 + "%";
                    Application.DoEvents();
                    path = textBox2.Text + "\\" + path;
                    File.WriteAllText(path + ".md5", GetMD5HashFromFile(path));
                }
                ++progress;
            }
            label6.Text = "完成";
            button3.Enabled = true;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://covscript.org/");
        }
    }
}
