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
        private void OpenFileBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
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

        private void SaveFileBtn_Click(object sender, RoutedEventArgs e)
        {
            if (fileNameBlock.Text == "")
            {
                MessageBox.Show("Error: file name is empty");
            }
            else if (!fileNameBlock.Text.Contains(".txt"))
            {
                File.WriteAllText(defaultDirectory + fileNameBlock.Text + ".txt", fileSpaceBox.Text);
            }
            else
            {
                File.WriteAllText(defaultDirectory + fileNameBlock.Text, fileSpaceBox.Text);
            }
        }

        private void FileSpaceBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                int caretIndex = fileSpaceBox.CaretIndex;
                fileSpaceBox.Text = fileSpaceBox.Text.Insert(caretIndex, "\n");
                fileSpaceBox.CaretIndex = caretIndex + 1;
            }
        }
        public string DeleteBeforeLastOccurrence(string input, char target)
        {

            int lastIndex = input.LastIndexOf(target);
            if (lastIndex >= 0)
            {
                return input.Substring(lastIndex + 1);
            }

            return input;
        }
        public void OpenFilesFromMainView(string FullFileName)
        {
            readText = File.ReadAllLines(FullFileName);
            for (int i = 0; i < readText.Length; i++)
            {
                fileSpaceBox.Text += readText[i] + '\n';
            }
            string fileName = DeleteBeforeLastOccurrence(FullFileName, '\\');
            fileNameBlock.Text = fileName.Split(".")[0];
        }
        public void OpenFilesFromMainView(object sender)
        {
            string button = sender.ToString().Split(":")[1].Replace(" ", "");
            readText = File.ReadAllLines(defaultDirectory + button);
            for (int i = 0; i < readText.Length; i++)
            {
                fileSpaceBox.Text += readText[i] + '\n';
            }
            fileNameBlock.Text = button.Split(".")[0];
        }
    }
}
