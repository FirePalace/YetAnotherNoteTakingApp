using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using NoteTakingApp.MVVM.Model;

namespace NoteTakingApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for NewNoteView.xaml
    /// </summary>
    public partial class NewNoteView : UserControl
    {
        public NewNoteView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainNoteWindow mainNoteWindow = new MainNoteWindow();
            mainNoteWindow.Show();
        }
    }
}
