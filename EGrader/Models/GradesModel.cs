﻿using EGrader.Classes.Database;
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



        public override int Insert(object insertObject) {
            throw new NotImplementedException();
        }


        public override int Update(object updateObject) {
            throw new NotImplementedException();
        }
    }
}
