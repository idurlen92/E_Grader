using EGrader.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGrader.Models.Factory {

    public enum ModelType { Classes, ClassesInSchools, CourseRubrics, Courses, Grades, Schools, StudentsInClasses, UserTypes, Users };


    class ModelFactory {


        public static Model NewModelInstance(AppContext context) {
            //TODO:
            if (context == AppContext.Login || context == AppContext.Users)
                return new UsersModel();
            return null;
        }


        public static Model NewModelnstance(ModelType type) {
            //UNFINISHED
            return null;
        }


    }
}
