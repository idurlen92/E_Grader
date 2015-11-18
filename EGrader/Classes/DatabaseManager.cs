using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGrader.Classes {
    class DatabaseManager {

        private static DatabaseManager instance = null;

        private String connectionString = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=fnh57dsx;Database=e_grader;";

        public NpgsqlConnection Connection { get; private set; }

        private DatabaseManager() { }


        public static DatabaseManager GetInstance() {
            if (instance == null)
                instance = new DatabaseManager();
            return instance;
        }


        public void Connect(){
            try {
                if(Connection == null)
                    Connection = new NpgsqlConnection(connectionString);
                Connection.Open();
            }
            catch(Exception e) {
                throw e;
            }
        }


        public void Disconnect() {
            try {
                if (Connection != null)
                    Connection.Close();
            }
            catch (Exception e) {
                throw e;
            }
        }


        public List<List<String>> Select(String commandString) {
            Connect();

            NpgsqlCommand command = new NpgsqlCommand(commandString, Connection);
            NpgsqlDataReader dataReader = command.ExecuteReader();

            List<List<String>> resultList = new List<List<String>>();
            while (dataReader.Read()) {
                List<String> rowColumnsList = new List<String>();
                for (int i = 0; i < dataReader.FieldCount; i++) {
                    if (dataReader.IsDBNull(i)) 
                        rowColumnsList.Add("-");
                    else 
                        rowColumnsList.Add(dataReader.GetValue(i).ToString());
                }
                resultList.Add(rowColumnsList);
            }

            Disconnect();
            return resultList;
        }




    }
}
