using EGrader.Classes;
using EGrader.Views.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EGrader.Views {
    class MenuFactory {

        public static UserControl NewInstance() {
            if (CurrentUser.IsAdmin())
                return new AdminMenu();
            else if (CurrentUser.IsStudent())
                return new StudentMenu();
            else 
                return new TeacherMenu();
        }

    }
}
