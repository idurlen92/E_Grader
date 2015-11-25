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


        public override List<object> Execute() {
            Console.WriteLine(statementBuilder.Create());
            return new List<object>();
        }



        public override List<object> GetAll() {
            String statement = statementBuilder.Select("u.id", "u.name", "u.lastname", "u.username", "u.birth_date", "u.gender", "ut.user_type_name").Join(
                new String[]{ "user_types ut", "u.user_type_id", "ut.id" }).Create();

            //Console.WriteLine(statement);
            
            List<object> userObjectList = new List<object>();
            foreach (List<String> userRow in databaseManager.Select(statement))
                userObjectList.Add(new Objects.UserObject(userRow));

            return userObjectList;
        }



        public override List<object> GetByCriteria(params object[] criteriaParams) {
            String statement = statementBuilder.Select("u.id", "u.name", "u.lastname", "u.username", "u.birth_date", "u.gender", "ut.user_type_name").Join(
                new String[] { "user_types ut", "u.user_type_id", "ut.id" }).Where(true, criteriaParams).Create();

            Console.WriteLine(statement);

            List<object> userObjectList = new List<object>();
            foreach (List<String> userRow in databaseManager.Select(statement, statementBuilder.ParamsDictionary))
                userObjectList.Add(new Objects.UserObject(userRow));

            return userObjectList;
        }



        public override object GetById(int id) {
            String statement = statementBuilder.Select("u.id", "u.name", "u.lastname", "u.username", "u.birth_date", "u.gender", "ut.user_type_name").Join(
                new String[] { "user_types ut", "u.user_type_id", "ut.id" }).Where("u.id=", id).Create();

            List<List<string>> resultList = databaseManager.Select(statement, statementBuilder.ParamsDictionary);
            return new UserObject(resultList[0]);
        }





        public static void Main(String[] args) {
            try {
                UsersModel model = new UsersModel();
                foreach (UserObject user in model.GetByCriteria("u.id NOT IN(", 1, 3, ")"))
                    Console.WriteLine(user.GetUsername() + ": " + user.GetUserType().ToString());
            }
            catch(Exception e) {
                Console.WriteLine(e.Message + "\n" + e.Message);
            }
        }


    }
}
