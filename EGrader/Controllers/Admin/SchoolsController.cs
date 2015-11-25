using EGrader.Models;
using EGrader.Views.Admin.Schools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EGrader.Controllers.Admin {
    public class SchoolsController : Controller {

        SchoolsModel model;
        SchoolsView view;


        public SchoolsController(Model model) {
            this.model = (SchoolsModel) model;
        }


        public void AttachView(UserControl view) {
            this.view = (SchoolsView) view;
        }



        public void DoAction(object sender, RoutedEventArgs e) {
            throw new NotImplementedException();
        }



    }//class
}
