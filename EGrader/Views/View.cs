using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGrader.Views {
    public interface View {

        void Update(ref List<object> objectsList);

    }
}
