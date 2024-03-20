using System.Diagnostics;
using System.IO;
using System.Windows.Controls;

namespace NoteTakingApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for AllNotesView.xaml
    /// </summary>
    public partial class AllNotesView : UserControl
    {
        public readonly string noteVaultPath = "D:\\C# Projects\\NoteTakingApp\\NoteVault\\";
        public AllNotesView()
        {
            InitializeComponent();


            string[] allFilePaths = Directory.GetFiles(noteVaultPath);
            List<String> allFileNames = new List<String>();


            foreach (string input in allFilePaths)
            {
                if (File.Exists(input))
                {
                    string result = DeleteBeforeLastOccurrence(input, '\\');
                    allFileNames.Add(result);
                }
            }

            foreach (string name in allFileNames)
            {

                Button button = new Button
                {
                    Content = name,
                    Name = name.Split('.')[0],
                    Height = 30,
                    Width = 100,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Left
                };
                button.Click += Button_Click;
                leftStackPanel.Children.Add(button);

            }
            Console.ReadLine();

        }
        static string DeleteBeforeLastOccurrence(string input, char target)
        {

            int lastIndex = input.LastIndexOf(target);
            if (lastIndex >= 0)
            {
                return input.Substring(lastIndex + 1);
            }

            return input;
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            string button = sender.ToString().Split(":")[1].Replace(" ", "");
            string fileEnding = button.Split(".")[1];
            if (fileEnding == "txt")
            {
                Process.Start("notepad.exe", noteVaultPath + button);
            }


        }
    }
}
