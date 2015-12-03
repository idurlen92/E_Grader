using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Controls;

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
