using EGrader.Classes;
using EGrader.Controllers;
using EGrader.Models;
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
            labelBirthDate.Content = CurrentUser.BirthDate;
            labelGender.Content = CurrentUser.Gender;
        }



    }//class
}
