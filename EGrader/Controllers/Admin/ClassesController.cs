using EGrader.Classes;
using EGrader.Models;
using EGrader.Models.Factory;
using EGrader.Models.Objects;
using EGrader.Views;
using EGrader.Views.Admin;
using EGrader.Views.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EGrader.Controllers.Admin {
    public class ClassesController : Controller{

        int selectedListItem = -1;

        List<ClassObject> classesList;
        List<ClassInSchoolObject> schoolClassesList;
        List<UserObject> teachersList;

        ClassDialog dialog;

        ClassesModel classesModel;
        UsersModel usersModel;

        ClassesInSchoolsModel model;
        ClassesView view;

        public ClassesController(Model model, View view) {
            this.model = (ClassesInSchoolsModel) model;
            this.view = (ClassesView) view;

            classesModel = (ClassesModel) ModelFactory.NewModelInstance(ModelType.Classes);
            usersModel = (UsersModel) ModelFactory.NewModelInstance(ModelType.Users);

            classesList = new List<ClassObject>();
            schoolClassesList = new List<ClassInSchoolObject>();
            teachersList = new List<UserObject>();

            this.view.CurrentListView.SelectionMode = System.Windows.Controls.SelectionMode.Single;
            this.view.buttonAdd.Click += ActionShowDialog;
            this.view.buttonDelete.Click += ActionDelete;
            this.view.buttonDelete.IsEnabled = false;

            LoadClassNames();
            LoadData();
        }


        /// <summary>
        /// Dohvaćanje razreda iz baze.
        /// </summary>
        void LoadClassNames() {
            try{
                foreach (ClassObject currentClass in classesModel.GetObjectsByCriteria())
                    classesList.Add(currentClass);
            }
            catch(Exception e) {
                Console.WriteLine(e.Message + ":\n" + e.StackTrace);
            }
        }


        /// <summary>
        /// Dohvaćanje svih potrebnih podataka i spremanje u listu.
        /// </summary>
        void LoadData() {
            try {
                teachersList.Clear();
                schoolClassesList.Clear();

                String subQuery = "SELECT cs.teacher_id FROM classes_in_schools cs";
                object[] criteria = new object[] { "u.works_in=", CurrentUser.WorksIn, " AND u.user_type_id=", 2, " AND u.id NOT IN(" + subQuery + ")"};
                foreach (UserObject teacher in usersModel.GetObjectsByCriteria(criteria))
                    teachersList.Add(teacher);

                List<object> classesArrayList = new List<object>();
                foreach (ClassInSchoolObject schoolClass in model.GetObjectsByCriteria("cs.school_id=", CurrentUser.WorksIn)) {
                    schoolClassesList.Add(schoolClass);
                    classesArrayList.Add( new String[]{ schoolClass.ClassName, schoolClass.SchoolName, schoolClass.TeacherName });
                }
                
                view.Update(ref classesArrayList);
                foreach (ListViewItem listItem in view.CurrentListView.Items) {
                    listItem.PreviewMouseLeftButtonUp += ActionItemClick;
                    listItem.MouseDoubleClick += ActionUpdateClass;
                }
            }
            catch(Exception e) {
                Console.WriteLine(e.Message + ":\n" + e.StackTrace);
            }
        }


        /// <summary>
        /// Handler klika na element liste - spremanje indeksa kliknutog elementa.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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


        /// <summary>
        /// Provjerava dal je razred već postojeć u trenutnoj školi.
        /// </summary>
        /// <param name="currentClass"></param>
        /// <returns></returns>
        private Boolean IsExistentClass (ClassObject currentClass) {
            foreach(ClassInSchoolObject schoolClass in schoolClassesList) {
                if (schoolClass.ClassId == currentClass.Id)
                    return true;
            }
            return false;
        }



        /// <summary>
        /// Kreiranje i prikaz dijaloškog okvira za unos razreda.
        /// </summary>
        void CreateDialog() {
            dialog = (ClassDialog) ViewFactory.NewDialogInstance(AppContext.ClassAdministration);

            foreach (UserObject teacher in teachersList)
                dialog.comboBoxTeacher.Items.Add(teacher.Lastname + " " + teacher.Name);
            foreach (ClassObject currentClass in classesList) {
                if(!IsExistentClass(currentClass))
                    dialog.comboBoxClassName.Items.Add(currentClass.ClassName);
            }

            dialog.buttonClose.Click += ActionCloseDialog;
            dialog.buttonInsert.Click += ActionInsert;

            dialog.ShowDialog();
        }


        /// <summary>
        /// Handler klika na gumb za dodavanje - prikaz dijaloškog okvira za dodavanje razreda.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ActionShowDialog(object sender, EventArgs e) {
            CreateDialog();
        }

        /// <summary>
        /// Akcija gumba za zatvaranje dijaloškog okvira.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ActionCloseDialog(object sender, EventArgs e) {
            dialog.Close();
        }


        /// <summary>
        /// Pretražianje liste razreda prema imenu razreda. Vraća id razreda.
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        int GetClassId(String className) {
            foreach(ClassObject currentClass in classesList) {
                if (currentClass.ClassName.Equals(className))
                    return currentClass.Id;
            }
            return 0;
        }


        /// <summary>
        /// Handler gumba za dodavanje razreda - Insert.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ActionInsert(object sender, EventArgs e) {
            if(dialog.comboBoxClassName.SelectedIndex < 0 || dialog.comboBoxTeacher.SelectedIndex < 0) {
                MessageBox.Show("Unesite sve potrebne podatke!");
                return;
            }

            ClassInSchoolObject schoolClass = new ClassInSchoolObject();
            schoolClass.SetSchoolId(CurrentUser.WorksIn);
            schoolClass.SetTeacherId(teachersList[dialog.comboBoxTeacher.SelectedIndex].Id);
            schoolClass.SetClassId(GetClassId(Convert.ToString(dialog.comboBoxClassName.SelectedItem)));

            if (model.Insert(schoolClass) < 1)
                MessageBox.Show("Greška u unosu podataka!");
            else
                LoadData();

            dialog.Close();
        }


        /// <summary>
        /// Handler gumba za brisanje razreda - delete.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ActionDelete(object sender, EventArgs e) {
            if(MessageBox.Show("Jeste li sigurni da želite obrisati razred?", "Upozorenje", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;

            if (model.Delete(schoolClassesList[selectedListItem]) < 1)
                MessageBox.Show("Ne možete obrisati razred u kojem ima učenika!");
            else
                LoadData();
        }



        void ActionUpdateClass(object sender, EventArgs e) {
            //CreateDialog();
            //ClassInSchoolObject schoolClass = schoolClassesList[selectedListItem];
            //TODO: 
        }

    }//class
}
