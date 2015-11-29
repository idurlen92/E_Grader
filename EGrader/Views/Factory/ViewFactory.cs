using EGrader.Classes;
using EGrader.Controllers;
using EGrader.Models;
using EGrader.Views.Admin;
using EGrader.Views.Menus;
using System.Windows;
using System.Windows.Controls;

namespace EGrader.Views.Factory {

    public enum DialogType { InsertSchool };


    public class ViewFactory {

        public static View NewViewInstance(AppContext context) {
            if (context == AppContext.Start)
                return new StartView();
            else if (context == AppContext.Profile)
                return new ProfileView();
            else if (context == AppContext.Grades)
                return new GradesView();
            else if (context == AppContext.Students || context == AppContext.Teachers)
                return new ListableView();
            return null;
        }


        public static Window NewDialogInstance(Controller controller, Model model, DialogType type) {
            //TODO:
            return null;
        }


        public static MenuView NewMenuInstance(UserType userType) {
            if (userType == UserType.Admin)
                return new AdminMenu();
            else if (userType == UserType.Student)
                return new StudentMenu();
            else
                return new TeacherMenu();
        }


    }//class
}
