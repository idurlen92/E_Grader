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

        private bool isTransactionExecuting = false;

        public NpgsqlConnection Connection { get { return connection; } }



        public static DatabaseManager GetInstance() {
            if (instance == null)
                instance = new DatabaseManager();
            return instance;
        }



        public void Connect() {
            if (connection == null)
                connection = new NpgsqlConnection(connectionString);
            if (connection.State != ConnectionState.Open)
                connection.Open();
        }



        public void Disconnect() {
            if (connection != null)
                connection.Close();            
        }



        public void StartTransaction() {
            Connect();
            transaction = connection.BeginTransaction();
            isTransactionExecuting = true;
        }



        public void CommitTransaction() {
            isTransactionExecuting = false;

            if (transaction != null) {
                transaction.Commit();
                transaction.Dispose();
            }
            else {
                Console.WriteLine("Transaction object is null");
            }

            Disconnect();
        }



        public void RollBackTransacion() {
            isTransactionExecuting = false;

            if (transaction != null) {
                transaction.Rollback();
                transaction.Dispose();
            }
            else {
                Console.WriteLine("Transaction object is null");
            }

            Disconnect();
        }



        // ####################### NON-SELECT STATEMENT #######################
        public int ExecuteStatement(String statement, Dictionary<String, String> paramsDictionary) {
            int rowsAffacted = 0;
            Connect();

            NpgsqlCommand command = new NpgsqlCommand(statement, connection);
            foreach (KeyValuePair<String, String> entry in paramsDictionary)
                command.Parameters.AddWithValue(entry.Key, entry.Value);

            rowsAffacted = command.ExecuteNonQuery();
            if (!isTransactionExecuting)
                Disconnect();

            return rowsAffacted;
        }



        // ####################### QUERYING  #######################



        /// <summary>
        /// Used for query with no binding parameters.
        /// </summary>
        /// <param name="commandString"></param>
        /// <returns>List of list of strings: every primitive type will be cast to String</returns>
        public DataTable ExecuteQuery(String commandString) {
            Dictionary<string, string> paramsDictionary = new Dictionary<string, string>();
            return ExecuteQuery(commandString, paramsDictionary);
        }



        /// <summary>
        /// Used for querying with binded parameters. 
        /// paramsDictionary: name/value
        /// </summary>
        /// <param name="commandString"></param>
        /// <param name="paramsDictionary"></param>
        /// <returns></returns>
        public DataTable ExecuteQuery(String commandString, Dictionary<String, String> paramsDictionary) {
            StartTransaction();
            
            NpgsqlCommand command = new NpgsqlCommand(commandString, connection);
            foreach (KeyValuePair<String, String> entry in paramsDictionary)
                command.Parameters.AddWithValue(entry.Key, entry.Value);

            DataTable dataTable = new DataTable();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
            adapter.Fill(dataTable);

            CommitTransaction();
            return dataTable ;
        }
        


    }//class
}
