using EGrader.Classes;
using EGrader.Models;
using EGrader.Models.Factory;
using EGrader.Models.Objects;
using EGrader.Views;
using EGrader.Views.Factory;
using EGrader.Views.Teacher;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace EGrader.Controllers.Teacher {
    class StudentGradingController : Controller {

        private List<String> monthsList = new List<string>() { "9", "10", "11", "12", "1", "2", "3", "4", "5", "6" };

        List<CourseObject> coursesList;
        List<CourseRubricObject> rubricsList;
        List<UserObject> studentsList;

        Dictionary<String, List<GradeObject>> gradesListDictionary;

        InsertGradeDialog dialog;
        
        CoursesModel coursesModel;
        CourseRubricsModel courseRubricsModel;
        UsersModel usersModel;

        GradesModel model;
        StudentGradingView view;


        public StudentGradingController(Model model, View view) {
            this.model = (GradesModel) model;
            this.view = (StudentGradingView) view;

            coursesModel = (CoursesModel) ModelFactory.NewModelInstance(ModelType.Courses);
            courseRubricsModel = (CourseRubricsModel) ModelFactory.NewModelInstance(ModelType.CourseRubrics);
            usersModel = (UsersModel) ModelFactory.NewModelInstance(ModelType.Users);

            coursesList = new List<CourseObject>();
            rubricsList = new List<CourseRubricObject>();
            studentsList = new List<UserObject>();

            gradesListDictionary = new Dictionary<string, List<GradeObject>>();

            this.view.comboBoxCourse.SelectionChanged += ActionComboChange;
            this.view.comboBoxStudent.SelectionChanged += ActionComboChange;
            this.view.buttonAddGrade.Click += ActionShowDialog;
            
            FetchAllData();
        }


        void FetchAllData() {
            try {
                view.comboBoxCourse.Items.Clear();
                view.comboBoxStudent.Items.Clear();

                foreach (CourseObject course in coursesModel.GetObjectsByCriteria()) {
                    coursesList.Add(course);
                    ComboBoxItem cBoxItem = new ComboBoxItem();
                    cBoxItem.Content = course.CourseName;
                    view.comboBoxCourse.Items.Add(cBoxItem);
                }
                foreach (UserObject student in usersModel.GetStudentsOfTeacher(CurrentUser.Id)) {
                    studentsList.Add(student);
                    ComboBoxItem cBoxItem = new ComboBoxItem();
                    cBoxItem.Content = student.Lastname + " " + student.Name;
                    view.comboBoxStudent.Items.Add(cBoxItem);
                }

                view.comboBoxCourse.SelectedIndex = 0;
                view.comboBoxStudent.SelectedIndex = 0;

                FetchGrades();
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message + ":\n" + ex.StackTrace);
            }
        }




        void FetchGrades() {
            CourseObject selectedCourse = coursesList[view.comboBoxCourse.SelectedIndex < 0 ? 0 : view.comboBoxCourse.SelectedIndex];
            UserObject selectedStudent = studentsList[view.comboBoxStudent.SelectedIndex < 0 ? 0 : view.comboBoxStudent.SelectedIndex];

            // ---------- Clearing lists ----------
            rubricsList.Clear();
            foreach (List<GradeObject> currentGradesList in gradesListDictionary.Values)
                currentGradesList.Clear();
            gradesListDictionary.Clear();

            // ---------- Fetching rubrics and grades ----------
            foreach (CourseRubricObject rubric in courseRubricsModel.GetObjectsByCriteria("course_id=", selectedCourse.Id)) {
                Console.WriteLine("Rubrika: " + rubric.RubricName);
                rubricsList.Add(rubric);
                List<GradeObject> currentGradesList = new List<GradeObject>();
                foreach (GradeObject grade in model.GetStudentGrades(selectedStudent.Id, rubric.Id))
                    currentGradesList.Add(grade);
                gradesListDictionary.Add(rubric.RubricName, currentGradesList);
            }

            CreateStringMatrix();
        }



        void CreateStringMatrix() {
            List<String> monthsListRoman = new List<string>() { "IX", "X", "XI", "XII", "I", "II", "III", "IV", "V", "VI" };

            String[,] matrix = new String[rubricsList.Count + 1, monthsList.Count + 1];

            for (int i = 0; i < monthsList.Count; i++)
                matrix[0, i + 1] = monthsListRoman[i];
            for (int i = 0; i < rubricsList.Count; i++)
                matrix[i + 1, 0] = rubricsList[i].RubricName;

            for(int i=0; i<rubricsList.Count; i++) {
                if (!gradesListDictionary.ContainsKey(rubricsList[i].RubricName))
                    continue;

                foreach(GradeObject gradeObj in gradesListDictionary[rubricsList[i].RubricName]) {
                    String extractedMonth = Utils.ParseMonth(gradeObj.Date);
                    int monthIndex = monthsList.IndexOf(extractedMonth) + 1;
                    matrix[i + 1, monthIndex] = Convert.ToString(gradeObj.Grade);
                }//if
            }

            for (int i = 1; i <= rubricsList.Count; i++) {
                for (int j = 1; j <= monthsList.Count; j++) {
                    if(matrix[i, j] == null)
                        matrix[i, j] = "-";
                }//for 2
            }//for 

            view.CreateGrid(matrix);

            foreach(Label label in view.contentGrid.Children) {
                int labelRow = (int) label.GetValue(Grid.RowProperty);
                int labelColumn = (int) label.GetValue(Grid.ColumnProperty);
                if (labelRow > 0 && labelColumn > 0 && label.Content.ToString().CompareTo("-") != 0) {
                    label.PreviewMouseDoubleClick += ActionDoubleClick;
                    GradeObject gradeObj = GetGradeByCell(labelRow, labelColumn);
                    label.ToolTip = (gradeObj == null) ? "Null object" : (gradeObj.Date + ((gradeObj.Note.Length > 0) ? (" " + gradeObj.Note) : ""));
                }
            }//foreach
        }


        void ActionComboChange(object sender, EventArgs e) {
            FetchGrades();
        }


        void ActionDoubleClick(object sender, EventArgs e) {
            if (MessageBox.Show("Jeste li sigurni da želite obrisati ocjenu?", "Upozorenje", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;

            Label label = sender as Label;
            GradeObject gradeObj = GetGradeByCell((int) label.GetValue(Grid.RowProperty), (int) label.GetValue(Grid.ColumnProperty));
            if (gradeObj == null) {
                Console.WriteLine("Grade is null");
                return;
            }

            if (model.Delete(gradeObj) < 1)
                MessageBox.Show("Greška u brisanju!");
            else
                FetchGrades();
        }



        void ActionShowDialog(object sender, EventArgs e) {
            dialog = (InsertGradeDialog) ViewFactory.NewDialogInstance(AppController.CurrentAppContext);
            for (int i = 1; i < 6; i++)
                dialog.comboBoxGrade.Items.Add(i);
            foreach (CourseRubricObject rubric in rubricsList)
                dialog.comboBoxRubric.Items.Add(rubric.RubricName);

            dialog.buttonClose.Click += ActionCloseDialog;
            dialog.buttonInsert.Click += ActionInsert;

            dialog.ShowDialog();
        }


        void ActionCloseDialog(object sender, EventArgs e) {
            String date = dialog.datePicker.SelectedDate.ToString();
            Console.WriteLine(date);
            dialog.Close();
        }


        GradeObject GetGradeByCell(int row, int col) {
            String rubricName = rubricsList[row - 1].RubricName;
            String month = monthsList[col - 1];

            if (!gradesListDictionary.ContainsKey(rubricName)) {
                Console.WriteLine("No such key in rubrics dictionary!");
                return null;
            }
            foreach(GradeObject gradeObj in gradesListDictionary[rubricName]) {
                if (Utils.ParseMonth(gradeObj.Date).Equals(month))
                    return gradeObj;
            }

            return null;
        }


        int GetRubricId(String rubricName) {
            foreach(CourseRubricObject rubric in rubricsList) {
                if (rubric.RubricName.CompareTo(rubricName) == 0)
                    return rubric.Id;
            }
            Console.WriteLine("Rubric not found!");
            return 0;
        }


        bool IsCellValid(String selectedDate, String rubricName) {
            if (!gradesListDictionary.ContainsKey(rubricName)) {
                Console.WriteLine("No dictionary key!!!");
                return false;
            }
            foreach(GradeObject gradeObj in gradesListDictionary[rubricName]) {
                if (Utils.ParseMonth(gradeObj.Date).Equals(Utils.ParseMonth(selectedDate)))
                    return false;
            }
            return true;
        }


        void ActionInsert(object sender, EventArgs e) {
            if(dialog.comboBoxGrade.SelectedIndex < 0 || dialog.comboBoxRubric.SelectedIndex < 0 || !dialog.datePicker.SelectedDate.HasValue) {
                MessageBox.Show("Unesite potrebna polja!");
                return;
            }
            else if(!IsCellValid(dialog.datePicker.SelectedDate.ToString(), Convert.ToString(dialog.comboBoxRubric.SelectedItem))) {
                MessageBox.Show("Ocjena za taj mjesec i rubriku je već unesena!");
                return;
            }

            GradeObject gradeObject = new GradeObject();
            gradeObject.SetStudentId(studentsList[view.comboBoxStudent.SelectedIndex].Id);
            gradeObject.SetTeacherId(CurrentUser.Id);
            gradeObject.SetRubricId(GetRubricId(Convert.ToString(dialog.comboBoxRubric.SelectedItem)));
            gradeObject.SetDate(dialog.datePicker.SelectedDate.ToString());
            gradeObject.SetGrade(Convert.ToInt32(dialog.comboBoxGrade.SelectedItem));
            if(dialog.textBoxNote.Text.Length > 0)
                gradeObject.SetNote(dialog.textBoxNote.Text);

            if (model.Insert(gradeObject) < 1)
                MessageBox.Show("Greška u unosu!");
            else
                FetchGrades();

            dialog.Close();
        }


    }//class
}
