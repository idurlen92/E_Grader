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

namespace EGrader.Controllers.Admin {
    public class ClassesController : Controller{

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

            LoadClassNames();
            LoadData();
        }



        void LoadClassNames() {
            try{
                foreach (ClassObject currentClass in classesModel.GetObjectsByCriteria())
                    classesList.Add(currentClass);
            }
            catch(Exception e) {
                Console.WriteLine(e.Message + ":\n" + e.StackTrace);
            }
        }


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
            }
            catch(Exception e) {
                Console.WriteLine(e.Message + ":\n" + e.StackTrace);
            }
        }


        private Boolean IsExistentClass (ClassObject currentClass) {
            foreach(ClassInSchoolObject schoolClass in schoolClassesList) {
                if (schoolClass.ClassId == currentClass.Id)
                    return true;
            }
            return false;
        }


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


        void ActionShowDialog(object sender, EventArgs e) {
            CreateDialog();
        }


        void ActionCloseDialog(object sender, EventArgs e) {
            dialog.Close();
        }


        int GetClassId(String className) {
            foreach(ClassObject currentClass in classesList) {
                if (currentClass.ClassName.Equals(className))
                    return currentClass.Id;
            }
            return 0;
        }


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

    }//class
}
