using EGrader.Models;
using EGrader.Views;
using EGrader.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EGrader.Controllers {

    public enum AppContext { Login, Start, Profile };


    class AppController {

        static AppContext currentAppContext = AppContext.Login;

        static MainWindow mainWindow;



        public static void CreateMainWindow() {
            Window currentWindow = App.Current.MainWindow;

            mainWindow = new MainWindow();
            mainWindow.sideMenu.Content = MenuFactory.NewInstance();
            ChangeContext(AppContext.Start);

            App.Current.MainWindow = mainWindow;

            currentWindow.Close();
            mainWindow.Show();
        }


        
        public static void ChangeContext(AppContext context) {
            currentAppContext = context;

            Model model = ModelFactory.NewInstance(context);
            Controller controller = ControllerFactory.NewInstance(model, context);
            mainWindow.mainWindowContent.Content = ViewFactory.NewInstance(controller, model, context);
        }


    }//class
}
