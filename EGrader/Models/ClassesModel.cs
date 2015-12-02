using EGrader.Models.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGrader.Models {
    public class ClassesModel : Model {


        public ClassesModel() : base("classes") { }


        public override int Delete(object deleteObject) {
            throw new NotImplementedException();
        }

        public override DataTable GetByCriteria(params object[] criteriaParams) {
            String statement = statementBuilder.Select("*").Where(criteriaParams).OrderBy("class_name").Create();
            return databaseManager.ExecuteQuery(statement, statementBuilder.WhereParamsDictionary);
        }

        public override List<object> GetObjectsByCriteria(params object[] criteriaParams) {
            List<object> classesList = new List<object>();
            DataTable dataTable = GetByCriteria(criteriaParams);
            foreach (DataRow row in dataTable.Rows)
                classesList.Add(new ClassObject(dataTable.Columns, row));
            return classesList;
        }

        public override int Insert(object insertObject) {
            throw new NotImplementedException();
        }

        public override int Update(object updateObject) {
            throw new NotImplementedException();
        }
    }
}
