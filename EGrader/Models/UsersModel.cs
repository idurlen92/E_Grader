using EGrader.Classes;
using EGrader.Classes.Database;
using EGrader.Models.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGrader.Models {
    class UsersModel : Model{

        // ---------- Constructor ----------
        public UsersModel() : base("users") {  }



        public override int Delete(object deleteObject) {
            List<object> deleteList = new List<object>();
            deleteList.Add(deleteObject);
            return Delete(deleteList);
        }


        public override int Delete(List<object> objectsToDeleteList) {
            int rowsAffected = 0;
            
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


        public override List<object> GetAll() {
            List<object> userObjectList = new List<object>();

            try {
                String statement = statementBuilder.Select("u.id", "u.name", "u.lastname", "u.username", "u.user_type_id", "u.works_in", 
                    "u.class_id", "ut.user_type_name").Join(new String[] { "user_types ut", "u.user_type_id", "ut.id" }).Create();    
                foreach (List<String> userRow in databaseManager.ExecuteQuery(statement))
                    userObjectList.Add(new UserObject(userRow));
            }
            catch (StatementBuilderException e) {
                Console.WriteLine(e.Message + ":\n" + e.StackTrace);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message + ":\n" + e.StackTrace);
            }

            return userObjectList;
        }



        public override List<object> GetByCriteria(params object[] criteriaParams) {
            List<object> userObjectList = new List<object>();

            try {
                String statement = statementBuilder.Select("u.id", "u.name", "u.lastname", "u.username", "u.user_type_id", "u.works_in",
                   "u.class_id", "ut.user_type_name").Join(new String[] { "user_types ut", "u.user_type_id", "ut.id" }).Where(criteriaParams).Create();
                foreach (List<String> userRow in databaseManager.ExecuteQuery(statement, statementBuilder.WhereParamsDictionary))
                    userObjectList.Add(new UserObject(userRow));
            }
            catch (StatementBuilderException e) {
                Console.WriteLine(e.Message + ":\n" + e.StackTrace);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message + ":\n" + e.StackTrace);
            }

            return userObjectList;
        }



        public override object GetById(int id) {
            UserObject user = null;

            try {
                String statement = statementBuilder.Select("u.id", "u.name", "u.lastname", "u.username", "u.user_type_id", "u.works_in",
                    "u.class_id", "ut.user_type_name").Join(new String[] { "user_types ut", "u.user_type_id", "ut.id" }).Where("u.id=", id).Create();
                List<List<string>> resultList = databaseManager.ExecuteQuery(statement, statementBuilder.WhereParamsDictionary);
                user = new UserObject(resultList[0]);
            }
            catch (StatementBuilderException e) {
                Console.WriteLine(e.Message + ":\n" + e.StackTrace);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message + ":\n" + e.StackTrace);
            }

            return user;
        }


        public override List<object> Execute() {
            throw new NotImplementedException();
        }


    }//class
}
