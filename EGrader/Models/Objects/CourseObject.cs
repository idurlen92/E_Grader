using System;
using System.Data;

namespace EGrader.Models.Objects {
    public class CourseObject {

        int id;
        string courseName;


        public CourseObject() { }
             
        public CourseObject(DataColumnCollection columns, DataRow row) {
            this.id = columns.Contains("id") ? Convert.ToInt32(row["id"]) : -1;
            this.courseName = columns.Contains("course_name") ? Convert.ToString(row["course_name"]) : "-";
        }


        public int Id { get { return id; } }
        public String CourseName { get { return courseName; } }


        public void SetId(int id) { this.id = id; }
        public void SetCourseName(String courseName) { this.courseName = courseName; }


    }//class
}
