using EGrader.Classes;
using EGrader.Models;
using EGrader.Models.Objects;
using EGrader.Views;
using EGrader.Views.Admin;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace EGrader.Controllers.Admin {
    public class TeachersController : Controller{

        List<int> selectedListItems;
        List<object> usersList;

        ListableView view;
        UsersModel model;


        public TeachersController(Model model, View view) {
            this.model = (UsersModel) model;
            this.view = (ListableView) view;

            usersList = new List<object>();
            selectedListItems = new List<int>();

            this.view.buttonDelete.Click += ActionDelete;
            this.view.buttonAdd.Click += ActionAdd;
            GetData();
        }


        private void GetData() {
            usersList.Clear();

            foreach (UserObject user in model.GetByCriteria("u.user_type_id = ", 2, " AND u.works_in = ", CurrentUser.WorksIn))
                usersList.Add(user);
            if (usersList.Count > 0) {
                view.Update(ref usersList);
                foreach (ListViewItem item in this.view.currentListView.Items)
                    item.PreviewMouseLeftButtonUp += ActionListViewClick;
            }
        }



        void ActionAdd(object sender, EventArgs e) {
            //TODO:
        }



        void ActionDelete(object sender, EventArgs e) {
            if (MessageBox.Show("Jeste li sigurni?", "Brisanje", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;
            
            List<object> objectsToDeleteList = new List<object>();
            foreach (int index in selectedListItems) {
                objectsToDeleteList.Add(usersList[index]);
                Console.WriteLine(((UserObject) usersList[index]).Username);
            }
            return;

            if (model.Delete(objectsToDeleteList) > 0) {
                MessageBox.Show("Uspješno obrisano!");
                selectedListItems.Clear();
                GetData();
            }
            else
                MessageBox.Show("Greška u brisanju!");
        }



        void ActionListViewClick(object sender, RoutedEventArgs e) {
            ListViewItem listItem = (ListViewItem) sender;

            int itemIndex = view.currentListView.Items.IndexOf(listItem);
            if (selectedListItems.Contains(itemIndex)) {
                listItem.IsSelected = false;
                selectedListItems.Remove(itemIndex);
            }
            else
                selectedListItems.Add(itemIndex);

            view.buttonDelete.IsEnabled = (selectedListItems.Count > 0);
        }



    }//class
}
