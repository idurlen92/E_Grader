using EGrader.Classes.Database;
using EGrader.Models.Objects;
using System;
using System.Collections.Generic;
using System.Data;

namespace EGrader.Models {
    class GradesModel : Model {


        public GradesModel() : base("grades") { }


        public override int Delete(object deleteObject) {
            int rowsAffected = -1;

            try {
                GradeObject gradeObj = deleteObject as GradeObject;
                String statement = statementBuilder.Delete("date=", gradeObj.Date, "AND student_id=", gradeObj.StudentId, "AND teacher_id=",
                                    gradeObj.TeacherId, "AND rubric_id=", gradeObj.RubricId);
                rowsAffected = databaseManager.ExecuteStatement(statement, statementBuilder.DeleteParamsDictionary);
            }
            catch (StatementBuilderException e) {
                Console.WriteLine(e.Message + ":\n" + e.StackTrace);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message + ":\n" + e.StackTrace);
            }

            return rowsAffected;
        }


        public override DataTable GetByCriteria(params object[] criteriaParams) {
            String statement = statementBuilder.Select("*").Where(criteriaParams).Create();
            return databaseManager.ExecuteQuery(statement, statementBuilder.WhereParamsDictionary);
        }


        public override List<object> GetObjectsByCriteria(params object[] criteriaParams) {
            List<object> gradesList = new List<object>();
            DataTable dataTable = GetByCriteria(criteriaParams);
            foreach (DataRow row in dataTable.Rows)
                gradesList.Add(new GradeObject(dataTable.Columns, row));
            return gradesList;
        }


        public List<object> GetStudentGrades(int studentId, int rubricID) {
            List<object> gradesList = new List<object>();
            String[] selectColumns = { "g.date", "g.student_id", "g.teacher_id", "g.rubric_id", "g.grade", "g.note" };
            String[] joinArray = { "course_rubrics cr", "cr.id", "g.rubric_id" };

            String statement = statementBuilder.Select(selectColumns).Join(joinArray).Where("g.student_id=", studentId, " AND g.rubric_id=", rubricID).Create();
            DataTable dataTable = databaseManager.ExecuteQuery(statement, statementBuilder.WhereParamsDictionary);

            foreach (DataRow row in dataTable.Rows)
                gradesList.Add(new GradeObject(dataTable.Columns, row));            

            return gradesList;
        }



        public override int Insert(object insertObject) {
            int rowsAffected = -1;
            try {
                GradeObject gradeObj = insertObject as GradeObject;

                String statement = "";
                if (gradeObj.Note.Length > 0)
                    statement = statementBuilder.Insert("student_id", "teacher_id", "rubric_id", "date", "grade", "note").Values(
                                gradeObj.StudentId, gradeObj.TeacherId, gradeObj.RubricId, gradeObj.Date, gradeObj.Grade, gradeObj.Note);
                else
                    statement = statementBuilder.Insert("student_id", "teacher_id", "rubric_id", "date", "grade").Values(
                            gradeObj.StudentId, gradeObj.TeacherId, gradeObj.RubricId, gradeObj.Date, gradeObj.Grade);

                rowsAffected = databaseManager.ExecuteStatement(statement, statementBuilder.InsertParamsDictionary);
            }
            catch(Exception e) {
                Console.WriteLine(e.Message + ":\n" + e.StackTrace);
            }
            return rowsAffected;
        }


        public override int Update(object updateObject) {
            throw new NotImplementedException();
        }
    }
}
