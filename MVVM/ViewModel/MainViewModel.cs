using NoteTakingApp.Core;

namespace NoteTakingApp.MVVM.ViewModel
{
    public class MainViewModelHelper
    {


    }
    internal class MainViewModel : ObservableObject
    {
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand NewNoteViewCommand { get; set; }
        public RelayCommand AllNotesViewCommand { get; set; }

        public HomeViewModel HomeVM { get; set; }
        public NewNoteViewModel NewNoteVM { get; set; }
        public AllNotesViewModel AllNotesVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {

                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {

            HomeVM = new HomeViewModel();
            NewNoteVM = new NewNoteViewModel();
            AllNotesVM = new AllNotesViewModel();
            CurrentView = HomeVM;


            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeVM;
            });
            NewNoteViewCommand = new RelayCommand(o =>
            {
                CurrentView = NewNoteVM;
            });
            AllNotesViewCommand = new RelayCommand(o =>
            {
                CurrentView = AllNotesVM;
            });


        }
    }
}
