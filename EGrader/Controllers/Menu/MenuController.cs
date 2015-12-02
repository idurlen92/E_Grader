using EGrader.Classes;
using EGrader.Views.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EGrader.Controllers.Menu {
    public class MenuController : Controller {

        MenuView menu;


        public MenuController(MenuView menu) {
            this.menu = menu;
            if (menu is AdminMenu) {
                foreach (Button button in ((AdminMenu) menu).contentHolder.Children)
                    button.Click += DoAction;
            }
            else if (menu is StudentMenu) {
                foreach (Button button in ((StudentMenu) menu).contentHolder.Children)
                    button.Click += DoAction;
            }
            else {
                foreach (Button button in ((TeacherMenu) menu).contentHolder.Children)
                    button.Click += DoAction;
            }
        }

       
        public void DoAction(object sender, RoutedEventArgs e) {
            String buttonName = ((Button) sender).Name.ToLower();

            if (buttonName.Contains("logout"))
                ActionLogOut();
            else if (buttonName.Contains("profile"))
                AppController.ChangeContext(AppContext.Profile);
            else if (buttonName.ToLower().Contains("studentgrading"))
                AppController.ChangeContext(AppContext.StudentGrading);
            else if (buttonName.Contains("teachers"))
                AppController.ChangeContext(AppContext.Teachers);
            else if (buttonName.Contains("students"))
                AppController.ChangeContext(AppContext.Students);
            else if (buttonName.Contains("classes"))
                AppController.ChangeContext(AppContext.ClassAdministration);
            else if (buttonName.Contains("grades"))
                AppController.ChangeContext(AppContext.Grades);

            menu.Toggle(sender, e);
        }
       


        public void ActionLogOut() {
            if(MessageBox.Show("Odjava", "Želite li se odjaviti?", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
                CurrentUser.LogUserOut();
                AppController.ReturnToLoginWindow();
            }
        }


    }//class
}
