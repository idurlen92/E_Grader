using EGrader.Controllers.Menu;
using EGrader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGrader.Controllers.Factory {
    class ControllerFactory {


        public static Controller NewControllerInstance(Model model, AppContext context) {
            //UNFINISHED
            return null;
        }


        public static Controller NewMenuControllerInstance() {
            return new MenuController();
        }

    }
}
