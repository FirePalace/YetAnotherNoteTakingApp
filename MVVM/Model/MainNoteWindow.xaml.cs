using Microsoft.Win32;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

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
            defaultDirectory = SetNoteDirectory();
        }
        private string SetNoteDirectory()
        {
            string noteDirectory = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory().ToString()).ToString()).ToString()).ToString();
            return noteDirectory + @"\NoteVault\";
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
                fileSpaceBox.Document.Blocks.Clear();
                fileNameBlock.Text = fileDialog.SafeFileName;
                readText = File.ReadAllLines(fileDialogPath);
                for (int i = 0; i < readText.Length; i++)
                {
                    fileSpaceBox.Document.Blocks.Add(new Paragraph(new Run(readText[i])));
                }
            }
        }

        private void SaveFileBtn_Click(object sender, RoutedEventArgs e)
        {
            string richText = new TextRange(fileSpaceBox.Document.ContentStart, fileSpaceBox.Document.ContentEnd).Text;
            if (fileNameBlock.Text == "")
            {
                MessageBox.Show("Error: file name is empty");
            }
            else if (!fileNameBlock.Text.Contains(".txt"))
            {
                File.WriteAllText(defaultDirectory + fileNameBlock.Text + ".txt", richText);
            }
            else
            {
                File.WriteAllText(defaultDirectory + fileNameBlock.Text, richText);
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
                fileSpaceBox.Document.Blocks.Add(new Paragraph(new Run(readText[i])));
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
                fileSpaceBox.Document.Blocks.Add(new Paragraph(new Run(readText[i])));
            }
            fileNameBlock.Text = button.Split(".")[0];
        }

        private void KeyWordSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            TextRange textRange = new TextRange(fileSpaceBox.Document.ContentStart, fileSpaceBox.Document.ContentEnd);

            textRange.ClearAllProperties();

            string textBoxText = textRange.Text;
            string searchText = KeyWordSearch.Text;

            if (string.IsNullOrWhiteSpace(textBoxText) || string.IsNullOrWhiteSpace(searchText))
            {
                //do nothing
            }
            else
            {

                Regex regex = new Regex(searchText);
                int count_MatchFound = Regex.Matches(textBoxText, regex.ToString()).Count;

                for (TextPointer startPointer = fileSpaceBox.Document.ContentStart; startPointer.CompareTo(fileSpaceBox.Document.ContentEnd) <= 0; startPointer = startPointer.GetNextContextPosition(LogicalDirection.Forward))
                {

                    if (startPointer.CompareTo(fileSpaceBox.Document.ContentEnd) == 0)
                    {
                        break;
                    }
                    string parsedString = startPointer.GetTextInRun(LogicalDirection.Forward);

                    int indexOfParseString = parsedString.IndexOf(searchText);

                    if (indexOfParseString >= 0)
                    {

                        startPointer = startPointer.GetPositionAtOffset(indexOfParseString);

                        if (startPointer != null)
                        {
                            TextPointer nextPointer = startPointer.GetPositionAtOffset(searchText.Length);
                            TextRange searchedTextRange = new TextRange(startPointer, nextPointer);
                            searchedTextRange.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(Colors.Yellow));

                        }
                    }
                }
            }
        }

    }
}

