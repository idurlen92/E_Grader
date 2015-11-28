using EGrader.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGrader.Models.Objects {
    public class UserObject {

        private int id;
        private int classId;
        private int worksIn;
        private int userTypeId;

        private String name;
        private String lastname;
        private String username;

        private UserType userType; 


        public UserObject() {
        }


        public UserObject(List<String> fieldsList) {
            this.id = Convert.ToInt32(fieldsList[0]);
            this.name = fieldsList[1];
            this.lastname = fieldsList[2];
            this.username = fieldsList[3];
            this.userTypeId = Convert.ToInt32(fieldsList[4]);
            this.worksIn = (fieldsList[5].CompareTo("-") == 0) ? 0 : Convert.ToInt32(fieldsList[5]);
            this.classId = (fieldsList[6].CompareTo("-") == 0) ? 0 : Convert.ToInt32(fieldsList[6]);

            String userType = fieldsList[7].ToLower();
            if (userType.Contains("admin"))
                this.userType = UserType.Admin;
            else
                this.userType = (userType.Contains("student") ? UserType.Student : UserType.Teacher);
        }


        // ---------- Getter properties ----------
        public int Id { get { return id; } }
        public int ClassId { get { return classId; } }
        public int UserTypeId { get { return userTypeId; } }
        public int WorksIn { get { return worksIn; } }
        public String Name { get { return name; } }
        public String Lastname { get { return lastname; } }
        public String Username { get { return username; } }
        public UserType UserType { get { return userType; } }


        // ---------- Setters ----------
        public void SetId(int id) { this.id = id; }
        public void SetClassId(int classId) { this.classId = classId; }
        public void SetUserTypeId(int userTypeId) { this.userTypeId = userTypeId; }
        public void SetWorksIn(int worksIn) { this.worksIn = worksIn; }
        public void SetName(String name) { this.name = name; }
        public void SetLastname(String lastname) { this.lastname = lastname; }
        public void SetUsername(String username) { this.username = username; }


    }//class
}
