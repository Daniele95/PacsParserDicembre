using DataHandlerTools;
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
        

        QueryPage queryPage;
        DownloadPage downloadPage;
        ExplorerLogic explorerLogic;

        public MainWindow()
        {
            InitializeComponent();

            queryPage = new QueryPage(this);
            downloadPage = new DownloadPage();
            explorerLogic = new ExplorerLogic();
        }
        
        
        void queryClick(object o, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(queryPage);
        }
        void downloadClick(object o, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(downloadPage);
        }

        // ------------------QUERY WINDOW------------------------------

        public void onSearchButtonClicked(object sender, RoutedEventArgs e)
        {
            queryPage.stackPanel.Children.Clear();
            QueryObject querySettings = new StudyLevelQuery();
            querySettings.SetField("PatientName", "Doe*");
            List<QueryObject> results = explorerLogic.searchPatient();
        }
       

        public void onStudyQueryArrived(QueryObject queryResults)
        {
            string resultText = "";
            resultText = queryResults.GetField("PatientName") + " " + queryResults.GetField("StudyDate") + " " + queryResults.GetField("PatientID") + "\n";

            this.Dispatcher.Invoke(() => {

                Button result = new Button();
                result.Content = resultText;

                // if button pressed, do a retrieval of the series in that study
            //    currentStudyToDownload = queryResults;
                result.Click += (sender, e) => { onStudyButtonClicked(queryResults); };

                queryPage.stackPanel.Children.Add(result);
            });

        }

        // search all series of a study
        public void onStudyButtonClicked(QueryObject queryResults)
        {
            downloadPage.stackPanel.Children.Clear();
            frame.NavigationService.Navigate(downloadPage);
        }

        // add found series to series list 
        public void onSeriesQueryArrived(QueryObject queryResults)
        {
            Dispatcher.Invoke(() => {
                Button result = new Button();
                result.Content = queryResults.GetField("SeriesInstanceUID") + "  " + queryResults.GetField("SeriesDescription");
                // qui fare una query a livello immagine per ottenere series description e un'immagine rappresentativa
                result.Click += ((obj, evento) => { onSeriesButtonClicked(queryResults); });
                downloadPage.stackPanel.Children.Add(result);
            });
        }

        // ------------------------------------------------

        public void onSeriesButtonClicked(QueryObject queryResults)
        {

        }

        public void onDownloadArrived(string path)
        {
            MessageBox.Show(path);
        }

    }
}