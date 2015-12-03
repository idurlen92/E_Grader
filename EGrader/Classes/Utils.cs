using EGrader.Classes.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EGrader.Classes {
    public class Utils {


        public static String FormatDate(String dateString) {
            StringBuilder formattedDate = new StringBuilder();
            /*
            String[] months = new String[] { "siječanj", "veljača", "ožujak", "travanj", "svibanj", "lipanj", "srpanj", "kolovoz",
                                            "rujan", "listopad", "studeni", "prosinac" }; 
            //TODO: zna stavit jednu znamenku za d/m -_-
            formattedDate.Append(dateString.Substring(0, 3) + " ");
            formattedDate.Append(months[Convert.ToInt32(dateString.Substring(3, 2)) - 1] + " ");
            formattedDate.Append(dateString.Substring(6, 4) + ".");
            */
            return formattedDate.ToString();
        }



        public static String ParseMonth(String dateString) {
            int firstDot = dateString.IndexOf('.') + 1;
            int secondDot = dateString.IndexOf('.', firstDot);
            return dateString.Substring(firstDot, secondDot - firstDot);
        }


    }//class
}
