using EGrader.Classes;
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
using System.Windows.Shapes;

namespace EGrader.Windows {
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window {

        int invalidCredentialsCounter = 0;
        UsersModel usersModel = new UsersModel(DatabaseManager.GetInstance());

        // ---------- TEMPORARY ---------- 
        String[,] usersPasswords = new String[,] { { "idurlen", "fnh57dsx" }, { "admin", "AdmiN12345" } };
        // -------------------------------


        public LoginWindow() {
            InitializeComponent();
            buttonLogin.IsEnabled = false;
        }


        private void HandleKeyPress(object sender, RoutedEventArgs e) {
            if (IsExistentUser(textBoxUsername.Text))
                buttonLogin.IsEnabled = true;
        }


        private void HandleLogin(object sender, RoutedEventArgs e) {
            if (!isValidCredentials(textBoxUsername.Text, textBoxPassword.Password)) {
                invalidCredentialsCounter++;
                if (invalidCredentialsCounter == 3)
                    labelForgottenPassword.Visibility = Visibility.Visible;
            }
            else {
                MessageBox.Show("Valid login for: " + textBoxUsername.Text);
                MainWindow mainWindow = new MainWindow();
                App.Current.MainWindow = mainWindow;
                this.Close();
                mainWindow.Show();
            }
        }


        private Boolean IsExistentUser(String username) {
            return usersModel.userExists(username);
        }


        private Boolean isValidCredentials(String username, String password) {
            return usersModel.userExists(username, password);
        }
    }
}
