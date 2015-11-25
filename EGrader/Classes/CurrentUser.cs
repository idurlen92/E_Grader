using EGrader.Models.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGrader.Classes {

    public enum UserType { Admin, Teacher, Student, None };

    public class CurrentUser {

        private static UserObject userObject;


        public static int Id { get { return  (userObject == null) ? -1 : userObject.Id; } }
        public static String BirthDate { get { return (userObject == null) ? "Not logged in" : userObject.BirthDate; } }
        public static String Gender { get { return (userObject == null) ? "Not logged in" : userObject.Gender; } }
        public static String Name { get { return (userObject == null) ? "Not logged in" : userObject.Name;  } }
        public static String Lastname { get { return (userObject == null) ? "Not logged in" : userObject.Lastname; } }
        public static String Username { get { return (userObject == null) ? "Not logged in" : userObject.Username; } }
        public static UserType UserType { get { return (userObject == null) ? UserType.None : userObject.UserType; } }
        

        public static bool IsAdmin() {
            return (userObject != null && userObject.UserType == UserType.Admin);
        }


        public static bool IsStudent() {
            return (userObject != null && userObject.UserType == UserType.Student);
        }


        public static bool isTeacher() {
            return (userObject != null && userObject.UserType == UserType.Teacher);
        }



        public static void LogUserIn(UserObject userObject) {
            CurrentUser.userObject = userObject;
        }
        


        public static void LogUserOut() {
            userObject = null;
        }
        

    }//class
}
