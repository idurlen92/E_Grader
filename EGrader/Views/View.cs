using System.Collections.Generic;

namespace EGrader.Views {
    public interface View {

        void Update(ref List<object> objectsList);

    }
}
