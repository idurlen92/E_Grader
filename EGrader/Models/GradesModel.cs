using System;
using System.Collections.Generic;
using System.Data;

namespace EGrader.Models {
    class GradesModel : Model {

        public GradesModel() : base("grades") { }



        public override int Delete(List<object> objectsToDeleteList) {
            throw new NotImplementedException();
        }

        public override int Delete(object deleteObject) {
            throw new NotImplementedException();
        }

        public override DataTable GetByCriteria(params object[] criteriaParams) {
            throw new NotImplementedException();
        }

        public override List<object> GetObjectsByCriteria(params object[] criteriaParams) {
            throw new NotImplementedException();
        }

        public override int Insert(object insertObject) {
            throw new NotImplementedException();
        }

        public override int Update(object updateObject) {
            throw new NotImplementedException();
        }
    }
}
