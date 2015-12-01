using EGrader.Classes.Database;
using EGrader.Models.Objects;
using System;
using System.Collections.Generic;
using System.Data;

namespace EGrader.Models {
    class UsersModel : Model{

        private String[] tableColumns = new String[] { "u.id", "u.name", "u.lastname", "u.username", "u.password", "u.user_type_id", "u.works_in",
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


        public override DataTable GetByCriteria(params object[] criteriaParams) {
            String statement = statementBuilder.Select(tableColumns).Join(joinParams).Where(criteriaParams).Create();
            return databaseManager.ExecuteQuery(statement, statementBuilder.WhereParamsDictionary);
        }


        public override List<object> GetObjectsByCriteria(params object[] criteriaParams) {
            List<object> usersList = new List<object>();
            DataTable dataTable = GetByCriteria(criteriaParams);
            foreach (DataRow row in dataTable.Rows)
                usersList.Add(new UserObject(dataTable.Columns, row));
            return usersList;
        }



        public override int Insert(object insertObject) {
            int rowsAffected = -1;
            UserObject user = insertObject as UserObject;

            String statement = "";
            if(user.UserType == Classes.UserType.Teacher)
                statement = statementBuilder.Insert("name", "lastname", "user_type_id", "username", "password", "works_in").Values(
                    user.Name, user.Lastname, user.UserTypeId, user.Username, user.Password, user.WorksIn);
            else
                statement = statementBuilder.Insert("name", "lastname", "user_type_id", "username", "password", "class_id").Values(
                    user.Name, user.Lastname, user.UserTypeId, user.Username, user.Password, user.ClassId);

            rowsAffected = databaseManager.ExecuteStatement(statement, statementBuilder.InsertParamsDictionary);
            return rowsAffected;
        }



        public override int Update(object updateObject) {
            int rowsAffected = -1;
            UserObject user = updateObject as UserObject;

            String statement = "";
            if (user.UserType == Classes.UserType.Teacher)
                statement = statementBuilder.Update("name=", user.Name, "lastname=", user.Lastname, "username=", user.Username, "password=",
                            user.Password).UWhere("id=", user.Id);
            else
                statement = statementBuilder.Update("name=", user.Name, "lastname=", user.Lastname, "username=", user.Username, "password=",
                            user.Password, "class_id=", user.ClassId).UWhere("id=", user.Id);

            rowsAffected = databaseManager.ExecuteStatement(statement, statementBuilder.UpdateParamsDictionary);
            return rowsAffected;
        }




        public List<object> GetStudents(int schoolId) {
            List<object> usersList = new List<object>();
            String[,] joinArrays = new String[,] { { "classes_in_schools c", "u.class_id", "c.id" }, { "user_types ut", "ut.id", "u.user_type_id" } };
            String[] selectColumns = new String[] { "u.id", "u.name", "u.lastname", "u.username", "u.password", "u.user_type_id", "u.works_in",
                "u.class_id", "ut.user_type_name" };

            String statement = statementBuilder.Select(selectColumns).Join(joinArrays).Where("c.school_id =", schoolId, "AND u.user_type_id=", 3).Create();
            DataTable dataTable = databaseManager.ExecuteQuery(statement, statementBuilder.WhereParamsDictionary);

            foreach (DataRow row in dataTable.Rows)
                usersList.Add(new UserObject(dataTable.Columns, row));

            return usersList;
        }



    }//class
}
