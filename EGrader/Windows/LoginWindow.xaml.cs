using EGrader.Controllers.Login;
using EGrader.Models.Factory;
using System.Windows;

namespace EGrader.Windows {
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window {


        public LoginWindow() {
            InitializeComponent();
            LoginController controller = new LoginController(ModelFactory.NewModelInstance(ModelType.Users), this);
        }


    }//class
}
