using EGrader.Classes;
using EGrader.Controllers;
using EGrader.Models;
using System.Windows.Controls;

namespace EGrader.Views {
    /// <summary>
    /// Interaction logic for ProfileView.xaml
    /// </summary>
    public partial class ProfileView : UserControl {

        Controller controller;
        Model model;

        public ProfileView(Controller controller, Model model) {
            this.controller = controller;
            this.model = model;
            InitializeComponent();

            labelName.Content = CurrentUser.Name;
            labelLastname.Content = CurrentUser.Lastname;
            labelUsername.Content = CurrentUser.Username;
            //TODO:
        }



    }//class
}
