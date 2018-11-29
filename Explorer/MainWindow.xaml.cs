using PacsParserDicembre;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Explorer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            StudyLevelQuery qu = new StudyLevelQuery();
            qu.SetField("PatientName", "Doe*");
            QueryService q = new QueryService();
            List<QueryObject> queryResults = q.LaunchQuery(@"C:\Users\daniele\Desktop\QUERYRESULTS\", qu);

            foreach (QueryObject s in queryResults)
                MessageBox.Show(s.getValueByTag("PatientName"));
        }
    }
}