using System.Windows;
using System.Windows.Controls;

namespace EGrader.Views.Menus {
    /// <summary>
    /// Interaction logic for StudentMenu.xaml
    /// </summary>
    public partial class StudentMenu : UserControl, MenuView {


        public StudentMenu() {
            InitializeComponent();
        }

        

        public void Toggle(object sender, RoutedEventArgs e) {
            Visibility = IsVisible ? Visibility.Collapsed : Visibility.Visible;
        }


    }//class
}
