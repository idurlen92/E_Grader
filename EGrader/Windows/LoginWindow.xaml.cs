using EGrader.Classes;
using EGrader.Classes.Database;
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
using System.Windows.Shapes;

namespace EGrader.Windows {
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window {

        Controller controller;


        public LoginWindow() {
            InitializeComponent();

            Model model = ModelFactory.NewInstance(AppContext.Login);
            controller = ControllerFactory.NewInstance(model, AppContext.Login);

            buttonLogin.Click += controller.DoAction;
            textBoxUsername.KeyUp += controller.DoAction;
            textBoxPassword.KeyUp += controller.DoAction;
        }
        

    }
}
