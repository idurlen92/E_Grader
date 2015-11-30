using EGrader.Classes;
using EGrader.Models;
using EGrader.Models.Factory;
using EGrader.Models.Objects;
using EGrader.Views;
using EGrader.Views.Admin;
using EGrader.Views.Factory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace EGrader.Controllers.Admin {
    public class UsersController : Controller{

        int selectedListItem = -1;

        bool isEdit = false;

        List<object> usersList;
        List<ClassInSchoolObject> schoolClassesList;

        UserDialog dialog;
        ClassesInSchoolsModel schoolClassesModel;
        UsersModel model;
        ListableView view;



        public UsersController(Model model, View view) {
            this.model = (UsersModel) model;
            this.view = (ListableView) view;

            usersList = new List<object>();
            schoolClassesList = new List<ClassInSchoolObject>();
            
            this.view.buttonDelete.Click += ActionDelete;
            this.view.buttonAdd.Click += ActionShowDialog;

            schoolClassesModel = (ClassesInSchoolsModel) ModelFactory.NewModelInstance(ModelType.ClassesInSchools);

            LoadClasses();
            GetData();
        }



        private void GetData() {
            try {
                usersList.Clear();

                if(AppController.CurrentAppContext == AppContext.Teachers)
                    usersList = model.GetObjectsByCriteria("u.user_type_id = ", 2, " AND u.works_in = ", CurrentUser.WorksIn);
                else
                    usersList = model.GetStudents(CurrentUser.WorksIn);

                if (usersList.Count == 0)
                    MessageBox.Show("Nema korisnika!");
                else
                    view.Update(ref usersList);

                foreach (ListViewItem listItem in view.CurrentListView.Items) {
                    listItem.PreviewMouseLeftButtonUp += ActionItemClick;
                    listItem.PreviewMouseDoubleClick += ActionShowEditDialog;
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message + ":\n" + e.StackTrace);
                MessageBox.Show("Greška u dohvaćanju podataka!");
            }
        }



        private List<String> LoadClasses() {
            List<String> classesList = new List<String>();

            try {
                foreach (ClassInSchoolObject schoolClass in schoolClassesModel.GetObjectsByCriteria()) {
                    schoolClassesList.Add(schoolClass);
                    classesList.Add(schoolClass.ClassName);
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message + ":\n" + e.StackTrace);
            }

            return classesList;
        }



        void CreateDialog() {
            dialog = (UserDialog) ViewFactory.NewDialogInstance(AppContext.Teachers);
            dialog.Closed += ActionDisableEdit;
            dialog.buttonClose.Click += ActionCloseDialog;
            dialog.ResizeMode = ResizeMode.NoResize;
            if(AppContext.Students == AppController.CurrentAppContext)
                dialog.CreateClassesList(LoadClasses());
        }



        void ActionShowEditDialog(object sender, EventArgs e) {
            CreateDialog();
            //TODO: event handler

            UserObject selectedUser = usersList[view.CurrentListView.Items.IndexOf(sender)] as UserObject;
            dialog.textBoxName.Text = selectedUser.Name;
            dialog.textBoxLastname.Text = selectedUser.Lastname;
            dialog.textBoxUsername.Text = selectedUser.Username;

            isEdit = true;
            dialog.ShowDialog();
        }


        void ActionShowDialog(object sender, EventArgs e) {
            CreateDialog();
            dialog.buttonInsert.Click += ActionInsert;
            dialog.ShowDialog();
        }


        void ActionDisableEdit(object sender, EventArgs e) {
            isEdit = false;
        }


        void ActionCloseDialog(object sender, EventArgs e) {
            isEdit = false;
            dialog.Close();
        }


        void ActionItemClick(object sender, RoutedEventArgs e) {
            ListViewItem listItem = (ListViewItem) sender;

            int itemIndex = view.CurrentListView.Items.IndexOf(listItem);
            if (selectedListItem == itemIndex) {
                listItem.IsSelected = false;
                selectedListItem = -1;
            }
            else
                selectedListItem = itemIndex;
            view.buttonDelete.IsEnabled = (selectedListItem > -1);
        }



        void ActionDelete(object sender, EventArgs e) {
            if (MessageBox.Show("Jeste li sigurni?", "Brisanje", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;

            object userToDelete = usersList[selectedListItem];
            Console.WriteLine("Deleting: " + ((UserObject) usersList[selectedListItem]).Username);

            if (model.Delete(userToDelete) > -1) {
                MessageBox.Show("Uspješno obrisano!");
                selectedListItem = -1;
                GetData();
            }
            else
                MessageBox.Show("Greška u brisanju!");
        }



        private UserObject FillUserData() {
            UserObject userObject = new UserObject();
            userObject.SetName(dialog.textBoxName.Text);
            userObject.SetLastname(dialog.textBoxLastname.Text);
            userObject.SetUserTypeId((AppController.CurrentAppContext == AppContext.Teachers) ? 2 : 3);
            userObject.SetUsername(dialog.textBoxUsername.Text);
            userObject.SetPassword(dialog.passwordBox.Password);
            userObject.SetWorksIn((AppController.CurrentAppContext == AppContext.Teachers) ? CurrentUser.WorksIn : -1);

            if (AppController.CurrentAppContext == AppContext.Students && dialog.CurrentComboBox != null) {
                int classId = schoolClassesList[dialog.CurrentComboBox.SelectedIndex].Id;
                userObject.SetClassId(classId);
            }

            return userObject;
        }



        void ActionInsert(object sender, RoutedEventArgs e) {
            if(dialog.textBoxLastname.Text.Length == 0 || dialog.textBoxName.Text.Length == 0 || dialog.passwordBox.Password.Length == 0 
            || dialog.passwordBoxConfirm.Password.Length == 0 || dialog.textBoxUsername.Text.Length == 0
            || (AppController.CurrentAppContext == AppContext.Students && dialog.CurrentComboBox.SelectedIndex < 0)) {
                MessageBox.Show("Ispunite sva polja!", "Obavijest");
                return;
            }
            else if(dialog.passwordBox.Password.CompareTo(dialog.passwordBoxConfirm.Password) != 0) {
                MessageBox.Show("Lozinke se ne podudaraju!");
                return;
            }


            UserObject userObject = FillUserData();
            if (isEdit) {

            }
            
            try {
                if (isEdit && model.Insert(userObject) < 1) 
                    MessageBox.Show("Greška u unosu!");
                else if (model.Update(userObject) < 1)
                    MessageBox.Show("Greška u ažuriranju!");
                GetData();
            }
            catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            finally {
                dialog.Close();
            }
        }



    }//class
}
