using NoteTakingApp.MVVM.View;
using NoteTakingApp.MVVM.ViewModel;
using System.Windows;
using System.Windows.Input;

namespace NoteTakingApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SearchAllNotes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AllNotesView.searchText = SearchAllNotes.Text;

                MainViewModel viewModel = (MainViewModel)DataContext;

                if (!viewModel.CurrentView.ToString().Contains("AllNotesViewModel"))
                {
                    if (viewModel.AllNotesViewCommand.CanExecute(null))
                    {
                        viewModel.AllNotesViewCommand.Execute(null);
                        allNotesButton.IsChecked = true;

                    }
                }
                else
                {
                    AllNotesView.allNotesView.SearchFromMainWindow(SearchAllNotes.Text);
                }



            }

        }
    }
}