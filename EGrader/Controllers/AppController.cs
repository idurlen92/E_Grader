using EGrader.Classes;
using EGrader.Controllers.Factory;
using EGrader.Controllers.Menu;
using EGrader.Models;
using EGrader.Models.Factory;
using EGrader.Views;
using EGrader.Views.Factory;
using EGrader.Views.Menus;
using EGrader.Windows;
using System.Windows;
using System.Windows.Controls;

namespace EGrader.Controllers {

    public enum AppContext { Login, Start, Profile, Teachers, Students, Classes, StudentGrading };


    class AppController {

        static AppContext currentAppContext = AppContext.Login;
        static MainWindow mainWindow;

        public static AppContext CurrentAppContext { get { return currentAppContext; } }


        public static void CreateMainWindow() {
            Window currentWindow = App.Current.MainWindow;
            mainWindow = new MainWindow();

            MenuView menu = ViewFactory.NewMenuInstance(CurrentUser.UserType);
            Controller menuController = ControllerFactory.NewMenuControllerInstance(menu);

            mainWindow.sideMenu.Content = (UserControl) menu;
            mainWindow.buttonToggleMenu.Click += menu.Toggle;
            mainWindow.mainWindowContent.Content = ViewFactory.NewViewInstance(AppContext.Start);
            mainWindow.labelUsername.Content = CurrentUser.Lastname + " " + CurrentUser.Name;

            currentAppContext = AppContext.Start;
            App.Current.MainWindow = mainWindow;

            currentWindow.Close();
            mainWindow.Show();
        }


        
        public static void ReturnToLoginWindow() {
            LoginWindow loginWindow = new LoginWindow();
            App.Current.MainWindow = loginWindow;

            mainWindow.Close();
            mainWindow = null;
            loginWindow.Show();

            currentAppContext = AppContext.Login;
        }



        public static void ChangeContext(AppContext context) {
            currentAppContext = context;

            Model model = ModelFactory.NewModelInstance(context);
            View view = ViewFactory.NewViewInstance(context);
            Controller controller = ControllerFactory.NewControllerInstance(model, view, context);

            mainWindow.mainWindowContent.Content = view;
        }


    }//class
}
