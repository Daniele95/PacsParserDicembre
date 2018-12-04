﻿using Database;
using DataHandlerTools;
using PacsParserDicembre;
using PacsParserDicembre.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
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
            frame.NavigationService.Navigate(queryPage);
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
        


        public void onSearchButtonClicked(string mode)
        {
            
            queryPage.stackPanel.Children.Clear();

            StudyLevelQuery querySettings = queryPage.getQueryFields();

            // search remote
            List<QueryObject> results = new List<QueryObject>();
            if (mode== "remote") results = explorerLogic.searchPatient(querySettings);

            foreach (QueryObject result in results)
            {
                Button resultButton = new Button();
                resultButton.Content = result.GetField("PatientName").Replace('^',' ') + "      " + result.GetField("StudyDescription").Replace('_',' ') + "      " + result.GetField("StudyDate") + "      " + result.GetField("Modality");
                resultButton.Click += (theSender, eventArgs) => { onStudyButtonClicked(result); };
                queryPage.stackPanel.Children.Add(resultButton);
            }
            // search database
            DownloadedFileInfo d = new DownloadedFileInfo();
            d.SetField("PatientName", queryPage.PatientNameBox.Text);
            List<DownloadedFileInfo> risultati = new List<DownloadedFileInfo>();
            if (mode == "local") risultati = database.Get(d, Constants.database);

            foreach (DownloadedFileInfo result in risultati)
            {
                Button resultButton = new Button();
                resultButton.Content = result.PatientName.Replace('^', ' ') + "      " + result.StudyDescription.Replace('_', ' ') + "      " + result.StudyDate + "      " + result.Modality;
                resultButton.Click += (theSender, eventArgs) => {
                    MessageBox.Show(XmlTools.getStoragePath(result));
                };
                queryPage.stackPanel.Children.Add(resultButton);
            }
        }


        // search all series of a study
        public void onStudyButtonClicked(QueryObject queryResults)
        {
            downloadPage.stackPanel.Children.Clear();
            frame.NavigationService.Navigate(downloadPage);


            List<QueryObject> results = explorerLogic.searchSeriesOfStudy((StudyLevelQuery)queryResults);


            foreach (QueryObject series in results)
            {

                // query images
                List<QueryObject> images = explorerLogic.searchImage((SeriesLevelQuery)series);
                // scarica l'immagine a metà di images.length
                int imageToDownload = (int)(images.Count / 2.0f);

                List<QueryObject> downloadedFilesInfo = explorerLogic.download(images[imageToDownload], "single");
                string filePath = Constants.listenerFolder+downloadedFilesInfo[0].GetField("SOPInstanceUID");

                Button resultButton = new Button();

                downloadPage.stackPanel.Children.Add(resultButton);
                resultButton.Click += (o, e) => {
                    explorerLogic.download(series, "series");
                };

                try
                {
                    Bitmap img = ImageTools.loadImage(filePath); // + ".dcm"

                    //-------------------------------

                    ImageBrush imgBrush = new ImageBrush();

                    imgBrush.ImageSource = ImageTools.BitmapToImageSource(img);
                    resultButton.Height = 70;
                    resultButton.Width = (int)(imgBrush.ImageSource.Width / imgBrush.ImageSource.Height * 70);

                    resultButton.Background = imgBrush;
                } catch (Exception) {  MessageBox.Show("could not load image");  }

                resultButton.Content = downloadedFilesInfo[0].GetField("SeriesDescription");
                File.Delete(filePath);
            }
        }

        private void frame_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
}