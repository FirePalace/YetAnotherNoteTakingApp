using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteTakingApp.Core;

namespace NoteTakingApp.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand NewNoteViewCommand { get; set; }

        public HomeViewModel HomeVM { get; set; }
        public NewNoteViewModel NewNoteVM { get; set; }

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
            CurrentView = HomeVM;


            HomeViewCommand = new RelayCommand(o => 
            { 
                CurrentView = HomeVM;
            });
            NewNoteViewCommand = new RelayCommand(o => 
            { 
                CurrentView = NewNoteVM;
            });

            
        }
    }
}
