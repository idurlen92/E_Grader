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
        public static int ClassId { get { return (userObject == null) ? -1 : userObject.ClassId; } }
        public static int UserTypeId { get { return (userObject == null) ? -1 : userObject.UserTypeId; } }
        public static int WorksIn { get { return (userObject == null) ? -1 : userObject.WorksIn; } }// ----- Id of school admin/teacher works in -----
        public static String Name { get { return (userObject == null) ? "Not logged in" : userObject.Name;  } }
        public static String Lastname { get { return (userObject == null) ? "Not logged in" : userObject.Lastname; } }
        public static String Username { get { return (userObject == null) ? "Not logged in" : userObject.Username; } }
        public static UserType UserType { get { return (userObject == null) ? UserType.None : userObject.UserType; } }
        

        /// <summary>
        /// Provjerava da li je trenutni korisnik u admin ulozi.
        /// </summary>
        /// <returns>boolean</returns>
        public static bool IsAdmin() {
            return (userObject != null && userObject.UserType == UserType.Admin);
        }


        /// <summary>
        /// Provjerava da li je trenutni korisnik u ucenuk ulozi.
        /// </summary>
        /// <returns></returns>
        public static bool IsStudent() {
            return (userObject != null && userObject.UserType == UserType.Student);
        }


        /// <summary>
        /// Provjerava da li je trenutni korisnik u ucitelj ulozi.
        /// </summary>
        /// <returns></returns>
        public static bool isTeacher() {
            return (userObject != null && userObject.UserType == UserType.Teacher);
        }



        /// <summary>
        /// Logiranje korisnika.
        /// </summary>
        /// <param name="userObject"></param>
        public static void LogUserIn(UserObject userObject) {
            CurrentUser.userObject = userObject;
        }
        


        /// <summary>
        /// Odjava korisnika.
        /// </summary>
        public static void LogUserOut() {
            userObject = null;
        }
        

    }//class
}
