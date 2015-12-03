using EGrader.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EGrader.Views.Menus {
    public interface MenuView {

        /// <summary>
        /// Apstraktna metoda togglanja menija.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Toggle(Object sender, RoutedEventArgs e);

    }
}
