using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace NoteTakingApp.MVVM.Model
{
    /// <summary>
    /// Interaction logic for MainNoteWindow.xaml
    /// </summary>
    public partial class MainNoteWindow : Window
    {
        
        public readonly string defaultDirectory = @"D:\C# Projects\NoteTakingApp\NoteVault\";
        public string[] readText = new string[10000];
        public MainNoteWindow()
        {
            InitializeComponent();
        }
        private void openFileBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog()
            {
                
            };
            fileDialog.InitialDirectory = defaultDirectory;
            fileDialog.DefaultExt = "txt";
            fileDialog.Multiselect = false;
            bool? statusOK = fileDialog.ShowDialog();
            
            if (statusOK == true && fileDialog.FileName != "")
            {
                string fileDialogPath = fileDialog.FileName;
                fileNameBlock.Text = "";
                fileSpaceBox.Text = "";
                fileNameBlock.Text = fileDialog.SafeFileName;
                readText = File.ReadAllLines(fileDialogPath);
                for (int i = 0; i < readText.Length; i++)
                {
                    fileSpaceBox.Text += readText[i] + '\n';
                }
            }
        }

        private void saveFileBtn_Click(object sender, RoutedEventArgs e)
        {
            if (fileNameBlock.Text == "")
            {
                MessageBox.Show("Error: file name is empty");
            }
            else
            {
                File.WriteAllText(defaultDirectory + fileNameBlock.Text + ".txt", fileSpaceBox.Text);
            }
        }

        private void fileSpaceBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                int caretIndex = fileSpaceBox.CaretIndex;
                fileSpaceBox.Text = fileSpaceBox.Text.Insert(caretIndex, "\n");
                fileSpaceBox.CaretIndex = caretIndex + 1;
            }
        }
    }
}
