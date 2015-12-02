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
            int rowsAffected = -1;

            try {
                UserObject user = deleteObject as UserObject;
                String statement = statementBuilder.Delete("id=", user.Id);
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
            String statement = statementBuilder.Select(tableColumns).Join(joinParams).Where(criteriaParams).OrderBy("u.lastname ASC, u.name ASC").Create();
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
            String[,] joinArray = new String[,] { { "classes_in_schools cs", "u.class_id", "cs.id" }, { "user_types ut", "ut.id", "u.user_type_id" } };

            String statement = statementBuilder.Select(tableColumns).Join(joinArray).Where("cs.school_id =", schoolId, "AND u.user_type_id=", 3).Create();
            DataTable dataTable = databaseManager.ExecuteQuery(statement, statementBuilder.WhereParamsDictionary);

            foreach (DataRow row in dataTable.Rows)
                usersList.Add(new UserObject(dataTable.Columns, row));

            return usersList;
        }


        public List<object> GetStudentsOfTeacher(int teacherId) {
            List<object> usersList = new List<object>();
            String[,] joinArray = new String[,] { { "classes_in_schools cs", "u.class_id", "cs.id" }, { "user_types ut", "ut.id", "u.user_type_id" } };

            String statement = statementBuilder.Select(tableColumns).Join(joinArray).Where("u.user_type_id=", 3, "AND cs.teacher_id =", teacherId).Create();
            DataTable dataTable = databaseManager.ExecuteQuery(statement, statementBuilder.WhereParamsDictionary);

            foreach (DataRow row in dataTable.Rows)
                usersList.Add(new UserObject(dataTable.Columns, row));

            return usersList;
        }



    }//class
}
