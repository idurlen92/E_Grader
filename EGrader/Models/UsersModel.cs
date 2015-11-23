using EGrader.Classes;
using EGrader.Classes.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGrader.Models {
    class UsersModel {

        private DatabaseManager databaseManager;

        private string tableName = "users";

        public UsersModel(DatabaseManager databaseManager) {
            this.databaseManager = databaseManager;
        }


        public bool userExists(String username) {
            List<List<String>> existentUser = databaseManager.Select("SELECT id FROM users WHERE username = '" + username +"' ");
            return existentUser.Capacity > 0;
        }

        public bool userExists(String username, String password) {
            String query = "SELECT id FROM users WHERE username = '" + username + "' AND password = '" + password + "' ";
            List<List<String>> existentUser = databaseManager.Select(query);
            return existentUser.Capacity > 0;
        }


    }
}
