using EGrader.Classes;
using EGrader.Controllers;
using EGrader.Models;
using System.Windows.Controls;
using System;
using System.Collections.Generic;

namespace EGrader.Views {
    /// <summary>
    /// Interaction logic for ProfileView.xaml
    /// </summary>
    public partial class ProfileView : UserControl, View {


        public ProfileView() {
            InitializeComponent();

            labelName.Content = CurrentUser.Name;
            labelLastname.Content = CurrentUser.Lastname;
            labelUsername.Content = CurrentUser.Username;
        }

        public void Update(ref List<object> objectsList) {
            throw new NotImplementedException();
        }


    }//class
}
