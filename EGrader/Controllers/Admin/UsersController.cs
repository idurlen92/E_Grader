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

        bool isEdit = false;
        int selectedListItem = -1;

        UserObject editedUser;

        List<ClassInSchoolObject> schoolClassesList;
        List<ClassObject> classesList;
        List<object> usersList;

        ClassesInSchoolsModel schoolClassesModel;
        ClassesModel classesModel;

        UserDialog dialog;
        
        UsersModel model;
        UsersView view;



        public UsersController(Model model, View view) {
            this.model = (UsersModel) model;
            this.view = (UsersView) view;

            classesList = new List<ClassObject>();
            schoolClassesList = new List<ClassInSchoolObject>();
            usersList = new List<object>();

            classesModel = (ClassesModel) ModelFactory.NewModelInstance(ModelType.Classes);
            schoolClassesModel = (ClassesInSchoolsModel) ModelFactory.NewModelInstance(ModelType.ClassesInSchools);

            this.view.buttonDelete.Click += ActionDelete;
            this.view.buttonAdd.Click += ActionShowDialog;

            schoolClassesModel = (ClassesInSchoolsModel) ModelFactory.NewModelInstance(ModelType.ClassesInSchools);

            LoadClasses();
            GetData();
        }


        String FindClassById(int id) {
            foreach(ClassObject currentClass in classesList) {
                if (currentClass.Id == id)
                    return currentClass.ClassName;
            }
            return "-";
        }


        String FindClassName(UserObject user) {
            if(user.UserType == UserType.Teacher) {
                foreach(ClassInSchoolObject schoolClass in schoolClassesList) {
                    if(schoolClass.TeacherId == user.Id)
                        return FindClassById(schoolClass.ClassId);
                }//foreach
            }
            else {
                foreach (ClassInSchoolObject schoolClass in schoolClassesList) {
                    if (schoolClass.Id == user.ClassId) 
                        return FindClassById(schoolClass.ClassId);
                }//foreach
            }

            return "--";
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
                else {
                    List<object> contentList = new List<object>();
                    foreach(UserObject user in usersList)
                        contentList.Add(new String[] { user.Lastname, user.Name, user.Username, FindClassName(user) });
                    view.Update(ref contentList);
                }
                    

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



        private void LoadClasses() {
            try {
                foreach (ClassObject currentClass in classesModel.GetObjectsByCriteria())
                    classesList.Add(currentClass);
                foreach (ClassInSchoolObject schoolClass in schoolClassesModel.GetObjectsByCriteria("cs.school_id=", CurrentUser.WorksIn)) 
                    schoolClassesList.Add(schoolClass);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message + ":\n" + e.StackTrace);
            }
        }



        void CreateDialog() {
            dialog = (UserDialog) ViewFactory.NewDialogInstance(AppContext.Teachers);
            dialog.Closed += ActionDisableEdit;
            dialog.buttonClose.Click += ActionCloseDialog;
            dialog.ResizeMode = ResizeMode.NoResize;
            if (AppContext.Students == AppController.CurrentAppContext) {
                dialog.CreateClassesList();
                foreach (ClassInSchoolObject schoolClass in schoolClassesList) 
                    dialog.CurrentComboBox.Items.Add(FindClassById(schoolClass.ClassId));
            }
        }



        void ActionShowEditDialog(object sender, EventArgs e) {
            CreateDialog();
            dialog.buttonInsert.Click += ActionInsertUpdate;

            editedUser = usersList[view.CurrentListView.Items.IndexOf(sender)] as UserObject;
            dialog.textBoxName.Text = editedUser.Name;
            dialog.textBoxLastname.Text = editedUser.Lastname;
            dialog.textBoxUsername.Text = editedUser.Username;

            if (dialog.CurrentComboBox != null) {
                for(int i= 0; i<schoolClassesList.Count; i++) {
                    if (schoolClassesList[i].Id == editedUser.ClassId) {
                        dialog.CurrentComboBox.SelectedIndex = i;
                        break;
                    }
                }
            }
            
            isEdit = true;
            dialog.ShowDialog();
        }


        void ActionShowDialog(object sender, EventArgs e) {
            CreateDialog();
            dialog.buttonInsert.Click += ActionInsertUpdate;
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



        void ActionInsertUpdate(object sender, RoutedEventArgs e) {
            if((!isEdit && (dialog.passwordBox.Password.Length == 0 || dialog.passwordBoxConfirm.Password.Length == 0)) 
                || dialog.textBoxLastname.Text.Length == 0  || dialog.textBoxName.Text.Length == 0 || dialog.textBoxUsername.Text.Length == 0
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
                userObject.SetId(editedUser.Id);
                userObject.SetUserTypeId(editedUser.UserTypeId);
                userObject.SetClassId((AppController.CurrentAppContext == AppContext.Students) ? editedUser.ClassId : -1);
                if (userObject.Password.Length == 0)
                    userObject.SetPassword(editedUser.Password);
            }
            
            try {
                if (!isEdit && model.Insert(userObject) < 1) 
                    MessageBox.Show("Greška u unosu!");
                else if (model.Update(userObject) < 0)
                    MessageBox.Show("Greška u ažuriranju!");
                GetData();
            }
            catch(Exception ex) {
                Console.WriteLine(ex.Message + ":\n"  + ex.StackTrace);
            }
            finally {
                dialog.Close();
            }
        }



    }//class
}
