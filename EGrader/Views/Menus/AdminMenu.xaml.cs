using EGrader.Controllers.Factory;
using EGrader.Controllers.Menu;
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
    /// Interaction logic for AdminMenu.xaml
    /// </summary>
    public partial class AdminMenu :  UserControl, MenuView{

        MenuController controller;

        public AdminMenu() {
            InitializeComponent();
            controller = (MenuController) ControllerFactory.NewMenuControllerInstance();
            foreach (object element in contentHolder.Children)
                ((Button) element).Click += controller.DoAction;
        }

        public void Toggle(object sender, RoutedEventArgs e) {
            Visibility = IsVisible ? Visibility.Collapsed : Visibility.Visible;    
        }


    }//class
}
