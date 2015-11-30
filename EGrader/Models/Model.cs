using EGrader.Classes.Database;
using System;
using System.Collections.Generic;
using System.Data;
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


        public abstract int Delete(object deleteObject);
        public abstract int Delete(List<object> objectsToDeleteList);
        public abstract DataTable GetByCriteria(params object[] criteriaParams);
        public abstract List<object> GetObjectsByCriteria(params object[] criteriaParams);
        //UNFINISHED
        public abstract int Insert(object insertObject);
        public abstract int Update(object updateObject);


    }//class
}
