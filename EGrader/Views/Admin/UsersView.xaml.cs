using EGrader.Models.Objects;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System;
using System.Data;
using EGrader.Classes;

namespace EGrader.Views.Admin {
    /// <summary>
    /// Interaction logic for ListableView.xaml
    /// </summary>
    public partial class UsersView : UserControl, View {

        ListView currentListView;
        public ListView CurrentListView { get { return currentListView; } }


        public UsersView() {
            InitializeComponent();

            buttonDelete.IsEnabled = false;

            currentListView = new ListView();
            currentListView.BorderThickness = new Thickness(1);
            currentListView.SelectionMode = SelectionMode.Single;
            currentListView.Margin = new Thickness(10);
            scrollView.Content = currentListView;
        }


        private void CreateList(List<object> objectsList) {
            foreach(String[] userArray in objectsList) {
                Grid grid = new Grid();
                grid.ShowGridLines = true;
                grid.RowDefinitions.Add(new RowDefinition());
                grid.Margin = new Thickness(30, 10, 30, 0);
                
                for (int i = 0; i < userArray.Length; i++) {
                    ColumnDefinition columnDef = new ColumnDefinition();
                    columnDef.Width = new GridLength(150);
                    grid.ColumnDefinitions.Add(columnDef);

                    Label label = new Label();
                    label.Content = userArray[i];
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
                MessageBox.Show("Nema korisnika!");
            else
                CreateList(objectsList);
        }


    }//class
}
