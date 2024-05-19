using NoteTakingApp.MVVM.Model;
using System.IO;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;

namespace NoteTakingApp.MVVM.View
{

    /// <summary>
    /// Interaction logic for AllNotesView.xaml
    /// </summary>
    public partial class AllNotesView : UserControl
    {
        public static string searchText = "";
        public static string lastSearchText = "";
        public static AllNotesView allNotesView;
        public static string noteVaultPath = "";
        static bool lastSearchTexthasBeenCalledOnce = false;
        public static List<string> openNotes = new List<string>();

        public List<FileInfo> allFileNames = new List<FileInfo>();
        bool isUserInteraction;


        public AllNotesView()
        { 

            InitializeComponent();
            noteVaultPath = SetNoteDirectory();
            allNotesView = this;
            if (searchText != "")
            {
                lastSearchText = searchText;
                SearchFromMainWindow(searchText);
                
            }
            else
            {
                SortAllNotes(0);

            }

        }
        private string SetNoteDirectory()
        {
            string noteDirectory = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory().ToString()).ToString()).ToString()).ToString();
            return noteDirectory + "\\NoteVault\\";
        }
        public void LoadAllNotes()
        {
            allFileNames.Clear();
            string[] allFilePaths = Directory.GetFiles(noteVaultPath);
            foreach (string file in allFilePaths)
            {
                FileInfo fileInfo = new FileInfo(file);
                allFileNames.Add(fileInfo);

            }
        }
        private void SortAllNotes(int SelectedSortingObjectNumber)
        {
            LoadAllNotes();
            switch (SelectedSortingObjectNumber)
            {
                case 0:
                    allFileNames.OrderBy(o => o.Name);

                    break;
                case 1:
                    allFileNames.OrderBy(o => o.Name);
                    allFileNames.Reverse();
                    break;
                case 2:
                    allFileNames.OrderBy(o => o.LastAccessTime);

                    break;
                case 3:
                    allFileNames.OrderBy(o => o.LastAccessTime);
                    allFileNames.Reverse();
                    break;


            }
            DisplayAllNotes();
        }
        private void DisplayAllNotes()
        {
            DisplayAllNotes(allFileNames);
        }
        private void DisplayAllNotes(List<FileInfo> allFileInfo)
        {
            int count = 0;
            ClearAllColumns();


            foreach (FileInfo file in allFileInfo)
            {

                Button button = new Button
                {
                    Content = file.Name,
                    Name = file.Name.Split('.')[0].Replace(" ","_"),
                    Height = 30,
                    Width = 100,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                    Tag = file.FullName,
                    Style = this.FindResource("CyanButton") as Style

                };
                button.Click += Button_Click;
                if (count <= 11)
                {
                    leftStackPanel.Children.Add(button);
                }
                else if (count > 11 && count <= 22)
                {
                    leftMiddleStackPanel.Children.Add(button);
                }
                else if (count > 23 && count <= 33)
                {
                    rightMiddleStackPanel.Children.Add(button);
                }
                else
                {
                    rightStackPanel.Children.Add(button);
                }
                count++;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (!openNotes.Contains(button.Tag.ToString()))
            {
                
                openNotes.Add(button.Tag.ToString());

                MainNoteWindow mainNoteWindow = new MainNoteWindow();
                mainNoteWindow.Show();
                mainNoteWindow.OpenFilesFromMainView(sender);

                if (searchText != "")
                {
                    mainNoteWindow.KeyWordSearch.Text = searchText;
                }
            }
            else
            {
                MessageBox.Show("Note is allready open");
            }

        }
        private void NoteSortCombobox_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            isUserInteraction = true;
        }
        private void NoteSortCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isUserInteraction)
            {
                ComboBox comboBox = (ComboBox)sender;
                if (!comboBox.IsLoaded) return;

                var test = e.AddedItems;
                if (test.Count == 1)
                {

                    foreach (var item in test)
                    {
                        if (item.ToString().Contains("Name asc."))
                        {
                            SortAllNotes(0);
                        }
                        else if (item.ToString().Contains("Name desc."))
                        {
                            SortAllNotes(1);
                        }
                        else if (item.ToString().Contains("Date asc."))
                        {
                            SortAllNotes(2);
                        }
                        else if (item.ToString().Contains("Date desc."))
                        {
                            SortAllNotes(3);
                        }
                        else
                        {
                            throw new ArgumentException("Something went wrong");
                        }
                    }
                }
                else
                {
                    throw new ArgumentException("Incorrect amount of objects selected.");
                }
                isUserInteraction = false;
            }

        }

        private void AllNotesSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = AllNotesSearchBox.Text;
            LoadAllNotes();
            allFileNames.RemoveAll(o => !o.Name.Split(".")[0].Contains(text));
            DisplayAllNotes();
        }
        public void SearchFromMainWindow(string searchTerm)
        {
            List<FileInfo> modifiedFileInfoList = new List<FileInfo>();
            try
            {
                LoadAllNotes();
                foreach (var item in allFileNames)
                {
                    string fileContents = File.ReadAllText(item.FullName);
                    if (fileContents.Contains(searchTerm))
                    {
                        modifiedFileInfoList.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Loading files failed because: " + ex);
            }
            DisplayAllNotes(modifiedFileInfoList);
        }
        private void ClearAllColumns()
        {
            leftStackPanel.Children.Clear();
            leftMiddleStackPanel.Children.Clear();
            rightMiddleStackPanel.Children.Clear();
            rightStackPanel.Children.Clear();
        }
    }
}
