using EGrader.Models.Objects;
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

namespace EGrader.Views.Admin {
    /// <summary>
    /// Interaction logic for ClassesView.xaml
    /// </summary>
    public partial class ClassesView : UserControl, View {

        ListView currentListView;
        public ListView CurrentListView { get { return currentListView; } }


        public ClassesView() {
            InitializeComponent();

            buttonDelete.IsEnabled = false;

            currentListView = new ListView();
            currentListView.BorderThickness = new Thickness(1);
            currentListView.SelectionMode = SelectionMode.Single;
            currentListView.Margin = new Thickness(10);
            scrollView.Content = currentListView;
        }


        public void CreateList(List<object> classesList) {
            foreach (String[] classArrayString in classesList) {
                Grid grid = new Grid();
                grid.ShowGridLines = true;
                grid.RowDefinitions.Add(new RowDefinition());
                grid.Margin = new Thickness(30, 10, 30, 0);

                for (int i=0; i < classArrayString.Length; i++) {
                    ColumnDefinition columnDef = new ColumnDefinition();
                    columnDef.Width = new GridLength(150);
                    grid.ColumnDefinitions.Add(columnDef);

                    Label label = new Label();
                    label.Content = classArrayString[i];
                    Grid.SetColumn(label, i);
                    Grid.SetRow(label, 0);

                    grid.Children.Add(label);
                }

                ListViewItem listItem = new ListViewItem();
                listItem.Content = grid;
                currentListView.Items.Add(listItem);
            }
        }



        public void Update(ref List<object> objectsList) {
            currentListView.Items.Clear();
            if (objectsList.Count == 0)
                MessageBox.Show("Nema razreda!");
            else
                CreateList(objectsList);
        }

        
    }//class
}
