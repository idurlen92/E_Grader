using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGrader.Classes {

    public enum UserType { Admin, Teacher, Student, None };


    class CurrentUser {    

        private static int userId;
        private static String username;
        private static UserType userType = UserType.None;



        public static int GetUserId(){ 
            return userId;
        }


        public static String GetUsername(){
            return username;
        }


        public static UserType GetUserType() {
            return userType;
        }



        public static bool IsAdmin() {
            return userType == UserType.Admin;
        }



        public static bool IsStudent() {
            return userType == UserType.Student;
        }



        public static bool isTeacher() {
            return userType == UserType.Teacher;
        }



        public static void LogUserIn(String username, int userId, String userType) {
            CurrentUser.username = username;
            CurrentUser.userId = userId;
            if (userType.StartsWith("student", StringComparison.OrdinalIgnoreCase))
                CurrentUser.userType = UserType.Student;
            else
                CurrentUser.userType = (userType.StartsWith("admin", StringComparison.OrdinalIgnoreCase) ? UserType.Admin : UserType.Teacher);
        }
        


        public static void LogUserOut() {
            CurrentUser.username = "";
            CurrentUser.userId = 0;
            CurrentUser.userType = UserType.None;
        }
        


    }
}
