using PubgGuestReset.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PubgGuestReset
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tbConsole.Text = "";
            Execute execute = new Execute();
            AdbCommand adbCommand = new AdbCommand();

            TextWriter writer = new TextBoxConsole(tbConsole);


          
            List<string> list = new List<string>(
                            adbCommand.c1.Split(new string[] { "\r\n" },
                           StringSplitOptions.RemoveEmptyEntries));

            string path = @folderDir;
            string disk = path.Split(':').First();
            list[0] = list[0].Replace("%disk%", disk + ":");
            list[1] = list[1].Replace("%path%", path);
            Console.SetOut(writer);
            execute.RunCommands(list);
            checkError();

        }
        void checkError()
        {
            string log = tbConsole.Text;
            if (log.Contains("device not found"))
            {
                MessageBox.Show("Gameloop Aktif Değil Lütfen Emülatörü Çalıştırın","HATA",MessageBoxButtons.OK);
            } 
            if (log.Contains("'adb' is not recognized"))
            {
                MessageBox.Show("Gameloop UI klasörü hatalı veya seçilmemiş","HATA",MessageBoxButtons.OK);
            }
        }
        bool logOpen = false;
        string folderDir = "";
        private void button4_Click(object sender, EventArgs e)
        {
            

            if (logOpen)
            {
                this.Size = new Size(372, 513);
                logOpen = false;


            }
            else
            {

                this.Size = new Size(647, 513);
                logOpen = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
           FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.Description = "Lütfen Gameloop UI klasörünü Seçiniz";
            folder.ShowDialog();
            folderDir = folder.SelectedPath;
            folderDirTxt.Text = folderDir;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            uuidTxt.Text = RandomString(32);
        }
        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }

    public class TextBoxConsole : TextWriter
    {
        TextBox output = null; //Textbox used to show Console's output.

        /// <summary>
        /// Custom TextBox-Class used to print the Console output.
        /// </summary>
        /// <param name="_output">Textbox used to show Console's output.</param>
        public TextBoxConsole(TextBox _output)
        {
            output = _output;
            output.ScrollBars = ScrollBars.Both;
            output.WordWrap = true;
        }

        //<summary>
        //Appends text to the textbox and to the logfile
        //</summary>
        //<param name="value">Input-string which is appended to the textbox.</param>
        public override void Write(char value)
        {
            base.Write(value);
            output.AppendText(value.ToString());//Append char to the textbox
        }


        public override Encoding Encoding => System.Text.Encoding.UTF8;
    }
}
