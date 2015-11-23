using EGrader.Classes;
using EGrader.Controllers.Factory;
using EGrader.Models;
using EGrader.Models.Factory;
using EGrader.Views;
using EGrader.Views.Factory;
using EGrader.Views.Menus;
using EGrader.Views.Menus.Factory;
using EGrader.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EGrader.Controllers {

    public enum AppContext { Login, Start, Profile };


    class AppController {


        static AppContext currentAppContext = AppContext.Login;
        static MainWindow mainWindow;


        public static void CreateMainWindow() {
            Window currentWindow = App.Current.MainWindow;
            mainWindow = new MainWindow();

            //MenuView menu = MenuViewFactory.NewMenuInstance(CurrentUser.GetUserType());
            MenuView menu = MenuViewFactory.NewMenuInstance(UserType.Teacher);//TODO: remove; TEMP

            mainWindow.sideMenu.Content = (UserControl) menu;
            mainWindow.buttonToggleMenu.Click += menu.Toggle;
            mainWindow.mainWindowContent.Content = ViewFactory.NewStartViewInstance();


            currentAppContext = AppContext.Start;
            App.Current.MainWindow = mainWindow;
            currentWindow.Close();
            mainWindow.Show();
        }


        
        public static void ChangeContext(AppContext context) {
            currentAppContext = context;

            try {
                Model model = ModelFactory.NewModelInstance(context);
                Controller controller = ControllerFactory.NewControllerInstance(model, context);
                UserControl view = ViewFactory.NewViewInstance(controller, model, context);

                mainWindow.mainWindowContent.Content = view;
                controller.AttachView(view);
            }
            catch(Exception e) {
                Console.WriteLine(e.Message + ":\n" + e.StackTrace);
            }
        }


    }//class
}
