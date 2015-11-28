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
        private NpgsqlConnection connection;
        private NpgsqlTransaction transaction;


        public NpgsqlConnection Connection { get { return connection; } }



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
                if (connection == null)
                    connection = new NpgsqlConnection(connectionString);
                if (connection.State != ConnectionState.Open)
                    connection.Open();
            }
            catch (Exception e) {
                Console.WriteLine(e.Message + ":\n" + e.StackTrace);
            }
        }



        public void Disconnect() {
            try {
                if (connection != null)
                    connection.Close();
            }
            catch (Exception e) {
                Console.WriteLine(e.Message + ":\n" + e.StackTrace);
            }
        }



        public void StartTransaction() {
            if(connection == null || connection.State != ConnectionState.Open) {
                Console.WriteLine("Connection error!");
                return;
            }

            transaction = connection.BeginTransaction();
        }



        public void CommitTransaction() {
            if (connection == null || connection.State != ConnectionState.Open) {
                Console.WriteLine("Connection error!");
                return;
            }

            if (transaction != null) {
                transaction.Commit();
                transaction.Dispose();
            }
            else
                Console.WriteLine("Transaction object is null");
        }



        public void RollBackTransacion() {
            if (connection == null || connection.State != ConnectionState.Open) {
                Console.WriteLine("Connection error!");
                return;
            }

            if (transaction != null) {
                transaction.Rollback();
                transaction.Dispose();
            }
            else
                Console.WriteLine("Transaction object is null");
        }



        // ####################### NON-SELECT STATEMENT #######################
        public int ExecuteStatement(String statement, Dictionary<String, String> paramsDictionary) {
            if (connection == null || connection.State != ConnectionState.Open) {
                Console.WriteLine("Connection error!");
                return 0;
            }

            NpgsqlCommand command = new NpgsqlCommand(statement, connection);
            foreach (KeyValuePair<String, String> entry in paramsDictionary)
                command.Parameters.AddWithValue(entry.Key, entry.Value);

            return command.ExecuteNonQuery();
        }



        // ####################### QUERYING  #######################
        private void CreateResultsList(ref List<List<String>> resultsList, ref NpgsqlDataReader dataReader) {
            while (dataReader.Read()) {
                List<String> rowColumnsList = new List<String>();
                for (int i = 0; i < dataReader.FieldCount; i++) {
                    if (dataReader.IsDBNull(i))
                        rowColumnsList.Add("-");
                    else
                        rowColumnsList.Add(Convert.ToString(dataReader.GetValue(i)));
                }
                resultsList.Add(rowColumnsList);
            }
        }




        /// <summary>
        /// Used for query with no binding parameters.
        /// </summary>
        /// <param name="commandString"></param>
        /// <returns>List of list of strings: every primitive type will be cast to String</returns>
        public List<List<String>> ExecuteQuery(String commandString) {
            List<List<String>> resultsList = new List<List<string>>();

            if (connection == null || connection.State != ConnectionState.Open) {
                Console.WriteLine("Connection error!");
                return resultsList;
            }

            NpgsqlCommand command = new NpgsqlCommand(commandString, connection);
            NpgsqlDataReader dataReader = command.ExecuteReader();
            CreateResultsList(ref resultsList, ref dataReader);
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
        public List<List<String>> ExecuteQuery(String commandString, Dictionary<String, String> paramsDictionary) {
            List<List<String>> resultsList = new List<List<String>>();

            if (connection == null || connection.State != ConnectionState.Open) {
                Console.WriteLine("Connection error!");
                return resultsList;
            }

            NpgsqlCommand command = new NpgsqlCommand(commandString, connection);
            foreach (KeyValuePair<String, String> entry in paramsDictionary)
                command.Parameters.AddWithValue(entry.Key, entry.Value);

            NpgsqlDataReader dataReader = command.ExecuteReader();
            CreateResultsList(ref resultsList, ref dataReader);
            dataReader.Close();
            //command.Dispose();

            return resultsList; ;
        }



    }//class
}
