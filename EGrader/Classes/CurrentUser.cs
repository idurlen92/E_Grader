using EGrader.Models.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGrader.Classes {

    public enum UserType { Admin, Teacher, Student, None };


    class CurrentUser {

        private static UserObject userObject;



        public static int Id { get { return  (userObject == null) ? -1 : userObject.GetId(); } }
        public static String BirthDate { get { return (userObject == null) ? "Not logged in" : userObject.GetBirthDate(); } }
        public static String Gender { get { return (userObject == null) ? "Not logged in" : userObject.GetGender(); } }
        public static String Name { get { return (userObject == null) ? "Not logged in" : userObject.GetName();  } }
        public static String Lastname { get { return (userObject == null) ? "Not logged in" : userObject.GetLastname(); } }
        public static String Username { get { return (userObject == null) ? "Not logged in" : userObject.GetUsername(); } }
        public static UserType UserType { get { return (userObject == null) ? UserType.None : userObject.GetUserType(); } }
        

        public static bool IsAdmin() {
            return (userObject != null && userObject.GetUserType() == UserType.Admin);
        }


        public static bool IsStudent() {
            return (userObject != null && userObject.GetUserType() == UserType.Student);
        }


        public static bool isTeacher() {
            return (userObject != null && userObject.GetUserType() == UserType.Teacher);
        }



        public static void LogUserIn(UserObject userObject) {
            CurrentUser.userObject = userObject;
        }
        


        public static void LogUserOut() {
            CurrentUser.userObject = null;
        }
        

    }//class
}
