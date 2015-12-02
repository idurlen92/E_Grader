using System;
using System.Collections.Generic;
using System.Data;
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

namespace EGrader.Views.Student {
    /// <summary>
    /// Interaction logic for GradesView.xaml
    /// </summary>
    public partial class GradesView : UserControl, View {


        public GradesView() {
            InitializeComponent();
        }


        public void CreateGrid(String[,] matrix) {
            contentGrid.ColumnDefinitions.Clear();
            contentGrid.RowDefinitions.Clear();
            contentGrid.Children.Clear();

            int height = 50;
            while (height * (matrix.GetLength(0) + 1) > contentGrid.Height)
                height--;

            for (int i = 0; i < matrix.GetLength(0); i++) {
                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = new GridLength(height);
                contentGrid.RowDefinitions.Add(rowDef);

                for (int j = 0; j < matrix.GetLength(1); j++) {
                    ColumnDefinition columnDef = new ColumnDefinition();
                    columnDef.Width = new GridLength(j == 0 ? 150 : (contentGrid.Width / (matrix.GetLength(1) + 2)));

                    Label label = new Label();
                    if (i == 0 || j == 0)
                        label.FontWeight = FontWeights.Bold;

                    Grid.SetRow(label, i);
                    Grid.SetColumn(label, j);
                    label.Content = matrix[i, j];
                    contentGrid.Children.Add(label);
                }//for 2
            }
        }



        public void Update(ref List<object> objectsList) {
            throw new NotImplementedException();
        }
    }//class
}
