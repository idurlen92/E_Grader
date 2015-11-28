using EGrader.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EGrader.Controllers.Menu {
    public class MenuController : Controller {


        public void AttachView(UserControl view) {
            throw new NotImplementedException("Not using in Menu context");
        }


        public void DoAction(object sender, RoutedEventArgs e) {
            Button clickedButton = (Button) sender;

            if (clickedButton.Name.ToLower().Contains("logout"))
                ActionLogOut();
            else if (clickedButton.Name.ToLower().Contains("profile"))
                AppController.ChangeContext(AppContext.Profile);
            else if (clickedButton.Name.ToLower().Contains("school"))
                AppController.ChangeContext(AppContext.Schools);
            else if (clickedButton.Name.ToLower().Contains("users"))
                AppController.ChangeContext(AppContext.Users);
            else if (clickedButton.Name.ToLower().Contains("grades"))
                AppController.ChangeContext(AppContext.Grades);
        }


        public void ActionLogOut() {
            if(MessageBox.Show("Odjava", "Želite li se odjaviti?", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
                CurrentUser.LogUserOut();
                AppController.ReturnToLoginWindow();
            }
        }


    }//class
}
