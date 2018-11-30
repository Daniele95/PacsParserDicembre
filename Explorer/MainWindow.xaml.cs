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
            ImageLevelQuery qu = new ImageLevelQuery();
            qu.SetField("SeriesInstanceUID", "1.3.6.1.4.1.5962.1.1.0.0.0.1168612284.20369.0.2");
            qu.SetField("StudyInstanceUID", "1.3.6.1.4.1.5962.1.1.0.0.0.1168612284.20369.0.1");
            QueryService q = new QueryService();
            List<QueryObject> queryResults = q.LaunchQuery(@"C:\Users\daniele\Desktop\QUERYRESULTS\", qu);

            foreach (QueryObject s in queryResults)
                MessageBox.Show(s.GetField("InstanceNumber"));
        }
    }
}