using EGrader.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGrader.Models.Factory {

    public enum ModelType { Classes, ClassesInSchools, CourseRubrics, Courses, Grades, Schools, StudentsInClasses, UserTypes, Users };


    class ModelFactory {


        /// <summary>
        /// Instanciranje novog modela prema traženom kontekstu.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Model NewModelInstance(AppContext context) {
            if (context == AppContext.Login || context == AppContext.Students || context == AppContext.Teachers)
                return new UsersModel();
            else if (context == AppContext.StudentGrading || context == AppContext.Grades)
                return new GradesModel();
            else if (context == AppContext.ClassAdministration)
                return new ClassesInSchoolsModel();
            return null;
        }


        /// <summary>
        /// Instanciranje novog modela prema tipu modela.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Model NewModelInstance(ModelType type) {
            if (type == ModelType.Users)
                return new UsersModel();
            else if (type == ModelType.ClassesInSchools)
                return new ClassesInSchoolsModel();
            else if (type == ModelType.Courses)
                return new CoursesModel();
            else if (type == ModelType.CourseRubrics)
                return new CourseRubricsModel();
            else if (type == ModelType.Grades)
                return new GradesModel();
            else if(type == ModelType.Classes)
                return new ClassesModel();
            return null;
        }


    }
}
