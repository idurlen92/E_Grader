using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGrader.Models.Objects {
    public class GradeObject {

        int grade;
        int rubricId;
        int studentId;
        int teacherId;

        String date = "";
        String note = "";


        public GradeObject() { }


        public GradeObject(DataColumnCollection columns, DataRow row) {
            this.grade = columns.Contains("grade") ? Convert.ToInt32(row["grade"]) : -1;
            this.rubricId = columns.Contains("rubric_id") ? Convert.ToInt32(row["rubric_id"]) : -1;
            this.studentId = columns.Contains("student_id") ? Convert.ToInt32(row["student_id"]) : -1;
            this.teacherId = columns.Contains("teacher_id") ? Convert.ToInt32(row["teacher_id"]) : -1;
            this.date = (columns.Contains("date") && !row.IsNull("date")) ? Convert.ToString(row["date"]) : "-";
            this.note = (columns.Contains("note") && !row.IsNull("note")) ? Convert.ToString(row["note"]) : "-";

            if (!this.date.Equals("-"))
                this.date = this.date.Substring(0, this.date.IndexOf(' ') + 1);
        }


        public int Grade { get { return grade; } }
        public int RubricId { get { return rubricId; } }
        public int StudentId { get { return studentId; } }
        public int TeacherId { get { return teacherId; } }
        public String Date { get { return date; } }
        public String Note { get { return note; } }


        public void SetGrade(int grade) { this.grade = grade; }
        public void SetRubricId(int rubricId) { this.rubricId = rubricId; }
        public void SetStudentId(int studentId) { this.studentId = studentId; }
        public void SetTeacherId(int teacherId) { this.teacherId = teacherId; }
        public void SetDate(String date) { this.date = date; }
        public void SetNote(String note) { this.note = note; }


    }//class
}