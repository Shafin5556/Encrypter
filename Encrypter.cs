using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.IO;
using System.Security.Cryptography;
using System.Security.Principal;
namespace Encrypter
{
    public partial class Encrypter : Form
    {
        WindowsIdentity identity = WindowsIdentity.GetCurrent();
        static string saved_security_key="";
        static string folder_path= "C:\\Program Files\\Encrypter";
        static string key_path = "C:\\Program Files\\Encrypter\\key.txt";
        static System.Windows.Forms.OpenFileDialog dlg;
        bool Done = false;
        static byte[] data;
        static string read_key;
        static string SP;

        public Encrypter()
        {
            InitializeComponent();
        }

        private void Encrypter_Load(object sender, EventArgs e)
        {
            string adminUserName = identity.Name.Split('\\')[1];
            User_name.Text= adminUserName;        
            if (!Directory.Exists(folder_path))
            {
               
                Directory.CreateDirectory(folder_path);
                if (!File.Exists(key_path))
                {
                    string validchars = "a5b3cd0efg8hij5k9lm6n8op3qr7s2tu6vw5xy4z";
                    var sb = new StringBuilder();
                    var rand = new Random();
                    for (int i = 1; i <= 34; i++)
                    {
                        int idx = rand.Next(0, validchars.Length);
                        char randomChar = validchars[idx];
                        sb.Append(randomChar);
                    }
                    SP = sb.ToString();
                    File.WriteAllText(Path.Combine(key_path),SP);
                    Security_Key_Text.Text = SP;
                }
                else
                {
                    read_key= File.ReadAllText(key_path);
                    Security_Key_Text.Text = read_key;
                }
                
            }
            else
            {
                if (!File.Exists(key_path))
                {
                    string validchars = "a5b3cd0efg8hij5k9lm6n8op3qr7s2tu6vw5xy4z";
                    var sb = new StringBuilder();
                    var rand = new Random();
                    for (int i = 1; i <= 34; i++)
                    {
                        int idx = rand.Next(0, validchars.Length);
                        char randomChar = validchars[idx];
                        sb.Append(randomChar);
                    }
                    SP = sb.ToString();
                    File.WriteAllText(Path.Combine(key_path), SP);
                    Security_Key_Text.Text = SP;
                }
                else
                {
                    read_key = File.ReadAllText(key_path);
                    Security_Key_Text.Text = read_key;
                }
            }
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystemProduct");
            ManagementObjectCollection collection = searcher.Get();
            foreach (ManagementObject obj in collection)
            {
                string hwid = (string)obj["UUID"];
                Hwid_Text.Text = hwid;
            }
            read_key = File.ReadAllText(key_path);
            Security_Key_Text.Text = read_key;
        }

        private void Setting_Button_Click(object sender, EventArgs e)
        {

        }

        private void Exit_Button_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Minimize_Button_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void main_Box_Click(object sender, EventArgs e)
        {

        }
        private void Hwid_Text_Click(object sender, EventArgs e)
        {

        }
        private void Security_Key_Text_Click(object sender, EventArgs e)
        {

        }
        private void Generate_Click(object sender, EventArgs e)
        {
            string validchars = "a5b3cd0efg8hij5k9lm6n8op3qr7s2tu6vw5xy4z";
            var sb = new StringBuilder();
            var rand = new Random();
            for (int i = 1; i <= 34; i++)
            {
                int idx = rand.Next(0, validchars.Length);
                char randomChar = validchars[idx];
                sb.Append(randomChar);
            }
            SP = sb.ToString();
            Custom_Key_Text.Text = SP;
        }
        private void Custom_Key_Text_TextChanged(object sender, EventArgs e)
        {

        }
        private void Save_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to save Security key ,Make sure you backup the previous keys?", "Confirmation", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {

