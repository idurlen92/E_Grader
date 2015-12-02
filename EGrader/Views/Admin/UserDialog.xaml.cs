using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EGrader.Views.Admin {
    /// <summary>
    /// Interaction logic for UserDialog.xaml
    /// </summary>
    public partial class UserDialog : Window {

        ComboBox comboBox = null;


        public UserDialog() {
            InitializeComponent();
        }


        public ComboBox CurrentComboBox { get { return comboBox; } }


        public void CreateClassesList() {
            Label label = new Label();
            Grid.SetColumn(label, 0);
            Grid.SetRow(label, 5);
            label.Content = "Razred:";

            comboBox = new ComboBox();
            comboBox.Height = 25;
            Grid.SetColumn(comboBox, 1);
            Grid.SetRow(comboBox, 5);

            contentGrid.Children.Add(label);
            contentGrid.Children.Add(comboBox);
        }


    }//class
}
