using EGrader.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGrader.Models.Objects {
    class UserObject {

        private int id;

        private String name;
        private String lastname;
        private String username;
        private String birthDate;
        private String gender;

        private UserType userType; 


        public UserObject(List<String> fieldsList) {
            this.id = Convert.ToInt32(fieldsList[0]);
            this.name = fieldsList[1];
            this.lastname = fieldsList[2];
            this.username = fieldsList[3];
            this.birthDate = fieldsList[4];
            this.gender = fieldsList[5];

            String userType = fieldsList[6].ToLower();
            if (userType.Contains("admin"))
                this.userType = UserType.Admin;
            else
                this.userType = (userType.Contains("student") ? UserType.Student : UserType.Teacher);
        }


        // ---------- Getters ----------
        public int GetId() {
            return id;
        }


        public String GetName() {
            return name;
        }


        public String GetLastname() {
            return lastname;
        }


        public  String GetUsername() {
            return username;
        }


        public String GetBirthDate() {
            return birthDate;
        }


        public String GetGender() {
            return gender;
        }


        public UserType GetUserType() {
            return userType;
        }


        // ---------- Setters ----------
        public void SetId(int id) {
            this.id = id;
        }


        public void SetName(String name) {
            this.name = name;
        }


        public void SetLastname(String lastname) {
            this.lastname = lastname;
        }


        public void SetUsername(String username) {
            this.username = username;
        }


        public void SetBirthDate(String birthDate) {
            this.birthDate = birthDate;
        }


        public void SetGender(String gender) {
            this.gender = gender;
        }




    }//class
}
