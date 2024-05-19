using NoteTakingApp.MVVM.Model;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NoteTakingApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public List<FileInfo> allFileNames = new List<FileInfo>();
        public readonly string noteVaultPath = "";
        public HomeView()
        {
            InitializeComponent();
            noteVaultPath = SetNoteDirectory();
            LoadPreview();

        }
        private string SetNoteDirectory()
        {
            string noteDirectory = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory().ToString()).ToString()).ToString()).ToString();
            return noteDirectory + @"\NoteVault\";
        }
        private void LoadPreview()
        {
            allFileNames.Clear();
            string[] allFilePaths = Directory.GetFiles(noteVaultPath);

            foreach (string file in allFilePaths)
            {
                FileInfo fileInfo = new FileInfo(file);
                allFileNames.Add(fileInfo);
            }

            allFileNames.OrderBy(o => o.LastAccessTime);
            allFileNames.Reverse();
            ShowPreview();
        }
        private void ShowPreview()
        {
            int count = 0;
            previewStackPanel.Children.Clear();
            foreach (FileInfo file in allFileNames)
            {
                
                Button button = new Button
                {


                    Name = file.Name.Split('.')[0].Replace(" ", "_"),

                    Tag = file.FullName,
                    Height = 150,
                    Width = 150,
                    Background = Brushes.White,
                    Margin = new Thickness(10),
                    HorizontalAlignment = HorizontalAlignment.Left

                };
                string[] allLines = File.ReadAllLines(file.FullName);
                for (int i = 0; i < allLines.Length; i++)
                {
                    button.Content += allLines[i] + '\n';
                }

                button.Click += Button_Click;
                previewStackPanel.Children.Add(button);
                count++;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            MainNoteWindow mainNoteWindow = new MainNoteWindow();
            mainNoteWindow.Show();
            mainNoteWindow.OpenFilesFromMainView(button.Tag.ToString());
            Console.WriteLine("");
        }

    }

}
