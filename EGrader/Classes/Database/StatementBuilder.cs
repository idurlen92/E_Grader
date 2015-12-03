using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGrader.Classes.Database {
    public class StatementBuilder {

        public enum JoinType { Left, Right, Full };
        private readonly String[] keywordsArray = { " SELECT ", "(SELECT", " LIKE", " AND ", " OR ", " BETWEEN ", " IN(", " IN ", " NOT IN ",
            " NOT IN(", "(", ")", "=", "<", ">", "<>"};

        protected readonly String tableName;

        private List<String> columnsList;

        private Dictionary<String, String> whereParamsDictionary;
        private Dictionary<String, String> deleteParamsDictionary;
        private Dictionary<String, String> insertParamsDictionary;
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
            insertParamsDictionary = new Dictionary<string, string>();
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
        public Dictionary<String, String> DeleteParamsDictionary { get { return deleteParamsDictionary; } }
        public Dictionary<String, String> InsertParamsDictionary { get { return insertParamsDictionary; } }
        public Dictionary<String, String> UpdateParamsDictionary { get { return updateParamsDictionary; } }
        



        // ###############################  H E L P E R     M E T H O D S  ###############################

        /// <summary>
        /// Metoda za provjervanje dal je proslijeđeni parametar primitivan tip.
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private bool IsPrimitiveType(object param) {
            return (param is string || param is int || param is long || param is short);
        }


        /// <summary>
        /// Provjerava da li je parametar dio naredbe ili varijabla koju treba bindati.
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
                return true;
            }
            return true;
        }



        /// <summary>
        /// Provjera da li je proslijeđeni parametar zadnji parametar IN operatora.
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
        /// Pozivanje istoimene funkcije (preopterećenje) s parametrom <code>isUpdate</code> postavljenim na <code>false</code>
        /// </summary>
        /// <param name="paramsDictionary"></param>
        /// <param name="statement"></param>
        /// <param name="conditionParams"></param>
        private void ProcessParameters(ref Dictionary<String, String> paramsDictionary, ref StringBuilder statement, params object[] conditionParams) {
            ProcessParameters(false, ref paramsDictionary, ref statement, conditionParams);
        }



        /// <summary>
        /// Procesiranje parametara kod WHERE, SET naredbi.
        /// </summary>
        /// <returns></returns>
        private void ProcessParameters(bool isUpdate, ref Dictionary<String, String> paramsDictionary, ref StringBuilder statement, params object[] conditionParams) {
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

                    if(isUpdate &&  i < (conditionParams.Length - 1))
                        statement.Append(", ");
                    else if (!isInOperator || (isInOperator && IsLastParam(conditionParams[i + 1]))){
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
        /// <summary>
        /// Formatiranje SELECT naredbe sa svim stupcima tablice (*).
        /// </summary>
        /// <returns></returns>
        public StatementBuilder Select() {
            selectStatement.Append("SELECT * FROM " + tableName + " ");
            return this;
        }


        /// <summary>
        /// Formatiranje SELECT naredbe sa stupcima u parametrima.
        /// </summary>
        /// <param name="columnParams"></param>
        /// <returns></returns>
        public StatementBuilder Select(params String[] columnParams) {
            List<String> columnsList = new List<string>();
            foreach (String column in columnParams)
                columnsList.Add(column);
            return Select(columnsList);
        }



        /// <summary>
        /// Formatiranje SELECT naredbe sa stupcima u parametrima.
        /// </summary>
        /// <param name="columnParams"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Traženje aliasa trenutne tablice.
        /// </summary>
        /// <param name="aliasesList"></param>
        /// <returns></returns>
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



        /// <summary>
        /// Dodavalje aliasa trenutne tablice  u stupce SELECT naredbe.
        /// </summary>
        /// <param name="alias"></param>
        private void AddTableAlias(String alias) {
            int insertIndex = selectStatement.ToString().IndexOf("JOIN") - 1;
            selectStatement.Insert(insertIndex, ' ' + alias);
        }



        /// <summary>
        /// 3D matrica: [1] naziv tablice za spajanje, [2] vanjski ključ, [2] ključ u tablici za spajanje
        /// Join('users u', 't.user_id', 'u.id')
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
        /// Polje 3D matrica: [1] naziv tablice za spajanje, [2] vanjski ključ, [2] ključ u tablici za spajanje
        /// Join('users u', 't.user_id', 'u.id')
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
        /// WHERE klauzula SELECT naredbe.
        /// Ne koristiti za ostale naredbe(Delete, update)
        /// Prihvaća samo string i broj.
        /// </summary>
        /// <param name="variablesList"></param>
        /// <param name="stringParams"></param>
        /// <returns></returns>
        public StatementBuilder Where(params object[] conditionParams) {
            whereParamsDictionary.Clear();

            if(conditionParams.Length > 0)
                whereStatement.Append("WHERE ");
            ProcessParameters(ref whereParamsDictionary, ref whereStatement, conditionParams);

            return this;
        }



        // ---------- OTHER STATEMENTS ----------
        /// <summary>
        /// Formatiranje GROUP BY naredbe.
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public StatementBuilder GroupBy(String column) {
            groupByStatement.Append("GROUP BY" + column + " ");
            return this;
        }


        /// <summary>
        /// FOrmatiranje ORDER BY naredbe.
        /// </summary>
        /// <param name="columnParams"></param>
        /// <returns></returns>
        public StatementBuilder OrderBy(params String[] columnParams) {
            orderByStatement.Append("ORDER BY ");
            for (int i = 0; i < columnParams.Length; i++)
                orderByStatement.Append(columnParams[i] + (i < (columnParams.Length - 1) ? ", " : " "));
            return this;
        }


        /// <summary>
        /// Formatiranje LIMIT naredbe.
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        public StatementBuilder Limit(int limit) {
            return Limit(Convert.ToString(limit));
        }

        ///<summary>
        /// Formatiranje LIMIT naredbe.
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        public StatementBuilder Limit(String limit) {
            limitStatement.Append("LIMIT " + limit + " ");
            return this;
        }


        /// <summary>
        /// Formatiranje OFFSET naredbe.
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public StatementBuilder Offset(int offset) {
            return Limit(Convert.ToString(offset));
        }



        /// <summary>
        /// Formatiranje OFFSET naredbe.
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public StatementBuilder Offset(String offset) {
            limitStatement.Append("OFFSET " + offset + " ");
            return this;
        }



        // ------------------- EXECUTION -------------------
        /// <summary>
        /// Formatiranje cijelog Select statementa.
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


        /// <summary>
        /// Resetiranje field-ova.
        /// </summary>
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
        /// <summary>
        /// Formatiranje Insert naredbe. Prihvaća samo sringove i brojeve kao parametre
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public StatementBuilder Insert(params String[] columns) {
            insertStatement.Clear();
            insertStatement.Append("INSERT INTO " + tableName + "(");
            for(int i=0; i<columns.Length; i++)
                insertStatement.Append(columns[i] + ((i < columns.Length - 1) ? ", " : ") "));

            return this;
        }


        /// <summary>
        /// Formatiranje drugog dijela Insert naredbe. Prihvaća samo stringove i brojeve kao parametre.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public String Values(params object[] parameters) {
            insertParamsDictionary.Clear();
            insertStatement.Append("VALUES(");

            int counter = 1;
            for(int i=0; i<parameters.Length; i++) {
                if (!IsPrimitiveType(parameters[i]))
                    throw new StatementBuilderException("Parameter is not a primitive type!");

                insertParamsDictionary.Add(":v" + counter, Convert.ToString(parameters[i]));
                insertStatement.Append(":v" + counter + ((i < parameters.Length - 1) ? ", " : ") "));
                counter++;
            }

            Console.WriteLine(insertStatement.ToString());

            return insertStatement.ToString();
        }



        // ###############################  U P D A T E  ###############################
        /// <summary>
        /// Formatiranje Update naredbe.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public StatementBuilder Update(params object[] parameters) {
            updateParamsDictionary.Clear();
            updateStatement.Clear();
            updateStatement.Append("UPDATE " + tableName + " SET ");

            ProcessParameters(true, ref updateParamsDictionary, ref updateStatement, parameters);

            return this;
        }


        /// <summary>
        /// Formatiranje Where klauzule Update naredbe.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public String UWhere(params object[] parameters) {
            if (updateStatement.Length == 0)
                throw new StatementBuilderException("Update method must be called before!");

            updateStatement.Append(" WHERE ");
            ProcessParameters(ref updateParamsDictionary, ref updateStatement, parameters);
            Console.WriteLine(updateStatement.ToString());

            return updateStatement.ToString();
        }



        // ###############################  D E L E T E  ###############################
        /// <summary>
        /// Formatiranje Delete naredbe.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public String Delete(params object[] parameters) {
            deleteParamsDictionary.Clear();
            deleteStatement.Clear();
            deleteStatement.Append("DELETE FROM " + tableName + " WHERE ");

            ProcessParameters(ref deleteParamsDictionary, ref deleteStatement, parameters);
            Console.WriteLine(deleteStatement.ToString());
            
            return deleteStatement.ToString();
        }


    }//class
}
