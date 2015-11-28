using EGrader.Classes.Database;
using EGrader.Models.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGrader.Models {
    public class SchoolsModel : Model {


        //---------- Constructor ----------
        public SchoolsModel() : base("schools") { }


        public override int Delete(object deleteObject) {
            List<object> deleteList = new List<object>();
            deleteList.Add(deleteObject);
            return Delete(deleteList);
        }


        public override int Delete(List<object> objectsToDeleteList) {
            int rowsAffected = 0;

            try {
                databaseManager.StartTransaction();
                foreach (SchoolObject school in objectsToDeleteList) {
                    String statement = statementBuilder.Delete("id=", school.Id);
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



        public override List<object> Execute() {
            throw new NotImplementedException();
        }


        public override List<object> GetAll() {
            String statement = statementBuilder.Select("id", "school_name", "address").Create();

            List<object> schoolsObjectsList = new List<object>();
            foreach (List<String> schoolRow in databaseManager.ExecuteQuery(statement))
                schoolsObjectsList.Add(new SchoolObject(schoolRow));

            return schoolsObjectsList;
        }



        public override List<object> GetByCriteria(params object[] criteriaParams) {
            String statement = statementBuilder.Select("id", "school_name", "address").Where(criteriaParams).Create();

            List<object> schoolsObjectsList = new List<object>();
            foreach (List<String> schoolRow in databaseManager.ExecuteQuery(statement, statementBuilder.WhereParamsDictionary))
                schoolsObjectsList.Add(new SchoolObject(schoolRow));

            return schoolsObjectsList;
        }



        public override object GetById(int id) {
            String statement = statementBuilder.Select("id", "school_name", "address").Where(true, "id=", id).Create();
            List<List<string>> resultList = databaseManager.ExecuteQuery(statement, statementBuilder.WhereParamsDictionary);

            return new SchoolObject(resultList[0]);
        }


    }//class
}
