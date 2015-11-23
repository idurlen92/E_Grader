using EGrader.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGrader.Models.Factory {

    public enum ModelType { Users, Schools };

    class ModelFactory {


        public static Model NewModelInstance(AppContext context) {
            //UNFINISHED
            if (context == AppContext.Login)
                return new UsersModel();
            return null;
        }


        public static Model NewModelnstance(ModelType type) {
            //UNFINISHED
            return null;
        }


    }
}
