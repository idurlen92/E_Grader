using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGrader.Classes.Database {
    public class StatementBuilder {

        public enum JoinType { Left, Right, Full };
        private readonly String[] keywordsArray = { "SELECT", "LIKE", "AND", "OR", "BETWEEN", "IN", "NOT IN",
            "(", ")", "=", "<", ">", "<>", "%"};

        protected readonly String tableName;

        private List<String> columnsList;

        private Dictionary<String, String> whereParamsDictionary;
        private Dictionary<String, String> deleteParamsDictionary;
        private Dictionary<String, String> insertValuesDictionary;
        private Dictionary<String, String> updateParamsDictionary;


        bool isJoined = false;

        private String tableAlias;

        private StringBuilder selectStatement;
        private StringBuilder whereStatement;
        private StringBuilder groupByStatement;
        private StringBuilder orderByStatement;
        private StringBuilder limitStatement;
        private StringBuilder offsetStatement;

        private StringBuilder deleteStatement;
        private StringBuilder insertStatement;
        private StringBuilder updateStatement;



        // ---------- Constructor ----------
        public StatementBuilder(string tableName) {
            this.tableName = tableName;

            columnsList = new List<string>();

            whereParamsDictionary = new Dictionary<string, string>();
            deleteParamsDictionary = new Dictionary<string, string>();
            insertValuesDictionary = new Dictionary<string, string>();
            updateParamsDictionary = new Dictionary<string, string>();

            selectStatement = new StringBuilder();
            whereStatement = new StringBuilder();
            groupByStatement = new StringBuilder();
            orderByStatement = new StringBuilder();
            limitStatement = new StringBuilder();
            offsetStatement = new StringBuilder();

            deleteStatement = new StringBuilder();
            insertStatement = new StringBuilder();
            updateStatement = new StringBuilder();
        }



        public Dictionary<String, String> WhereParamsDictionary { get { return whereParamsDictionary; } }



        // ###############################  H E L P E R     M E T H O D S  ###############################

        private bool IsPrimitiveType(object param) {
            return (param is string || param is int || param is long || param is short);
        }


        /// <summary>
        /// Check if given parameter is a part of a statement or a variable that needs to be bound.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Boolean IsVariable(object arg) {
            if (arg is string) {
                String stringArg = (String) Convert.ChangeType(arg, TypeCode.String);
                foreach (String keyword in keywordsArray) {
                    if (stringArg.ToUpper().Contains(keyword))
                        return false;
                }
            }
            return true;
        }



        /// <summary>
        /// Check if given parameter is last parameter in case of IN operators..
        /// </summary>
        /// <returns></returns>
        private Boolean IsLastParam(object param) {
            if (param is string) {
                String stringArg = (String) Convert.ChangeType(param, TypeCode.String);
                return stringArg.StartsWith(")");
            }
            return false;
        }



        /// <summary>
        /// Use to proccess passed parameters in Where clause of Select, Delete, or Update statement or Set clause of Update statement.
        /// </summary>
        /// <returns></returns>
        private void ProcessParameters(ref Dictionary<String, String> paramsDictionary, ref StringBuilder statement, params object[] conditionParams) {
            int counter = (paramsDictionary.Count + 1);
            bool isInOperator = false;
            bool isLikeOperator = false;//TODO: (maybe)

            for (int i = 0; i < conditionParams.Length; i++) {
                if (!IsPrimitiveType(conditionParams[i]))
                    throw new StatementBuilderException("Parameter is not a primitive type!");

                String stringParam = (String) Convert.ChangeType(conditionParams[i], TypeCode.String);

                if (!IsVariable(conditionParams[i])) {
                    statement.Append(" " + stringParam + " ");
                    isInOperator = (stringParam.ToLower().Contains(" in") && stringParam.EndsWith("("));
                }
                else {
                    paramsDictionary.Add(":v" + counter, stringParam);
                    statement.Append(":v" + counter);
                    counter++;

                    if (!isInOperator)
                        statement.Append(" ");
                    else if (IsLastParam(conditionParams[i + 1])) {
                        isInOperator = false;
                        statement.Append(" ");
                    }
                    else
                        statement.Append(", ");
                }
            }// for
        }


        // ###############################  I N S E R T  ###############################

        // ---------- SELECT STATEMENT ----------

        public StatementBuilder Select() {
            selectStatement.Append("SELECT * FROM " + tableName + " ");
            return this;
        }




        public StatementBuilder Select(params String[] columnParams) {
            List<String> columnsList = new List<string>();
            foreach (String column in columnParams)
                columnsList.Add(column);

            return Select(columnsList);
        }


        public StatementBuilder Select(List<String> columnsList) {
            this.columnsList.Clear();
            selectStatement.Append("SELECT ");

            for (int i = 0; i < columnsList.Count; i++) {
                this.columnsList.Add(columnsList[i]);
                selectStatement.Append(columnsList[i] + (i < (columnsList.Count - 1) ? ", " : " "));
            }

            selectStatement.Append("FROM " + tableName + " ");
            return this;
        }


        // ---------- JOIN STATEMENT ----------
        private String FindTableAlias(List<String> aliasesList) {
            foreach (String aliasedColumn in columnsList) {
                String[] parts = aliasedColumn.Split('.');
                if (parts.Length != 2)
                    throw new StatementBuilderException("Select statement should contain aliases when listing columns");
                if (!aliasesList.Contains(parts[0]))
                    return parts[0];
            }
            throw new StatementBuilderException("Alias not found in Select statement");
        }



        private void AddTableAlias(String alias) {
            int insertIndex = selectStatement.ToString().IndexOf("JOIN") - 1;
            selectStatement.Insert(insertIndex, ' ' + alias);
        }



        /// <summary>
        /// 3D array where:
        /// [1] foreign key 
        /// [2] foreign table id
        /// Example: Join('users u', 't.user_id', 'u.id')
        /// </summary>
        /// <param name="joinParams"></param>
        /// <returns></returns>
        public StatementBuilder Join(String[] joinParams) {
            if (joinParams.Length != 3)
                throw new StatementBuilderException("Array must be 3 of lenght");

            String[,] stringArray = new String[1, 3];
            stringArray[0, 0] = joinParams[0];
            stringArray[0, 1] = joinParams[1];
            stringArray[0, 2] = joinParams[2];

            return Join(stringArray);
        }



        /// <summary>
        /// <para>Array of 3D arrays where:
        /// [0] table to join name
        
        /// </summary>
        /// <param name="joinParams"></param>
        /// <returns></returns>
        public StatementBuilder Join(String[,] joinParams) {
            if (joinParams.GetLength(1) != 3)
                throw new StatementBuilderException("Each array must be 3 of lenght");

            List<String> aliasesList = new List<String>();

            for (int i = 0; i < joinParams.GetLength(0); i++) {
                String[] parts = joinParams[i, 0].Split(' ');
                if (parts.Length != 2)
                    throw new StatementBuilderException("Join statement doesn't contain aliases");
                aliasesList.Add(parts[1]);
                selectStatement.Append("JOIN " + joinParams[i, 0] + " ON " + joinParams[i, 1] + " = " + joinParams[i, 2] + " ");
            }

            tableAlias = FindTableAlias(aliasesList);
            AddTableAlias(tableAlias);

            isJoined = true;

            return this;
        }



        // ---------- WHERE STATEMENT ----------

        /// <summary>
        /// Accepting strings or numbers!
        /// </summary>
        /// <param name="variablesList"></param>
        /// <param name="stringParams"></param>
        /// <returns></returns>
        public StatementBuilder Where(params object[] conditionParams) {
            return Where(false, conditionParams);
        } 



        /// <summary>
        /// Where clause of Select statement.
        /// Do not use in other statements! (Delete, update, ...)
        /// Accepting strings or numbers!
        /// </summary>
        /// <param name="variablesList"></param>
        /// <param name="stringParams"></param>
        /// <returns></returns>
        public StatementBuilder Where(Boolean isCheckAlias, params object[] conditionParams) {
            whereParamsDictionary.Clear();
            whereStatement.Append("WHERE ");
            ProcessParameters(ref whereParamsDictionary, ref whereStatement, conditionParams);

            return this;
        }



        // ---------- OTHER STATEMENTS ----------
        public StatementBuilder GroupBy(String column) {
            groupByStatement.Append("GROUP BY" + column + " ");
            return this;
        }


        public StatementBuilder OrderBy(params String[] columnParams) {
            orderByStatement.Append("ORDER BY ");
            for (int i = 0; i < columnParams.Length; i++)
                orderByStatement.Append(columnParams[i] + (i < (columnParams.Length - 1) ? ", " : " "));
            return this;
        }


        public StatementBuilder Limit(int limit) {
            return Limit(Convert.ToString(limit));
        }



        public StatementBuilder Limit(String limit) {
            limitStatement.Append("LIMIT " + limit + " ");
            return this;
        }



        public StatementBuilder Offset(int offset) {
            return Limit(Convert.ToString(offset));
        }



        public StatementBuilder Offset(String offset) {
            limitStatement.Append("OFFSET " + offset + " ");
            return this;
        }



        // ------------------- EXECUTION -------------------
        /// <summary>
        /// Creates SQL Select statement.
        /// </summary>
        /// <returns></returns>
        public String Create() {
            StringBuilder fullStatementBuilder = new StringBuilder();

            fullStatementBuilder.Append(selectStatement);
            fullStatementBuilder.Append(whereStatement);
            fullStatementBuilder.Append(groupByStatement);
            fullStatementBuilder.Append(orderByStatement);
            fullStatementBuilder.Append(limitStatement);
            fullStatementBuilder.Append(offsetStatement);

            // ----- cleaning up statement -----
            resetFields();
            Console.WriteLine(fullStatementBuilder.ToString());

            return fullStatementBuilder.ToString();
        }


        private void resetFields() {
            isJoined = false;
            tableAlias = "";

            selectStatement.Clear();
            whereStatement.Clear();
            groupByStatement.Clear();
            orderByStatement.Clear();
            limitStatement.Clear();
            offsetStatement.Clear();
        }


        // ###############################  I N S E R T  ###############################
        public StatementBuilder Insert(params String[] columns) {
            insertStatement.Clear();
            insertStatement.Append("INSERT INTO " + tableName + "(");
            for(int i=0; i<columns.Length; i++)
                insertStatement.Append(columns[i] + ((i < columns.Length - 1) ? ", " : ") "));

            return this;
        }


        public String Values(params object[] parameters) {
            insertValuesDictionary.Clear();
            insertStatement.Clear();
            insertStatement.Append("VALUES(");

            int counter = 1;
            for(int i=0; i<parameters.Length; i++) {
                if (!IsPrimitiveType(parameters[i]))
                    throw new StatementBuilderException("Parameter is not a primitive type!");

                insertValuesDictionary.Add(":v" + counter, Convert.ToString(parameters[i]));
                insertStatement.Append(":v" + counter + ((i < parameters.Length - 1) ? ", " : ") "));
                counter++;
            }

            return insertStatement.ToString();
        }



        // ###############################  U P D A T E  ###############################
        public StatementBuilder Update(params object[] parameters) {
            updateParamsDictionary.Clear();
            updateStatement.Clear();
            updateStatement.Append("UPDATE " + tableName + " SET ");

            ProcessParameters(ref updateParamsDictionary, ref updateStatement, parameters);

            return this;
        }


        /// <summary>
        /// Where clause of Update statement. DO NOT use for Select statement!
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public String UWhere(params object[] parameters) {
            if (updateStatement.Length == 0)
                throw new StatementBuilderException("Update method must be called before!");

            updateStatement.Append(" WHERE ");
            ProcessParameters(ref updateParamsDictionary, ref updateStatement, parameters);

            return updateStatement.ToString();
        }


        public String Delete(params object[] parameters) {
            deleteParamsDictionary.Clear();
            deleteStatement.Clear();
            deleteStatement.Append("DELETE FROM " + tableName + " WHERE ");

            ProcessParameters(ref deleteParamsDictionary, ref deleteStatement, parameters);
            
            return deleteStatement.ToString();
        }




        public static void Main(String[] args) {
            StatementBuilder builder = new StatementBuilder("users");
            Console.WriteLine(builder.Delete("id IN(", 1, 2, 3, ")"));
        }



    }//class
}
