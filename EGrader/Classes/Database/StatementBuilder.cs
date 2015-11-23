using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGrader.Classes.Database {
    class StatementBuilder {

        public enum JoinType { Left, Right, Full };

        private readonly String tableName;
        private DatabaseManager dbManager;

        List<String> selectColumnsList;
        List<String> whereVaraiables;

        String[] keywordsArray = { "AND", "OR", "IN", "NOT IN", "(", ")", "=" };

        bool isJoined = false;

        StringBuilder selectStatement;
        StringBuilder whereStatement;
        StringBuilder groupByStatement;
        StringBuilder orderByStatement;
        StringBuilder limitStatement;
        StringBuilder offsetStatement;


        public StatementBuilder(String tableName) {
            this.tableName = tableName;

            dbManager = DatabaseManager.GetInstance();

            selectColumnsList = new List<String>();
            whereVaraiables = new List<String>();

            selectStatement = new StringBuilder();
            whereStatement = new StringBuilder();
            groupByStatement = new StringBuilder();
            orderByStatement = new StringBuilder();
            limitStatement = new StringBuilder();
            offsetStatement = new StringBuilder();
        }



        // ---------- SELECT STATEMENT ----------
        public StatementBuilder Select() {
            selectStatement.Append("SELECT * FROM " + tableName + " ");
            return this;
        }



        public StatementBuilder Select(params String[] columnParams) {
            selectStatement.Append("SELECT ");

            for (int i = 0; i < columnParams.Length; i++) {
                selectColumnsList.Add(columnParams[i]);
                selectStatement.Append(columnParams[i] + (i < (columnParams.Length - 1) ? ", " : " "));
            }

            selectStatement.Append("FROM " + tableName + " ");
            return this;
        }


        // ---------- JOIN STATEMENT ----------


        private String GetTableAlias(List<String> aliasesList) {
            foreach (String aliasedColumn in selectColumnsList) {
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
        /// <para>Array of 3D arrays where:
        /// [0] table to join name
        /// [1] foreign key 
        /// [2] foreign table id
        /// Example: Join('users u', 't.user_id', 'u.id')
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

            AddTableAlias(GetTableAlias(aliasesList));
            isJoined = true;

            return this;
        }



        // ---------- WHERE STATEMENT ----------
        private Boolean IsVariable(object arg) {
            if (arg is byte || arg is short || arg is int || arg is long)
                return true;
            else if (arg is string) {
                String stringArg = (String) Convert.ChangeType(arg, TypeCode.String);
                foreach (String keyword in keywordsArray) {
                    if (stringArg.Contains(keyword))
                        return false;
                }
                return true;
            }

            return false;
        }



        /// <summary>
        /// TODO:
        /// </summary>
        /// <param name="variablesList"></param>
        /// <param name="stringParams"></param>
        /// <returns></returns>
        public StatementBuilder Where(params object[] conditionParams) {
            whereStatement.Append("WHERE ");

            int counter = 1;
            foreach (object param in conditionParams) {
                try {
                    String stringParam = (String) Convert.ChangeType(param, TypeCode.String);
                    if (!IsVariable(param))
                        whereStatement.Append(" " + stringParam + " ");
                    else {
                        whereVaraiables.Add(stringParam);
                        whereStatement.Append(":v" + (counter++) + " ");
                    }
                }
                catch (Exception e) {
                    throw e;
                }
            }

            Console.WriteLine("Statement =>\t" + whereStatement.ToString());

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
        public void Exec() {
            StringBuilder fullStatementBuilder = new StringBuilder();
            fullStatementBuilder.Append(selectStatement);
            fullStatementBuilder.Append(whereStatement);
            fullStatementBuilder.Append(groupByStatement);
            fullStatementBuilder.Append(orderByStatement);
            fullStatementBuilder.Append(limitStatement);
            fullStatementBuilder.Append(offsetStatement);

            // ---------- TODO: replace with db exec ----------
            Console.WriteLine(fullStatementBuilder.ToString());

            // ----- cleaning up statement -----
            resetFields();
        }




        private void resetFields() {
            selectColumnsList.Clear();

            selectStatement.Clear();
            whereStatement.Clear();
            groupByStatement.Clear();
            orderByStatement.Clear();
            limitStatement.Clear();
            offsetStatement.Clear();
        }


    }
}
