using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EGrader.Controllers {
    public interface Controller {

        void AttachView(UserControl view);
        void DoAction(object sender, RoutedEventArgs e);
        
    }
}