                Security_Key_Text.Text = Custom_Key_Text.Text;
                File.WriteAllText(Path.Combine(key_path), Custom_Key_Text.Text);
                MessageBox.Show("Done");
            }
            else
            {
                Custom_Key_Text.Clear();
            }
        }
        private void label7_Click(object sender, EventArgs e)
        {

        }
        private void Encrypt_Browse_Click(object sender, EventArgs e)
        {
            dlg = new System.Windows.Forms.OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Encrypt_Target_path.Text=dlg.FileName;       
            }
        }
        private void Encrypt_File_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] first_to_last;
                byte[] data = File.ReadAllBytes(dlg.FileName);
                byte[] shafin_encryption_first_to_last = data;
                string key = Security_Key_Text.Text;
                MD5CryptoServiceProvider shafin_algorithm = new MD5CryptoServiceProvider();
                first_to_last = shafin_algorithm.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                shafin_algorithm.Clear();
                TripleDESCryptoServiceProvider shafin_encrypter = new TripleDESCryptoServiceProvider();
                shafin_encrypter.Key = first_to_last;
                shafin_encrypter.Mode = CipherMode.ECB;
                shafin_encrypter.Padding = PaddingMode.PKCS7;
                ICryptoTransform shafin_transform = shafin_encrypter.CreateEncryptor();
                byte[] resultArray = shafin_transform.TransformFinalBlock(shafin_encryption_first_to_last, 0, shafin_encryption_first_to_last.Length);
                shafin_encrypter.Clear();
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PNG files (*.png)|*.png|Text files (*.txt)|*.txt|JPEG files (*.jpg)|*.jpg|PDF files (*.pdf)|*.pdf|Word documents (*.doc)|*.doc|WAV Files (*.wav)|*.wav|All Files (*.*)|*.*";
                saveFileDialog.RestoreDirectory = true;
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string fileName = saveFileDialog.FileName;
                    File.WriteAllBytes(fileName, resultArray);
                }
                Done = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Encryption Failed " + ex.Message);
            }
            finally
            {
                if (Done == true)
                {
                    MessageBox.Show("Encryption Done");
                }
            }
            Done = false;
        }
        private void Encrypt_Target_path_TextChanged(object sender, EventArgs e)
        {

        }
        private void Decrypt_Browse_Click(object sender, EventArgs e)
        {
            dlg = new System.Windows.Forms.OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Decrypt_Target_Path.Text = dlg.FileName;
            }
        }
        private void Decrypt_Target_Path_TextChanged(object sender, EventArgs e)
        {

        }
        private void Decrypt_File_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] first_to_last;
                byte[] data = File.ReadAllBytes(dlg.FileName);
                byte[] shafin_encryption_first_to_last = (data);
                string key = Security_Key_Text.Text;
                MD5CryptoServiceProvider shafin_algorithm = new MD5CryptoServiceProvider();
                first_to_last = shafin_algorithm.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                shafin_algorithm.Clear();
                TripleDESCryptoServiceProvider shafin_encrypter = new TripleDESCryptoServiceProvider(); shafin_encrypter.Key = first_to_last;
                shafin_encrypter.Mode = CipherMode.ECB;
                shafin_encrypter.Padding = PaddingMode.PKCS7;
                ICryptoTransform shafin_transform = shafin_encrypter.CreateDecryptor();
                byte[] resultArray = shafin_transform.TransformFinalBlock(shafin_encryption_first_to_last, 0, shafin_encryption_first_to_last.Length);
                shafin_encrypter.Clear();
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PNG files (*.png)|*.png|Text files (*.txt)|*.txt|JPEG files (*.jpg)|*.jpg|PDF files (*.pdf)|*.pdf|Word documents (*.doc)|*.doc|WAV Files (*.wav)|*.wav|All Files (*.*)|*.*";
                saveFileDialog.RestoreDirectory = true;
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string fileName = saveFileDialog.FileName;
                    File.WriteAllBytes(fileName, resultArray);
                }
                Done=true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Decryption Failed " + ex.Message);
            }
            finally
            {
                if (Done == true)
                {
                    MessageBox.Show("Decryption Done");
                }
            }
            Done = false;
        }

        private void Encrypt_Text_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] first_to_last;
                byte[] shafin_encryption_first_to_last = UTF8Encoding.UTF8.GetBytes(Encrypt_Text_Format.Text);
                string key = Security_Key_Text.Text;
                MD5CryptoServiceProvider shafin_algorithm = new MD5CryptoServiceProvider();
                first_to_last = shafin_algorithm.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                shafin_algorithm.Clear();
                TripleDESCryptoServiceProvider shafin_encrypter = new TripleDESCryptoServiceProvider();
                shafin_encrypter.Key = first_to_last;
                shafin_encrypter.Mode = CipherMode.ECB;
                shafin_encrypter.Padding = PaddingMode.PKCS7;
                ICryptoTransform shafin_transform = shafin_encrypter.CreateEncryptor();
                byte[] resultArray = shafin_transform.TransformFinalBlock(shafin_encryption_first_to_last, 0, shafin_encryption_first_to_last.Length); 
                shafin_encrypter.Clear();
                Encrypt_Text_Format.Text = Convert.ToBase64String(resultArray, 0, resultArray.Length);
                Done = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Encryption Failed " + ex.Message);
            }
            finally
            {
                if (Done == true)
                {
                    MessageBox.Show("Encryption Done");
                }
            }
            Done = false;
        }

        private void Encrypt_Text_Format_TextChanged(object sender, EventArgs e)
        {

        }
  
        private void Decrypt_Text_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] first_to_last;
                byte[] shafin_encryption_first_to_last = Convert.FromBase64String(Decrypt_Text_Format.Text);
                string key = Security_Key_Text.Text;
                MD5CryptoServiceProvider shafin_algorithm = new MD5CryptoServiceProvider();
                first_to_last = shafin_algorithm.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                shafin_algorithm.Clear();
                TripleDESCryptoServiceProvider shafin_encrypter = new TripleDESCryptoServiceProvider();
                shafin_encrypter.Key = first_to_last;
                shafin_encrypter.Mode = CipherMode.ECB;
                shafin_encrypter.Padding = PaddingMode.PKCS7;
                ICryptoTransform shafin_transform = shafin_encrypter.CreateDecryptor();
                byte[] resultArray = shafin_transform.TransformFinalBlock(shafin_encryption_first_to_last, 0, shafin_encryption_first_to_last.Length);
                shafin_encrypter.Clear();
                Decrypt_Text_Format.Text = UTF8Encoding.UTF8.GetString(resultArray);
                Done= true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Decryption Failed " + ex.Message);
            }
            finally
            {
                if (Done == true)
                {
                    MessageBox.Show("Decryption Done");
                }
            }
            Done = false;
        }
        private void Decrypt_Text_Format_TextChanged(object sender, EventArgs e)
        {

        }

        private void Decrypt_Section_Click(object sender, EventArgs e)
        {

        }
    }
}
