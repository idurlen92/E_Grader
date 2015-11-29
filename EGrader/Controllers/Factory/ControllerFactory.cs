using EGrader.Controllers.Admin;
using EGrader.Controllers.Menu;
using EGrader.Models;
using EGrader.Views;
using EGrader.Views.Menus;

namespace EGrader.Controllers.Factory {
    class ControllerFactory {


        public static Controller NewControllerInstance(Model model, View view, AppContext context) {
            if (context == AppContext.Students)
                return new StudentsController(model, view);
            else if (context == AppContext.Teachers)
                return new TeachersController(model, view);
            else if (context == AppContext.Grades)
                return new GradesController(model, view);
            return null;
        }


        public static Controller NewMenuControllerInstance(MenuView menu) {
            return new MenuController(menu);
        }



    }//class
}
