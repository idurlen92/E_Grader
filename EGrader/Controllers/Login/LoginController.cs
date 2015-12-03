using EGrader.Classes;
using EGrader.Models;
using EGrader.Models.Objects;
using EGrader.Windows;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EGrader.Controllers.Login {
    class LoginController : Controller {

        int invalidCredentialsCounter = 0;

        StringBuilder usernameStringBuilder;
        StringBuilder passwordStringBuilder;

        Model model;
        LoginWindow view;
        

        public LoginController(Model model, Window view) {
            this.model = model;
            this.view = (LoginWindow) view;

            usernameStringBuilder = new StringBuilder();
            passwordStringBuilder = new StringBuilder();

            this.view.buttonLogin.IsEnabled = false;

            this.view.buttonLogin.Click += ActionLogin;
            this.view.textBoxPassword.KeyUp += ActionKeyPress;
            this.view.textBoxUsername.KeyUp += ActionKeyPress;
        }


        /// <summary>
        /// Handlanje pritiska gumba u poljima za unos teksta - punjenje strigbuildera, togglanje labele za info i disablanje buttona za login.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ActionKeyPress(object sender, RoutedEventArgs e) {
            if(sender is TextBox) {
                usernameStringBuilder.Clear();
                usernameStringBuilder.Append(((TextBox) sender).Text);
            }
            else {
                passwordStringBuilder.Clear();
                passwordStringBuilder.Append(((PasswordBox) sender).Password);
            }

            view.labelInfo.Visibility = Visibility.Hidden;
            view.buttonLogin.IsEnabled = (usernameStringBuilder.Length > 2 && passwordStringBuilder.Length > 2);
        }



        /// <summary>
        /// Handlanje gumba za login - provjeravanje postojanja korisnika, login...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ActionLogin(object sender, RoutedEventArgs e) {
            List<object> usersList = new List<object>();
            try {
                foreach (UserObject user in model.GetObjectsByCriteria("u.username = ", usernameStringBuilder.ToString(), "AND u.password = ",
                passwordStringBuilder.ToString()))
                    usersList.Add(user);
            }
            catch (Exception exc){
                Console.WriteLine(exc.Message + ":\n" + exc.StackTrace);
                return;
            }

            if (usersList.Count == 1) {
                CurrentUser.LogUserIn((UserObject) usersList[0]);
                AppController.CreateMainWindow();
            }
            else {
                view.labelForgottenPassword.Visibility = (++invalidCredentialsCounter == 3) ? Visibility.Visible : Visibility.Hidden;
                view.labelInfo.Visibility = Visibility.Visible;
            }
        }

        
    }//class
}
