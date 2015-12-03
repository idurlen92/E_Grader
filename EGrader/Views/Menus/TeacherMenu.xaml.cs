using System.Windows;
using System.Windows.Controls;

namespace EGrader.Views.Menus {
    /// <summary>
    /// Interaction logic for TeacherMenu.xaml
    /// </summary>
    public partial class TeacherMenu : UserControl, MenuView {

        public TeacherMenu() {
            InitializeComponent();
        }

        /// <summary>
        /// Postavljaneje menija na vidljivo/nevidljivo stanje (toggle).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Toggle(object sender, RoutedEventArgs e) {
            Visibility =  IsVisible ? Visibility.Collapsed : Visibility.Visible;
        }


    }//class
}
