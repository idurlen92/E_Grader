using EGrader.Classes;
using EGrader.Classes.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGrader.Models {
    class UsersModel : Model{

        // ---------- Constructor ----------
        public UsersModel() : base("users") {  }


        public override List<object> Execute() {
            Console.WriteLine(statementBuilder.Create());
            return new List<object>();
        }


        public override object GetAll() {
            String statement = statementBuilder.Select("u.name", "u.lastname", "u.username", "ut.user_type_name").Join(
                new String[]{ "user_types ut", "u.user_type_id", "ut.id" }).Create();

            Console.WriteLine(statement);

            try {
                databaseManager.Select(statement);
            }
            catch(Exception e) {
                Console.WriteLine(e.Message + ":\n" + e.StackTrace);
            }

            //List<List<String>> usersList = databaseManager.Select(statement);
            //return usersList;
            return null;
        }


        public override List<object> GetByCriteria(params string[] criteriaParams) {
            throw new NotImplementedException();
        }


        public override object GetById(int id) {
            return "Ivica";
        }


        public static void Main(String[] args) {
            UsersModel model = new UsersModel();

            model.GetAll();
        }


    }
}
