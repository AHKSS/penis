using System;
using System.IO;
using WeAreDevs_API;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace penis
{
    public partial class Form : System.Windows.Forms.Form
    {
        public Form()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.autoAttachS == true)
            {
                checkBox1.Checked = true;
                timer1.Enabled = true;
            }
            else
            {
                checkBox1.Checked = false;
                timer1.Enabled = false;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            if (Properties.Settings.Default.autoAttachS == true)
            {
                checkBox1.Checked = true;

            }
            else
            {
                checkBox1.Checked = false;
            }
        }

        bool AutoAttach = false;

        ExploitAPI api = new ExploitAPI();
        

        private void Populate(string path)
        {
            listBox1.Items.Clear();

            DirectoryInfo dinfo = new DirectoryInfo(Application.StartupPath + path);
            FileInfo[] Files = dinfo.GetFiles("*.txt");
            FileInfo[] Files1 = dinfo.GetFiles("*.lua");
            foreach (FileInfo file in Files)
            {
                listBox1.Items.Add(file.Name);
            }

            foreach (FileInfo file in Files1)
            {
                listBox1.Items.Add(file.Name);
            }
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!Directory.Exists("scripts"))
            {
                Directory.CreateDirectory("scripts");
            }

            object item = listBox1.SelectedItem;

            if (item != null)
            {
                string path = @"\scripts\";

                fastColoredTextBox1.Text = File.ReadAllText(Application.StartupPath + @"\scripts\" + item.ToString());

                Populate(path);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            api.SendLimitedLuaScript(fastColoredTextBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFileDialog = new OpenFileDialog();
            OpenFileDialog.FileName = "";
            OpenFileDialog.Filter = "Text Files|*.txt|LUA Files|*.lua|All Files (*.*) |*.*|Lua Files|*Lua";
            OpenFileDialog.ShowDialog();
            string text = File.ReadAllText(Path.GetFullPath(OpenFileDialog.FileName));
            this.fastColoredTextBox1.Text = text;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Txt Files (*.txt)|*.txt|Lua Files (*.lua)|*.lua|All Files (*.*)|*.*";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Stream s = sfd.OpenFile();
                StreamWriter sw = new StreamWriter(s);
                sw.Write(fastColoredTextBox1.Text);
                sw.Close();
                fastColoredTextBox1.Clear();
                
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            api.LaunchExploit();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked == true)
            {
                Properties.Settings.Default.autoAttachS = true;
                Properties.Settings.Default.Save();
                timer1.Enabled = true;
            }
            else if(checkBox1.Checked == false)
            {
                Properties.Settings.Default.autoAttachS = false;
                Properties.Settings.Default.Save();
                timer1.Enabled = false;
            }
                
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                if (AutoAttach == false)
                {
                    Process[] rbProcess = Process.GetProcessesByName("RobloxPlayerBeta");
                    if (rbProcess.Length == 1)
                    {
                        if (api.isAPIAttached() == false)
                        {
                            AutoAttach = true;
                            Task.Delay(5000);
                            api.LaunchExploit();

                        }
                    }
                }
                if (AutoAttach == true)
                {
                    Process[] rbProcess = Process.GetProcessesByName("RobloxPlayerBeta");
                    if (rbProcess.Length == 0)
                    {
                        if (api.isAPIAttached() == false)
                        {
                            AutoAttach = false;
                        }
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string path = @"\scripts\";
            Populate(path);
        }
    }
}
