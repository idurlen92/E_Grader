using EGrader.Classes.Database;
using EGrader.Models.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGrader.Models {
    public class CoursesModel : Model {

        private String[] tableColumns = { "id", "course_name" };


        public CoursesModel() : base("courses") { }


        public override int Delete(object deleteObject) {
            int rowsAffected = -1;

            try {
                CourseObject course = deleteObject as CourseObject;
                String statement = statementBuilder.Delete("id=", course.Id);
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
            String statement = statementBuilder.Select(tableColumns).Where(criteriaParams).OrderBy("2 ASC").Create();
            return databaseManager.ExecuteQuery(statement, statementBuilder.WhereParamsDictionary);
        }


        public override List<object> GetObjectsByCriteria(params object[] criteriaParams) {
            List<object> coursesList = new List<object>();
            DataTable dataTable = GetByCriteria(criteriaParams);
            foreach (DataRow row in dataTable.Rows)
                coursesList.Add(new CourseObject(dataTable.Columns, row));
            return coursesList;
        }



        public override int Insert(object insertObject) {
            int rowsAffected = -1;
            CourseObject course = insertObject as CourseObject;

            String statement = statementBuilder.Insert("id", "course_name").Values(course.Id, course.CourseName);
            rowsAffected = databaseManager.ExecuteStatement(statement, statementBuilder.InsertParamsDictionary);

            return rowsAffected;
        }



        public override int Update(object updateObject) {
            //TODO:
            throw new NotImplementedException();
        }
    }
}
