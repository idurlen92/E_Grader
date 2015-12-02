using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGrader.Models.Objects {
    public class ClassObject {

        int id;
        String className;

        public ClassObject() { }

        public ClassObject(DataColumnCollection columns, DataRow row) {
            this.id = columns.Contains("id") ? Convert.ToInt32(row["id"]) : -1;
            this.className = columns.Contains("class_name") ? Convert.ToString(row["class_name"]) : "-";
        }


        public int Id { get { return id; } }
        public String ClassName { get { return className; } }


        public void SetId(int id) { this.id = id; }
        public void SetClassName(String className) { this.className = className; }


    }
}
