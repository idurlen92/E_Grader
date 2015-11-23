using EGrader.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EGrader.Views.Menus.Factory {
    class MenuViewFactory {

        public static MenuView NewMenuInstance(UserType userType) {
            if (userType == UserType.Admin)
                return new AdminMenu();
            else if (userType == UserType.Student)
                return new StudentMenu();
            else
                return new TeacherMenu();
        }

    }
}
