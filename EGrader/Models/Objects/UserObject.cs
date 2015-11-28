using EGrader.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGrader.Models.Objects {
    public class UserObject {

        private int id;

        private String name;
        private String lastname;
        private String username;
        private String birthDate;
        private String gender;

        private UserType userType; 



        public UserObject(int id, String name, String lastname, String username, String birthDate, String gender, UserType userType) {
            this.id = id;
            this.name = name;
            this.lastname = lastname;
            this.username = username;
            this.birthDate = birthDate;
            this.gender = gender;
            this.userType = userType;
        }


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


        // ---------- Getter properties ----------
        public int Id { get { return id; } }
        public String BirthDate { get { return birthDate; } }
        public String Gender { get { return gender; } }
        public String Name { get { return name; } }
        public String Lastname { get { return lastname; } }
        public String Username { get { return username; } }
        public UserType UserType { get { return userType; } }


        // ---------- Setters ----------
        public void SetId(int id) { this.id = id; }
        public void SetBirthDate(String birthDate) { this.birthDate = birthDate; }
        public void SetName(String name) { this.name = name; }
        public void SetLastname(String lastname) { this.lastname = lastname; }
        public void SetUsername(String username) { this.username = username; }
        public void SetGender(String gender) { this.gender = gender; }


    }//class
}
