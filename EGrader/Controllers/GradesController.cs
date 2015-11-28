using EGrader.Models;
using EGrader.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EGrader.Controllers {
    class GradesController : Controller {

        //GradesModel model;
        GradesView view;


        public GradesController(Model model) {
            //TODO: this.model = (GradesModel) model;
        }

        public void AttachView(UserControl view) {
            this.view = (GradesView) view;
        }


        public void DoAction(object sender, RoutedEventArgs e) {
           //TODO:
        }


    }//class
}
