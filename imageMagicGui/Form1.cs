using MyNetLib.com.gd67.form.ui;
using MyNetLib.com.gd67.shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace imageMagicGui
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        void appendLog(string log)
        {
            UIHelper.appendRTText(richTextBox1, log);
            UIHelper.appendRTText(richTextBox1, "\n");
            UIHelper.invokeOrCall(richTextBox1, delegate () {
                richTextBox1.ScrollToCaret();
            });
        }
        bool converting = false;
        private void button1_Click(object sender, EventArgs e)
        {
            if (converting)
            {
                appendLog("正在转换中");
                return;
            }
            var rst= openFileDialog1.ShowDialog();
            if(DialogResult.OK == rst)
            {
                converting = true;
                button1.Text = "转换中";
                textBox1.Text = openFileDialog1.FileName;
                string targetDir = textBox1.Text + "-images";
                Directory.CreateDirectory(targetDir);
                string cdir= Environment.CurrentDirectory;
                if (cdir.EndsWith("Debug") || cdir.EndsWith("Release"))
                {
                    cdir = Path.GetDirectoryName(cdir);
                    cdir = Path.GetDirectoryName(cdir);
                }
                String cmd = cdir+"\\ImageMagick-7.0.9-Q16\\magick.exe";
                string args= "-density 150 " + textBox1.Text + " " + targetDir + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(textBox1.Text) + "-%03d.png";
                appendLog(cmd + " " +args);

                new Thread(new ThreadStart(delegate ()
                {
                    var r = RunCommand.runcommoand(cmd, args, Path.GetDirectoryName(targetDir));
                    appendLog("转换完成" + r);
                    RunCommand.startprogram(targetDir, "");
                    converting = false;
                    UIHelper.setviewText(button1, "打开pdf");
                })).Start();

                
            }
        }
    }
}
