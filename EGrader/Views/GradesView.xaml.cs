using EGrader.Controllers;
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

namespace EGrader.Views {
    /// <summary>
    /// Interaction logic for GradesView.xaml
    /// </summary>
    public partial class GradesView : UserControl {

        GradesController controller;
        //GradesModel model;

        public GradesView(Controller controller, Model model) {
            this.controller = (GradesController) controller;
            //TODO:this.model = (GradesModel) model;
            InitializeComponent();

            CreateGrid();
            contentGrid.MouseLeftButtonUp += Func;
        }


        void Func(object sender, RoutedEventArgs e) {
            Label label = sender as Label;
            int row = (int) label.GetValue(Grid.RowProperty);
            int col = (int) label.GetValue(Grid.ColumnProperty);
            MessageBox.Show(row + ", " + col);
        }

        private void CreateGrid() {
            for(int i=0; i<5; i++) {
                contentGrid.ColumnDefinitions.Add(new ColumnDefinition());
                for(int j=0; j<5; j++) {
                    contentGrid.RowDefinitions.Add(new RowDefinition());
                    Label label = new Label();
                    if (i == 0 || j == 0)
                        label.FontWeight = FontWeights.ExtraBold;
                    label.Content = Convert.ToString(i + 1) + Convert.ToString(j + 1);
                    Grid.SetRow(label, i);
                    Grid.SetColumn(label, j);
                    contentGrid.Children.Add(label);
                }
            }
        }


    }//class
}
