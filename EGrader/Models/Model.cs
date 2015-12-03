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

        /// <summary>
        /// Apstraktna metoda za Delete.
        /// </summary>
        /// <param name="deleteObject"></param>
        /// <returns></returns>
        public abstract int Delete(object deleteObject);
        /// <summary>
        /// Apstraktna metoda za filtriranje upita. Vraća Datatable.
        /// </summary>
        /// <param name="criteriaParams"></param>
        /// <returns></returns>
        public abstract DataTable GetByCriteria(params object[] criteriaParams);
        /// <summary>
        /// Apstraktna metoda za filtriranje upita. Vraća odgovarajući POCO objekt.
        /// </summary>
        /// <param name="criteriaParams"></param>
        /// <returns></returns>
        public abstract List<object> GetObjectsByCriteria(params object[] criteriaParams);
        /// <summary>
        /// Apstraktna metoda za insert statement, na temelju proslijeđenog POCO objekta.
        /// </summary>
        /// <param name="insertObject"></param>
        /// <returns></returns>
        public abstract int Insert(object insertObject);
        /// <summary>
        /// Apstraktna metoda za update tablice na temelju proslijeđenog POCO objekta.
        /// </summary>
        /// <param name="updateObject"></param>
        /// <returns></returns>
        public abstract int Update(object updateObject);


    }//class
}
