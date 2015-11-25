using EGrader.Controllers;
using EGrader.Models;
using EGrader.Views.Admin.Schools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EGrader.Views.Factory {

    public enum DialogType { InsertSchool };

    public class ViewFactory {

        public static UserControl NewViewInstance(Controller controller, Model model, AppContext context) {
            //TODO:
            if (context == AppContext.Start)
                return new StartView();
            else if (context == AppContext.Profile)
                return new ProfileView(controller, model);
            else if (context == AppContext.Schools)
                return new SchoolsView(controller, model);
            return null;
        }


        public static Window NewDialogInstance(Controller controller, Model model, DialogType type) {
            //TODO:
            if (type == DialogType.InsertSchool)
                return new InsertSchoolDialog(controller, model);
            return null;
        }

    }
}
