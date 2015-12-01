using EGrader.Models.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGrader.Models {
    public class CourseRubricsModel : Model {

        String[] tableColumns = { "cr.id", "cr.rubric_name", "cr.course_id", "c.courseName" };
        String[] joinParams = { "courses c", "c.id", "cr.course_id" };


        public CourseRubricsModel() : base("course_rubrics") { }

        public override int Delete(object deleteObject) {
            throw new NotImplementedException();
        }


        public override DataTable GetByCriteria(params object[] criteriaParams) {
            String statement = statementBuilder.Select(tableColumns).Join(joinParams).Where(criteriaParams).Create();
            return databaseManager.ExecuteQuery(statement, statementBuilder.WhereParamsDictionary);
        }


        public override List<object> GetObjectsByCriteria(params object[] criteriaParams) {
            List<object> courseRubricsList = new List<object>();
            DataTable dataTable = GetByCriteria(criteriaParams);
            foreach (DataRow row in dataTable.Rows)
                courseRubricsList.Add(new CourseRubricObject(dataTable.Columns, row));
            return courseRubricsList;
        }


        public override int Insert(object insertObject) {
            throw new NotImplementedException();
        }


        public override int Update(object updateObject) {
            throw new NotImplementedException();
        }


    }//class
}
