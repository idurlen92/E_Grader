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
            if (context == AppContext.Login || context == AppContext.Students || context == AppContext.Teachers)
                return new UsersModel();
            return null;
        }


        public static Model NewModelInstance(ModelType type) {
            if (type == ModelType.Users)
                return new UsersModel();
            return null;
        }


    }
}
