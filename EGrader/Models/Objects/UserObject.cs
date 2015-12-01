using EGrader.Classes;
using System;
using System.Data;

namespace EGrader.Models.Objects {
    public class UserObject {

        private int id = -1;
        private int classId = -1;
        private int worksIn = -1;
        private int userTypeId = -1;

        private String name;
        private String lastname;
        private String username;
        private String password;

        private UserType userType; 


        public UserObject() {
        }



        public UserObject(DataColumnCollection columns, DataRow row) {
            this.id = columns.Contains("id") ? Convert.ToInt32(row["id"]) : -1;
            this.classId = (columns.Contains("class_id") && !row.IsNull("class_id")) ? Convert.ToInt32(row["class_id"]) : -1;
            this.userTypeId = columns.Contains("user_type_id") ? Convert.ToInt32(row["user_type_id"]) : -1;
            this.worksIn = (columns.Contains("works_in") && !row.IsNull("works_in")) ? Convert.ToInt32(row["works_in"]) : -1;

            this.name = columns.Contains("name") ? Convert.ToString(row["name"]) : "-";
            this.lastname = columns.Contains("lastname") ? Convert.ToString(row["lastname"]) : "-";
            this.username = columns.Contains("username") ? Convert.ToString(row["username"]) : "-";
            this.password = columns.Contains("password") ? Convert.ToString(row["password"]) : "-";

            String userType = columns.Contains("user_type_name") ? Convert.ToString(row["user_type_name"]) : "-";
            if (userType.ToLower().Contains("admin"))
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
        public String Password { get { return password; } }
        public UserType UserType { get { return userType; } }


        // ---------- Setters ----------
        public void SetId(int id) { this.id = id; }
        public void SetClassId(int classId) { this.classId = classId; }
        public void SetWorksIn(int worksIn) { this.worksIn = worksIn; }
        public void SetName(String name) { this.name = name; }
        public void SetLastname(String lastname) { this.lastname = lastname; }
        public void SetUsername(String username) { this.username = username; }
        public void SetPassword(String password) { this.password = password; }


        public void SetUserTypeId(int userTypeId) {
            this.userTypeId = userTypeId;
            if (userTypeId == 1)
                this.userType = UserType.Admin;
            else
                this.userType = (userTypeId == 2) ? UserType.Teacher : UserType.Student;
            }


    }//class
}
