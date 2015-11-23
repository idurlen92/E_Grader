using EGrader.Controllers.Login;
using EGrader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGrader.Controllers {
    class ControllerFactory {


        public static Controller NewInstance(Model model, AppContext context) {
            if (context == AppContext.Login)
                return new LoginController(model);
            return null;
        }

    }
}
