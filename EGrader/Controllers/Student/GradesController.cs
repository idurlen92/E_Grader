using EGrader.Controllers.Teacher;
using EGrader.Models;
using EGrader.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using EGrader.Models.Objects;
using EGrader.Views.Student;
using EGrader.Models.Factory;
using System.Windows.Controls;
using EGrader.Classes;

namespace EGrader.Controllers.Student {
    public class GradesController : Controller {

        private List<String> monthsList = new List<string>() { "9", "10", "11", "12", "1", "2", "3", "4", "5", "6" };

        List<CourseObject> coursesList;
        List<CourseRubricObject> rubricsList;

        Dictionary<String, List<GradeObject>> gradesListDictionary;

        CoursesModel coursesModel;
        CourseRubricsModel courseRubricsModel;

        GradesModel model;
        GradesView view;

        public GradesController(Model model, View view){
            this.model = (GradesModel) model;
            this.view = (GradesView) view;

            coursesModel = (CoursesModel) ModelFactory.NewModelInstance(ModelType.Courses);
            courseRubricsModel = (CourseRubricsModel) ModelFactory.NewModelInstance(ModelType.CourseRubrics);

            coursesList = new List<CourseObject>();
            rubricsList = new List<CourseRubricObject>();
            gradesListDictionary = new Dictionary<string, List<GradeObject>>();

            this.view.comboBoxCourse.SelectionChanged += ActionComboChange;

            FetchAllData();
        }


        /// <summary>
        /// Dohvaćanje svih podataka iz baze.
        /// </summary>
        void FetchAllData() {
            try {
                view.comboBoxCourse.Items.Clear();

                foreach (CourseObject course in coursesModel.GetObjectsByCriteria()) {
                    coursesList.Add(course);
                    view.comboBoxCourse.Items.Add(course.CourseName);
                }

                view.comboBoxCourse.SelectedIndex = 0;
                FetchGrades();
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message + ":\n" + ex.StackTrace);
            }
        }


        /// <summary>
        /// Dohvaćanje svih ocjena učenika iz traženog predmeta.
        /// </summary>
        void FetchGrades() {
            CourseObject selectedCourse = coursesList[view.comboBoxCourse.SelectedIndex < 0 ? 0 : view.comboBoxCourse.SelectedIndex];

            // ---------- Clearing lists ----------
            rubricsList.Clear();
            foreach (List<GradeObject> currentGradesList in gradesListDictionary.Values)
                currentGradesList.Clear();
            gradesListDictionary.Clear();

            // ---------- Fetching rubrics and grades ----------
            foreach (CourseRubricObject rubric in courseRubricsModel.GetObjectsByCriteria("course_id=", selectedCourse.Id)) {
                rubricsList.Add(rubric);
                List<GradeObject> currentGradesList = new List<GradeObject>();
                foreach (GradeObject grade in model.GetStudentGrades(CurrentUser.Id, rubric.Id))
                    currentGradesList.Add(grade);
                gradesListDictionary.Add(rubric.RubricName, currentGradesList);
            }

            CreateStringMatrix();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        GradeObject GetGradeByCell(int row, int col) {
            String rubricName = rubricsList[row - 1].RubricName;
            String month = monthsList[col - 1];

            if (!gradesListDictionary.ContainsKey(rubricName)) {
                Console.WriteLine("No such key in rubrics dictionary!");
                return null;
            }
            foreach (GradeObject gradeObj in gradesListDictionary[rubricName]) {
                if (Utils.ParseMonth(gradeObj.Date).Equals(month))
                    return gradeObj;
            }

            return null;
        }


        /// <summary>
        /// Izrada matrice ocjena.
        /// </summary>
        void CreateStringMatrix() {
            List<String> monthsListRoman = new List<string>() { "IX", "X", "XI", "XII", "I", "II", "III", "IV", "V", "VI" };

            String[,] matrix = new String[rubricsList.Count + 1, monthsList.Count + 1];

            for (int i = 0; i < monthsList.Count; i++)
                matrix[0, i + 1] = monthsListRoman[i];
            for (int i = 0; i < rubricsList.Count; i++)
                matrix[i + 1, 0] = rubricsList[i].RubricName;

            for (int i = 0; i < rubricsList.Count; i++) {
                if (!gradesListDictionary.ContainsKey(rubricsList[i].RubricName))
                    continue;

                foreach (GradeObject gradeObj in gradesListDictionary[rubricsList[i].RubricName]) {
                    String extractedMonth = Utils.ParseMonth(gradeObj.Date);
                    int monthIndex = monthsList.IndexOf(extractedMonth) + 1;
                    matrix[i + 1, monthIndex] = Convert.ToString(gradeObj.Grade);
                }//if
            }

            for (int i = 1; i <= rubricsList.Count; i++) {
                for (int j = 1; j <= monthsList.Count; j++) {
                    if (matrix[i, j] == null)
                        matrix[i, j] = "-";
                }//for 2
            }//for 

            view.CreateGrid(matrix);

            foreach (Label label in view.contentGrid.Children) {
                int labelRow = (int) label.GetValue(Grid.RowProperty);
                int labelColumn = (int) label.GetValue(Grid.ColumnProperty);
                if (labelRow > 0 && labelColumn > 0 && label.Content.ToString().CompareTo("-") != 0) {
                    GradeObject gradeObj = GetGradeByCell(labelRow, labelColumn);
                    label.ToolTip = (gradeObj == null) ? "Null object" : (gradeObj.Date + ((gradeObj.Note.Length > 0) ? (" " + gradeObj.Note) : ""));
                }
            }//foreach
        }


        void ActionComboChange(object sender, EventArgs e) {
            FetchGrades();
        }

    }//class
}
