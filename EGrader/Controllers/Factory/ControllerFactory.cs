using EGrader.Controllers.Admin;
using EGrader.Controllers.Menu;
using EGrader.Controllers.Student;
using EGrader.Controllers.Teacher;
using EGrader.Models;
using EGrader.Views;
using EGrader.Views.Menus;

namespace EGrader.Controllers.Factory {
    class ControllerFactory {


        /// <summary>
        /// Kreira instance Controller klase ovisno o kontekstu.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="view"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Controller NewControllerInstance(Model model, View view, AppContext context) {
            if (context == AppContext.Students || context == AppContext.Teachers)
                return new UsersController(model, view);
            else if (context == AppContext.ClassAdministration)
                return new ClassesController(model, view);
            else if (context == AppContext.StudentGrading)
                return new StudentGradingController(model, view);
            else if (context == AppContext.Grades)
                return new GradesController(model, view);
            return null;
        }


        /// <summary>
        /// Kreira instancu MenuController-a.
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public static Controller NewMenuControllerInstance(MenuView menu) {
            return new MenuController(menu);
        }



    }//class
}
