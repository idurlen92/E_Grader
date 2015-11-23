using EGrader.Windows;
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

namespace EGrader.Views.Menus {
    /// <summary>
    /// Interaction logic for StudentMenu.xaml
    /// </summary>
    public partial class StudentMenu : UserControl {


        public StudentMenu() {
            InitializeComponent();
            
        }

  
        private void ButtonLogout_Click(object sender, RoutedEventArgs e) {
            if (MessageBox.Show("Želite li se odjaviti?", "Odjava", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
                App.Current.MainWindow.Close();
                App.Current.MainWindow = new LoginWindow();
                App.Current.MainWindow.Show();
            }
        }
    }
}
