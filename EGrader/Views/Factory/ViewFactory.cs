using EGrader.Controllers;
using EGrader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EGrader.Views.Factory {
    class ViewFactory {

        public static UserControl NewViewInstance(Controller controller, Model model, AppContext context) {
            //UNFINISHED
            return null;
        }


        public static UserControl NewStartViewInstance() {
            return new StartView();
        }

    }
}
