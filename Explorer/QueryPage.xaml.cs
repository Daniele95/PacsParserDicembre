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
    /// <summary>
    /// Interaction logic for QueryPage.xaml
    /// </summary>
    public partial class QueryPage : Page
    {
        public MainWindow mainWindow;

        public QueryPage(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();
        }

        private void onLocalSearchButtonClicked(object sender, RoutedEventArgs e)
        {
            mainWindow.onSearchButtonClicked("local");
        }

        private void onRemoteSearchButtonClicked(object sender, RoutedEventArgs e)
        {
            mainWindow.onSearchButtonClicked("remote");
        }

        public StudyLevelQuery getQueryFields()
        {
            StudyLevelQuery querySettings = new StudyLevelQuery();

            querySettings.SetField("PatientName", PatientNameBox.Text);
            querySettings.SetField("PatientBirthDate", PatientBirthDatePicker.Text);
            querySettings.SetField("StudyDate", StudyDatePicker.Text);
            querySettings.SetField("StudyDescription", StudyDescriptionBox.Text);
            querySettings.SetField("Modality", ModalityBox.Text);

            return querySettings;

        }

    }
}
