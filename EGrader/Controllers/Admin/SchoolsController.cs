using EGrader.Models;
using EGrader.Views.Admin.Schools;
using EGrader.Views.Factory;
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
            if(sender is Button) {
                String buttonName = ((Button) sender).Name;
                if (buttonName.ToLower().Contains("add"))
                    ActionShowForm();
                else if (buttonName.ToLower().Contains("confirm"))
                    ActionConfirm();
            }
            else {
                Console.WriteLine("Not button??!");
            }
        }



        private void ActionConfirm() {
            //TODO:
        }


        private void ActionShowForm() {
            InsertSchoolDialog dialog = (InsertSchoolDialog) ViewFactory.NewDialogInstance(this, model, DialogType.InsertSchool);
            dialog.ShowDialog();
        }



    }//class
}
