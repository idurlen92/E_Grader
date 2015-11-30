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
                                      "u.name || ' ' || u.lastname teacher", "c.class_year || c.class_section class_name" };
        private String[,] joinColumns = { { "users u", "u.id", "cs.teacher_id" }, { "schools s", "s.id", "cs.school_id"}, 
                                        {"classes c", "c.id", "cs.class_id" } };



        public ClassesInSchoolsModel() : base("classes_in_schools") {  }



        public override int Delete(object deleteObject) {
            List<object> deleteList = new List<object>();
            deleteList.Add(deleteObject);
            return Delete(deleteList);
        }


        public override int Delete(List<object> objectsToDeleteList) {
            int rowsAffected = -1;

            try {
                databaseManager.StartTransaction();
                foreach (ClassInSchoolObject schoolClass in objectsToDeleteList) {
                    String statement = statementBuilder.Delete("id=", schoolClass.Id);
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



        public static void Main(String[] args) {
            try {
                ClassesInSchoolsModel model = new ClassesInSchoolsModel();
                foreach (ClassInSchoolObject cis in model.GetObjectsByCriteria())
                    Console.WriteLine(cis.SchoolName + ", " + cis.TeacherName);
            }
            catch(Exception e) {
                Console.WriteLine(e.Message + ":\n" + e.StackTrace);
            }
        }


    }//class
}
