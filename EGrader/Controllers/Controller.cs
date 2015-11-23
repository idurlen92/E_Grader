using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EGrader.Controllers {
    public interface Controller {

        void DoAction(object sender, RoutedEventArgs e);

    }
}
