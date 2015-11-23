using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGrader.Classes.Database {
    public class DatabaseManager {
        private static DatabaseManager instance = null;

        private const String connectionString = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=fnh57dsx;Database=e_grader;";

        public NpgsqlConnection Connection { get; private set; }



        private DatabaseManager() {
            Connect();
        }



        public static DatabaseManager GetInstance() {
            if (instance == null)
                instance = new DatabaseManager();
            instance.Connect();

            return instance;
        }



        public void Connect() {
            try {
                if (Connection == null)
                    Connection = new NpgsqlConnection(connectionString);
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
            }
            catch (Exception e) {
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



        /// <summary>
        /// Inserts one row in database, with string parameters defined respectivly as stated in the query.
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="parametersList"></param>
        /// <returns></returns>
        public int Insert(String queryString, params String[] parametersList) {
            //TODO: add checks!

            String subString = queryString.Substring(queryString.IndexOf("values", StringComparison.OrdinalIgnoreCase));
            subString = subString.Substring(subString.IndexOf('('));
            subString = subString.Substring(1, subString.IndexOf(')') - 1);

            String[] subStringParts = subString.Split(',');

            NpgsqlCommand command = new NpgsqlCommand(queryString, Connection);
            for (int i = 0; i < parametersList.Length; i++) {
                String paramName = subStringParts[i].Substring(subStringParts[i].IndexOf(':') + 1);
                command.Parameters.AddWithValue(paramName, parametersList[i]);
            }

            return command.ExecuteNonQuery();
        }



        public void StartTransaction() {
            //TODO
        }



        public void CommitTransaction() {
            //TODO
        }



        public List<List<String>> Select(String commandString) {
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

            return resultList;
        }


    }//class
}
