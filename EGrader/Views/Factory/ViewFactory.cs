using EGrader.Classes;
using EGrader.Controllers;
using EGrader.Views.Admin;
using EGrader.Views.Menus;
using EGrader.Views.Teacher;
using System.Windows;

namespace EGrader.Views.Factory {

    public enum DialogType { InsertSchool };


    public class ViewFactory {

        public static View NewViewInstance(AppContext context) {
            if (context == AppContext.Start)
                return new StartView();
            else if (context == AppContext.Profile)
                return new ProfileView();
            else if (context == AppContext.Students || context == AppContext.Teachers)
                return new ListableView();
            else if (context == AppContext.StudentGrading)
                return new StudentGradingView();
            return null;
        }


        public static Window NewDialogInstance(AppContext context) {
            if (context == AppContext.Students || context == AppContext.Teachers)
                return new UserDialog();
            else if (context == AppContext.StudentGrading)
                return new InsertGradeDialog();
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
