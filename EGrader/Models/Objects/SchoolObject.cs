using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGrader.Models.Objects {
    public class SchoolObject {

        private int id;
        private String schoolName;
        private String address;

        public SchoolObject(List<String> fieldsList) {
            this.id = Convert.ToInt32(fieldsList[0]);
            this.schoolName = fieldsList[1];
            this.address = fieldsList[2];
        }

        // ---------- Getters ----------
        public int Id { get { return id; } }
        public String SchoolName { get { return schoolName; } }
        public String Address { get { return address; } }


        // ---------- Setters ----------
        public void SetId(int id) { this.id = id; }
        public void SetAddress(String address) { this.address = address; }
        public void SetSchoolName(String schoolName) { this.schoolName = schoolName; }
        


    }//class
}
