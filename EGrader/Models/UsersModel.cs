using EGrader.Classes.Database;
using EGrader.Models.Objects;
using System;
using System.Collections.Generic;
using System.Data;

namespace EGrader.Models {
    class UsersModel : Model{

        private String[] columns = new String[] { "u.id", "u.name", "u.lastname", "u.username", "u.user_type_id", "u.works_in",
                    "u.class_id", "ut.user_type_name" };
        private String[] joinParams = new String[] { "user_types ut", "u.user_type_id", "ut.id" };


        // ---------- Constructor ----------
        public UsersModel() : base("users") {  }



        public override int Delete(object deleteObject) {
            List<object> deleteList = new List<object>();
            deleteList.Add(deleteObject);
            return Delete(deleteList);
        }


        public override int Delete(List<object> objectsToDeleteList) {
            int rowsAffected = -1;
            
            try {
                databaseManager.StartTransaction();
                foreach (UserObject user in objectsToDeleteList) {
                    String statement = statementBuilder.Delete("id=", user.Id);
                    rowsAffected = databaseManager.ExecuteStatement(statement, statementBuilder.DeleteParamsDictionary);
                }
                databaseManager.CommitTransaction();
            }
            catch (StatementBuilderException e) {
                databaseManager.RollBackTransacion();
                Console.WriteLine(e.Message + ":\n" + e.StackTrace);
            }
            catch (Exception e) {
                databaseManager.RollBackTransacion();
                Console.WriteLine(e.Message + ":\n" + e.StackTrace);
            }

            return rowsAffected;
        }



        public override DataTable GetAll() {
            String statement = statementBuilder.Select(columns).Join(joinParams).Create();    
            return databaseManager.ExecuteQuery(statement);
        }

        public override List<object> GetAllObjects() {
            List<object> usersList = new List<object>();
            DataTable dataTable = GetAll();
            foreach (DataRow row in dataTable.Rows)
                usersList.Add(new UserObject(dataTable.Columns, row));
            return usersList;
        }


        public override DataTable GetByCriteria(params object[] criteriaParams) {
            String statement = statementBuilder.Select(columns).Join(joinParams).Where(criteriaParams).Create();
            return databaseManager.ExecuteQuery(statement, statementBuilder.WhereParamsDictionary);
        }


        public override List<object> GetObjectsByCriteria(params object[] criteriaParams) {
            List<object> usersList = new List<object>();
            DataTable dataTable = GetByCriteria(criteriaParams);
            foreach (DataRow row in dataTable.Rows)
                usersList.Add(new UserObject(dataTable.Columns, row));
            return usersList;
        }


        public DataTable GetStudents(int schoolId) {
            String[,] joinArrays = new String[,] { { "classes_in_schools c", "u.class_id", "c.id" }, { "user_types ut", "ut.id", "u.user_type_id"} };
            String[] selectColumns = new String[] { "u.id", "u.name", "u.lastname", "u.username", "u.user_type_id", "u.works_in",
                "u.class_id", "ut.user_type_name" };
            String statement = statementBuilder.Select(selectColumns).Join(joinArrays).Where("c.school_id =", schoolId, "AND u.user_type_id=", 3).Create();
            return databaseManager.ExecuteQuery(statement, statementBuilder.WhereParamsDictionary);
        }


    }//class
}
