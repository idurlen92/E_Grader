using EGrader.Models.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGrader.Models {
    public class SchoolsModel : Model {

        public SchoolsModel() : base("schools") { }


        public override List<object> Execute() {
            throw new NotImplementedException();
        }



        public override List<object> GetAll() {
            String statement = statementBuilder.Select("id", "school_name", "address").Create();

            List<object> schoolsObjectsList = new List<object>();
            foreach (List<String> schoolRow in databaseManager.Select(statement))
                schoolsObjectsList.Add(new SchoolObject(schoolRow));

            return schoolsObjectsList;
        }



        public override List<object> GetByCriteria(params object[] criteriaParams) {
            String statement = statementBuilder.Select("id", "school_name", "address").Where(criteriaParams).Create();

            List<object> schoolsObjectsList = new List<object>();
            foreach (List<String> schoolRow in databaseManager.Select(statement, statementBuilder.WhereParamsDictionary))
                schoolsObjectsList.Add(new SchoolObject(schoolRow));

            return schoolsObjectsList;
        }



        public override object GetById(int id) {
            String statement = statementBuilder.Select("id", "school_name", "address").Where(true, "id=", id).Create();
            List<List<string>> resultList = databaseManager.Select(statement, statementBuilder.WhereParamsDictionary);

            return new SchoolObject(resultList[0]);
        }



        public static void Main(String[] args) {
            //TODO: test
            SchoolsModel model = new SchoolsModel();
        }


    }//class
}
