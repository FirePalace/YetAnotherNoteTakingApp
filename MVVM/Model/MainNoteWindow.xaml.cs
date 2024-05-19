using Microsoft.Win32;
using NoteTakingApp.MVVM.View;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace NoteTakingApp.MVVM.Model
{
    /// <summary>
    /// Interaction logic for MainNoteWindow.xaml
    /// </summary>
    public partial class MainNoteWindow : Window
    {
        public readonly string defaultDirectory = "";
        public string[] readText = new string[10000];
        private string currentNote = "";
        public MainNoteWindow()
        {
            InitializeComponent();
            Closing += OnWindowClosing;
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
            fileDialog.DefaultExt = "rtf";
            fileDialog.Multiselect = false;
            bool? statusOK = fileDialog.ShowDialog();

            if (statusOK == true && fileDialog.FileName != "")
            {
                string fileDialogPath = fileDialog.FileName;
                fileNameBlock.Text = "";
                fileSpaceBox.Document.Blocks.Clear();
                FileStream streamToRtfFile = new FileStream(fileDialogPath, FileMode.Open);
                fileSpaceBox.Selection.Load(streamToRtfFile, DataFormats.Rtf);
            }
        }

        private void SaveFileBtn_Click(object sender, RoutedEventArgs e)
        {

            if (fileNameBlock.Text == "")
            {
                MessageBox.Show("Error: file name is empty");
            }
            else if (!fileNameBlock.Text.Contains(".rtf"))
            {
                SaveRichTextBox(".rtf");
                MessageBox.Show("File Saved");

            }
            else
            {
                SaveRichTextBox("");
                MessageBox.Show("File Saved");
            }
        }

        public void SaveRichTextBox(string rtf)
        {

            TextRange t = new TextRange(fileSpaceBox.Document.ContentStart, fileSpaceBox.Document.ContentEnd);
            using (FileStream file = new FileStream(defaultDirectory + fileNameBlock.Text + rtf, FileMode.Create))
            {
                t.Save(file, System.Windows.DataFormats.Rtf);
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
        public string DeleteFirstOccurance(string originalString, string stringToRemove)
        {
            int index = originalString.IndexOf(stringToRemove);
            if (index != -1)
            {
                originalString = originalString.Remove(index, stringToRemove.Length);
            }
            return originalString;
        }
        public void OpenFilesFromMainView(string FullFileName)
        {

            FileStream streamToRtfFile = new FileStream(FullFileName, FileMode.Open);
            fileSpaceBox.Selection.Load(streamToRtfFile, DataFormats.Rtf);
            string fileName = DeleteBeforeLastOccurrence(FullFileName, '\\');
            fileNameBlock.Text = fileName.Split(".")[0];
            streamToRtfFile.Close();
        }
        public void OpenFilesFromMainView(object sender)
        {

            string button = sender.ToString().Split(":")[1];
            button = DeleteFirstOccurance(button, " ");
            currentNote = defaultDirectory + button;
            FileStream streamToRtfFile = new FileStream(defaultDirectory + button, FileMode.Open);
            fileSpaceBox.Selection.Load(streamToRtfFile, DataFormats.Rtf);
            fileNameBlock.Text = button.Split(".")[0];
            streamToRtfFile.Close();
        }
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            AllNotesView.openNotes.Remove(currentNote);

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

