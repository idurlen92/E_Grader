using EGrader.Classes.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGrader.Models {
    public abstract class Model {

        protected DatabaseManager databaseManager;
        protected StatementBuilder statementBuilder;

        private String tableName;


        public Model(String tableName) {
            this.tableName = tableName;

            databaseManager = DatabaseManager.GetInstance();
            statementBuilder = new StatementBuilder(tableName);
        }


        public abstract List<object> Execute();
        public abstract object GetAll();
        public abstract object GetById(int id);
        public abstract List<object> GetByCriteria(params String[] criteriaParams);


        public Model Select() {
            statementBuilder.Select();
            return this;
        }


        public Model Select(params String[] columnParams) {
            statementBuilder.Select(columnParams);
            return this;
        }


        public Model Join(String [,] joinParams) {
            statementBuilder.Join(joinParams);
            return this;
        }


        public Model Where(params object[] conditionParams) {
            statementBuilder.Where(conditionParams);
            return this;
        }


        public Model GroupBy(String column) {
            statementBuilder.GroupBy(column);
            return this;
        }


        public Model OrderBy(params String[] columnParams) {
            statementBuilder.OrderBy(columnParams);
            return this;
        }


        public Model Limit(int limit) {
            return Limit(Convert.ToString(limit));
        }


        public Model Limit(String limit) {
            statementBuilder.Limit(limit);
            return this;
        }


        public Model Offset(int offset) {
            return Offset(Convert.ToString(offset));
        }


        public Model Offset(String offset) {
            statementBuilder.Offset(offset);
            return this;
        }



    }//class
}
