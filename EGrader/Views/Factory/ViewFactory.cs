using EGrader.Classes;
using EGrader.Controllers;
using EGrader.Views.Admin;
using EGrader.Views.Menus;
using EGrader.Views.Student;
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
                return new UsersView();
            else if (context == AppContext.ClassAdministration)
                return new ClassesView();
            else if (context == AppContext.StudentGrading)
                return new StudentGradingView();
            else if (context == AppContext.Grades)
                return new GradesView();
            return null;
        }


        public static Window NewDialogInstance(AppContext context) {
            if (context == AppContext.Students || context == AppContext.Teachers)
                return new UserDialog();
            else if (context == AppContext.StudentGrading)
                return new InsertGradeDialog();
            else if (context == AppContext.ClassAdministration)
                return new ClassDialog();
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
