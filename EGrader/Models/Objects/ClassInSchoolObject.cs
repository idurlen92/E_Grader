using System;
using System.Data;

namespace EGrader.Models.Objects {
    class ClassInSchoolObject {

        int id;// This is id that goes to students
        int classId;
        int schoolId;
        int teacherId;

        String className;
        String schoolName;
        String teacherName;


        public ClassInSchoolObject() { }

        public ClassInSchoolObject(DataColumnCollection columns, DataRow row) {
            this.id = columns.Contains("id") ? Convert.ToInt32(row["id"]) : -1;
            this.classId = (columns.Contains("class_id") && !row.IsNull("class_id")) ? Convert.ToInt32(row["class_id"]) : -1;
            this.schoolId = (columns.Contains("school_id") && !row.IsNull("school_id")) ? Convert.ToInt32(row["school_id"]) : -1;
            this.teacherId = (columns.Contains("teacher_id") && !row.IsNull("teacher_id")) ? Convert.ToInt32(row["teacher_id"]) : -1;
            this.className = (columns.Contains("class_name") && !row.IsNull("class_name")) ? Convert.ToString(row["class_name"]) : "-";
            this.schoolName = (columns.Contains("school_name") && !row.IsNull("school_name")) ? Convert.ToString(row["school_name"]) : "-";
            this.teacherName = (columns.Contains("teacher") && !row.IsNull("teacher")) ? Convert.ToString(row["teacher"]) : "-";
        }



        public int Id { get { return id; } }
        public int ClassId { get { return classId; } }
        public int SchoolId { get { return schoolId; } }
        public int TeacherId { get { return teacherId; } }
        public String ClassName { get { return className; } }
        public String SchoolName { get { return schoolName; } }
        public String TeacherName { get { return teacherName; } }



        public void SetId(int id) { this.id = id; }
        public void SetClassId(int classId) { this.classId = classId; }
        public void SetSchoolId(int schoolId) { this.schoolId = schoolId; }
        public void SetTeacherId(int teacherId) { this.teacherId = teacherId; }
        public void SetClassName(String className) { this.className = className; }
        public void SetSchoolName(String schoolName) { this.schoolName = schoolName; }
        public void SetTeacherName(String teacherName) { this.teacherName = teacherName; }


    }//class
}
