using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGrader.Classes.Database {
    public class StatementBuilder {

        public enum JoinType { Left, Right, Full };

        protected readonly String tableName;

        List<String> columnsList;

        Dictionary<String, String> whereParamsDictionary;
        public Dictionary<String, String> insertValuesDictionary;

        String[] keywordsArray = { "SELECT", "AND", "OR", "BETWEEN", "IN", "NOT IN", "(", ")", "=", "<", ">", "<>"};

        bool isJoined = false;

        String tableAlias;

        StringBuilder selectStatement;
        StringBuilder whereStatement;
        StringBuilder groupByStatement;
        StringBuilder orderByStatement;
        StringBuilder limitStatement;
        StringBuilder offsetStatement;

        StringBuilder insertStatement;


        public StatementBuilder(String tableName) {
            this.tableName = tableName;

            columnsList = new List<String>();

            whereParamsDictionary = new Dictionary<String, String>();
            insertValuesDictionary = new Dictionary<string, string>();

            selectStatement = new StringBuilder();
            whereStatement = new StringBuilder();
            groupByStatement = new StringBuilder();
            orderByStatement = new StringBuilder();
            limitStatement = new StringBuilder();
            offsetStatement = new StringBuilder();

            insertStatement = new StringBuilder();
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
            
            int counter = 1;
            bool isInOperator = false;

            for(int i=0; i<conditionParams.Length; i++) {
                object param = conditionParams[i];
                try {
                    String stringParam = (String) Convert.ChangeType(param, TypeCode.String);

                    if (!IsVariable(param)) {
                        whereStatement.Append(" " + stringParam + " ");
                        isInOperator = (stringParam.ToLower().Contains(" in") && stringParam.EndsWith("("));
                    }
                    else{
                        whereParamsDictionary.Add(":v" + counter, stringParam);
                        whereStatement.Append(":v" + counter);
                        counter++;
                        if (!isInOperator)
                            whereStatement.Append(" ");
                        else if(IsLastParam(conditionParams[i + 1])) { 
                            isInOperator = false;
                            whereStatement.Append(" ");
                        }
                        else
                            whereStatement.Append(", ");
                    }
                }
                catch (Exception e) {
                    throw new StatementBuilderException(e.Message);
                }
            }

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
            insertStatement.Append("VALUES(");

            int counter = 1;
            for(int i=0; i<parameters.Length; i++) {
                if (!IsPrimitiveType(parameters[i]))
                    throw new StatementBuilderException("Only strings and numbers allowed!");


                insertValuesDictionary.Add(":v" + counter, Convert.ToString(parameters[i]));
                insertStatement.Append(":v" + counter + ((i < parameters.Length - 1) ? ", " : ") "));
                counter++;
            }

            return insertStatement.ToString();
        }



        // ###############################  U P D A T E  ###############################
        public StatementBuilder Update(params object[] parameters) {
            //UNFINISHED
            return this;
        }


        /// <summary>
        /// Where clause of Update statement. DO NOT use for Select statement!
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public String UWhere(params object[] parameters) {
            //UNFINISHED
            return "";
        }


        public String Delete(params object[] parameters) {
            //UNFINISHED
            return "";
        }




        public static void Main(String[] args) {
            StatementBuilder builder = new StatementBuilder("users");
            Console.WriteLine(builder.Insert("name", "surname", "birth_date").Values("ivan", "durlen", 2));
            foreach (KeyValuePair<String, String> element in builder.insertValuesDictionary)
                Console.WriteLine(element.Key + ": " + element.Value);
        }



    }//class
}
