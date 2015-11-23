using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGrader.Classes.Database {
    public class StatementBuilderException : Exception{

        public StatementBuilderException() : base("SQLStatementBuilderException") { }
        public StatementBuilderException(String message) : base(message) { }
    }
}
