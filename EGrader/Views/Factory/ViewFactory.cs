using EGrader.Controllers;
using EGrader.Models;
using EGrader.Views.Admin;
using System.Windows;
using System.Windows.Controls;

namespace EGrader.Views.Factory {

    public enum DialogType { InsertSchool };

    public class ViewFactory {

        public static UserControl NewViewInstance(Controller controller, Model model, AppContext context) {
            //TODO:
            if (context == AppContext.Start)
                return new StartView();
            else if (context == AppContext.Profile)
                return new ProfileView(controller, model);
            else if (context == AppContext.Grades)
                return new GradesView(controller, model);
            else if (context == AppContext.Users)
                return new ListableView(controller, model);
            return null;
        }


        public static Window NewDialogInstance(Controller controller, Model model, DialogType type) {
            //TODO:
            return null;
        }

    }
}
