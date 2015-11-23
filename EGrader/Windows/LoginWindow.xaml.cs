using EGrader.Classes;
using EGrader.Classes.Database;
using EGrader.Controllers;
using EGrader.Controllers.Login;
using EGrader.Models;
using EGrader.Models.Factory;
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



        public LoginWindow() {
            InitializeComponent();

            Model model = ModelFactory.NewModelInstance(AppContext.Login);
            Controller controller = new LoginController(model, this);

            buttonLogin.IsEnabled = false;

            buttonLogin.Click += controller.DoAction;
            textBoxUsername.KeyUp += controller.DoAction;
            textBoxPassword.KeyUp += controller.DoAction;
        }
        

    }
}
