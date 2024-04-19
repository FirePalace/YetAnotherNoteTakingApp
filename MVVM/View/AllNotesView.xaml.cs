using NoteTakingApp.MVVM.Model;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace NoteTakingApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for AllNotesView.xaml
    /// </summary>
    public partial class AllNotesView : UserControl
    {
        public readonly string noteVaultPath = "D:\\C# Projects\\NoteTakingApp\\NoteVault\\";
        public List<FileInfo> allFileNames = new List<FileInfo>();
        public AllNotesView()
        {
            InitializeComponent();
            LoadAllNotes();


        }
        private void LoadAllNotes()
        {
            LoadAllNotes(0);
        }
        private void LoadAllNotes(int SelectedSortingObjectNumber)
        {
            allFileNames.Clear();
            string[] allFilePaths = Directory.GetFiles(noteVaultPath);
            foreach (string file in allFilePaths)
            {
                FileInfo fileInfo = new FileInfo(file);
                allFileNames.Add(fileInfo);
            }


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
            leftStackPanel.Children.Clear();
            foreach (FileInfo file in allFileNames)
            {

                Button button = new Button
                {
                    Content = file.Name,
                    Name = file.Name.Split('.')[0],
                    Height = 30,
                    Width = 100,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Left
                };
                button.Click += Button_Click;
                leftStackPanel.Children.Add(button);

            }
            //Console.ReadLine();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainNoteWindow mainNoteWindow = new MainNoteWindow();
            mainNoteWindow.Show();
            mainNoteWindow.OpenFileFromAllNotesView(sender);
        }

        private void NoteSortCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var test = e.AddedItems;
            if (test.Count == 1)
            {

                foreach (var item in test)
                {
                    if (item.ToString().Contains("Name asc."))
                    {
                        LoadAllNotes(0);
                    }
                    else if (item.ToString().Contains("Name desc."))
                    {
                        LoadAllNotes(1);
                    }
                    else if (item.ToString().Contains("Date asc."))
                    {
                        LoadAllNotes(2);
                    }
                    else if (item.ToString().Contains("Date desc."))
                    {
                        LoadAllNotes(3);
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

        }
    }
}
