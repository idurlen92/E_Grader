using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EGrader.Views {
    /// <summary>
    /// Interaction logic for StartView.xaml
    /// </summary>
    public partial class StartView : UserControl, View {


        public StartView() {
            InitializeComponent();
        }

        public void Update(ref DataTable dataTable) {
            throw new NotImplementedException();
        }

        public void Update(ref List<object> objectsList) {
            throw new NotImplementedException();
        }
    }
}
