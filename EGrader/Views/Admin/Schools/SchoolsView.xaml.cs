using EGrader.Controllers;
using EGrader.Controllers.Admin;
using EGrader.Models;
using EGrader.Models.Objects;
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
            LoadSchools();

            buttonAddSchool.Click += controller.DoAction;
        }


        private void LoadSchools() {
            List<SchoolObject> schoolsList = new List<SchoolObject>();
            foreach (SchoolObject school in model.GetAll()) {
                schoolsList.Add(school);
                Console.WriteLine(school.SchoolName);
            }

            if (schoolsList.Count == 0) {
                Label infoLabel = new Label();
                infoLabel.Width = 200;
                infoLabel.Content = "Škole nisu pronađene";
                contentPanel.Children.Insert(0, infoLabel);
            }
            else
                CreateListView(schoolsList);
        }



        private void CreateListView(List<SchoolObject> schoolsList) {
            ListView listView = new ListView();

            foreach (SchoolObject school in schoolsList) {
                Grid grid = new Grid();
                for(int i=0; i<2; i++) 
                    grid.ColumnDefinitions.Add(new ColumnDefinition());

                for(int i=0; i<2; i++) {
                    Label label = new Label();
                    label.Width = 200;
                    label.Content = (i == 0) ? school.SchoolName : school.Address;
                    Grid.SetColumn(label, i);
                    grid.Children.Add(label);
                }

                ListViewItem item = new ListViewItem();
                item.Content = grid;
                listView.Items.Add(item);
            }

            ScrollViewer scrollView = new ScrollViewer();
            scrollView.Margin = new Thickness(25, 25, 25, 0);
            scrollView.Content = listView;
            scrollView.Height = (575 - buttonAddSchool.Width - buttonAddSchool.Margin.Top - scrollView.Margin.Top);

            contentPanel.Children.Insert(0, scrollView);
        }



    }//class
}
