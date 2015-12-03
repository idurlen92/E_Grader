using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace EGrader.Views.Student {
    /// <summary>
    /// Interaction logic for GradesView.xaml
    /// </summary>
    public partial class GradesView : UserControl, View {


        public GradesView() {
            InitializeComponent();
        }


        /// <summary>
        /// Kreiranje "tablice" ocjena na temelju proslijeđene string matrice.
        /// </summary>
        /// <param name="matrix"></param>
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
                    contentGrid.ColumnDefinitions.Add(columnDef);

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
