using Npgsql;
using System;
using System.Collections;
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
        /// <param name="valuesDictionary"></param>
        /// <returns></returns>
        public int Insert(String queryString, Dictionary<String, String> valuesDictionary) {
            //TODO: add checks!

            String subString = queryString.Substring(queryString.IndexOf("values", StringComparison.OrdinalIgnoreCase));
            subString = subString.Substring(subString.IndexOf('('));
            subString = subString.Substring(1, subString.IndexOf(')') - 1);

            String[] subStringParts = subString.Split(',');

            NpgsqlCommand command = new NpgsqlCommand(queryString, Connection);
            foreach (KeyValuePair<String, String> entry in valuesDictionary)
                command.Parameters.AddWithValue(entry.Key, entry.Value);

            return command.ExecuteNonQuery();
        }



        public void StartTransaction() {
            //TODO
        }



        public void CommitTransaction() {
            //TODO
        }


        // --------------- Querying  ---------------
        private List<List<String>> CreateResultsList(NpgsqlDataReader dataReader) {
            List<List<String>> resultList = new List<List<String>>();

            while (dataReader.Read()) {
                List<String> rowColumnsList = new List<String>();
                for (int i = 0; i < dataReader.FieldCount; i++) {
                    if (dataReader.IsDBNull(i))
                        rowColumnsList.Add("-");
                    else
                        rowColumnsList.Add(Convert.ToString(dataReader.GetValue(i)));
                }
                resultList.Add(rowColumnsList);
            }

            return resultList;
        }




        /// <summary>
        /// Used for query with no binding parameters.
        /// </summary>
        /// <param name="commandString"></param>
        /// <returns>List of list of strings: every primitive type will be cast to String</returns>
        public List<List<String>> Select(String commandString) {
            NpgsqlCommand command = new NpgsqlCommand(commandString, Connection);
            NpgsqlDataReader dataReader = command.ExecuteReader();
            List<List<String>> resultsList = CreateResultsList(dataReader);
            dataReader.Close();
            //command.Dispose();

            return resultsList;
        }



        /// <summary>
        /// Used for querying with binded parameters. 
        /// paramsDictionary: name/value
        /// </summary>
        /// <param name="commandString"></param>
        /// <param name="paramsDictionary"></param>
        /// <returns></returns>
        public List<List<String>> Select(String commandString, Dictionary<String, String> paramsDictionary) {
            List<List<String>> resultList = new List<List<String>>();

            NpgsqlCommand command = new NpgsqlCommand(commandString, Connection);
            foreach (KeyValuePair<String, String> entry in paramsDictionary)
                command.Parameters.AddWithValue(entry.Key, entry.Value);

            NpgsqlDataReader dataReader = command.ExecuteReader();
            List<List<String>> resultsList = CreateResultsList(dataReader);
            dataReader.Close();
            //command.Dispose();

            return resultsList; ;
        }



    }//class
}
