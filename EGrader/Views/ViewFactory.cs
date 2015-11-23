using EGrader.Controllers;
using EGrader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EGrader.Views {
    class ViewFactory {
        

        public static UserControl NewInstance(Controller controller, Model model, AppContext appContext) {
            if (appContext == AppContext.Start || appContext == AppContext.Profile)
                return new ProfileView(controller, model);
            //UNFINISHED
            return null;
        }


    }//class
}
