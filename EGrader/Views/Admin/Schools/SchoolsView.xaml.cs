using EGrader.Controllers;
using EGrader.Controllers.Admin;
using EGrader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EGrader.Views.Admin.Schools {
    /// <summary>
    /// Interaction logic for SchoolsView.xaml
    /// </summary>
    public partial class SchoolsView : UserControl {

        SchoolsController controller;
        SchoolsModel model;


        public SchoolsView(Controller controller, Model model) {
            this.controller = (SchoolsController) controller;
            this.model = (SchoolsModel) model;

            InitializeComponent();
        }


    }//class
}
