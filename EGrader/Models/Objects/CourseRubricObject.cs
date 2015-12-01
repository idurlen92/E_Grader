using System;
using System.Data;

namespace EGrader.Models.Objects {
    public class CourseRubricObject {

        int id;
        int courseId;

        string courseName;
        string rubricName;
        


        public CourseRubricObject() { }


        public CourseRubricObject(DataColumnCollection columns, DataRow row) {
            this.id = columns.Contains("id") ? Convert.ToInt32(row["id"]) : -1;
            this.courseId = columns.Contains("course_id") ? Convert.ToInt32(row["course_id"]) : -1;
            this.courseName = columns.Contains("course_name") ? Convert.ToString(row["course_name"]) : "-";
            this.rubricName = columns.Contains("rubric_name") ? Convert.ToString(row["rubric_name"]) : "-";
        }


        public int Id { get { return id; } }
        public int CourseId { get { return courseId; } }
        public String CourseName { get { return courseName; } }
        public String RubricName { get { return rubricName; } }


        public void SetId(int id) { this.id = id; }
        public void SetCourseId(int courseId) { this.courseId = courseId; }
        public void SetCourseName(String courseName) { this.courseName = courseName; }
        public void SetRubricName(String rubricName) { this.rubricName = rubricName; }

    }
}
