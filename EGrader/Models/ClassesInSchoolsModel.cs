using EGrader.Classes.Database;
using EGrader.Models.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGrader.Models {
    public class ClassesInSchoolsModel : Model {

        private String[] tableColumns = { "cs.id", "cs.class_id", "cs.school_id", "cs.teacher_id", "s.school_name",
                                      "u.name || ' ' || u.lastname teacher", "c.class_name"};
        private String[,] joinColumns = { { "users u", "u.id", "cs.teacher_id" }, { "schools s", "s.id", "cs.school_id"}, 
                                        {"classes c", "c.id", "cs.class_id" } };



        public ClassesInSchoolsModel() : base("classes_in_schools") {  }



        public override int Delete(object deleteObject) {
            int rowsAffected = -1;

            try {
                ClassInSchoolObject schoolClass = deleteObject as ClassInSchoolObject;
                String statement = statementBuilder.Delete("id=", schoolClass.Id);
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
            String statement = statementBuilder.Select(tableColumns).Join(joinColumns).Where(criteriaParams).Create();
            return databaseManager.ExecuteQuery(statement, statementBuilder.WhereParamsDictionary);
        }



        public override List<object> GetObjectsByCriteria(params object[] criteriaParams) {
            List<object> classesList = new List<object>();
            DataTable dataTable = GetByCriteria(criteriaParams);
            foreach (DataRow row in dataTable.Rows)
                classesList.Add(new ClassInSchoolObject(dataTable.Columns, row));
            return classesList;
        }


        public override int Insert(object insertObject) {
            int rowsAffected = -1;

            try {
                ClassInSchoolObject schoolClass = insertObject as ClassInSchoolObject;
                String statement = statementBuilder.Insert("class_id", "school_id", "teacher_id").Values(
                                     schoolClass.ClassId, schoolClass.SchoolId, schoolClass.TeacherId);

                rowsAffected = databaseManager.ExecuteStatement(statement, statementBuilder.InsertParamsDictionary);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message + ":\n" + e.StackTrace);
            }
            return rowsAffected;
        }

        public override int Update(object updateObject) {
            throw new NotImplementedException();
        }
    }//class
}
