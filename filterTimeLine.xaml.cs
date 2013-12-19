using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using System.IO;
using System.ServiceModel.Syndication;
using System.Xml;
using Microsoft.Phone.Tasks;
using System.Windows.Navigation;
using System.Windows.Media;


namespace sdkRSSReaderCS
{
    public partial class filterTimeLine : PhoneApplicationPage
    {
        static string myurlcity = "http://www.techiesin.com/dogood/feed/?abcxyz=a";

        protected override void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            myurlcity = "http://www.techiesin.com/dogood/?s=" + e.Fragment.ToString() + "&feed=rss2";
            ftimeloadText.Text = "getting results for - " + e.Fragment.ToString(); ;
            ftimeloadText.Visibility = Visibility.Visible;
            ftimelinePB.Visibility = Visibility.Visible;
            WebClient webClient = new WebClient();

            // Subscribe to the DownloadStringCompleted event prior to downloading the RSS feed.
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompletedCity);

            // Download the RSS feed. DownloadStringAsync was used instead of OpenStreamAsync because we do not need 
            // to leave a stream open, and we will not need to worry about closing the channel. 
            webClient.DownloadStringAsync(new System.Uri(myurlcity));

        }

        void view()
        {
            WebClient webClient = new WebClient();

            // Subscribe to the DownloadStringCompleted event prior to downloading the RSS feed.
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);

            // Download the RSS feed. DownloadStringAsync was used instead of OpenStreamAsync because we do not need 
            // to leave a stream open, and we will not need to worry about closing the channel. 
            webClient.DownloadStringAsync(new System.Uri(myurlcity));

        }

        // Constructor
        public filterTimeLine()
        {
            InitializeComponent();
            view();
        }

        private void refreshIcon_Menu(object sender, EventArgs e)
        {
            ffeedListBox.Visibility = Visibility.Collapsed;
            ftimeloadText.Text = "refreshing the list, please wait";
            ftimeloadText.Visibility = Visibility.Visible;
            ftimelinePB.Visibility = Visibility.Visible;
            // WebClient is used instead of HttpWebRequest in this code sample because 
            // the implementation is simpler and easier to use, and we do not need to use 
            // advanced functionality that HttpWebRequest provides, such as the ability to send headers.
            WebClient webClient = new WebClient();


            // Subscribe to the DownloadStringCompleted event prior to downloading the RSS feed.
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompletedRefresh);

            // Download the RSS feed. DownloadStringAsync was used instead of OpenStreamAsync because we do not need 
            // to leave a stream open, and we will not need to worry about closing the channel.
            Random random = new Random();
            string url = myurlcity + "&random=" + random.Next().ToString();
            webClient.DownloadStringAsync(new System.Uri(url));

        }



        // Event handler which runs after the feed is fully refreshed.
        private void webClient_DownloadStringCompletedRefresh(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    // Showing the exact error message is useful for debugging. In a finalized application, 
                    // output a friendly and applicable string to the user instead. 
                    ftimeloadText.Text = "refresh failed, check your internet connectivity";
                    ftimelinePB.Visibility = Visibility.Collapsed;

                });
            }
            else
            {
                UpdateFeedList(e.Result);
                ffeedListBox.Visibility = Visibility.Visible;
                ftimeloadText.Visibility = Visibility.Collapsed;
                ftimelinePB.Visibility = Visibility.Collapsed;

            }
        }

        // Event handler which runs after the feed is fully refreshed.
        private void webClient_DownloadStringCompletedCity(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    // Showing the exact error message is useful for debugging. In a finalized application, 
                    // output a friendly and applicable string to the user instead. 
                    ftimeloadText.Text = "no results for this query, filter again";
                    ftimelinePB.Visibility = Visibility.Collapsed;
                });
            }
            else
            {
                UpdateFeedList(e.Result);
                ffeedListBox.Visibility = Visibility.Visible;
                ftimeloadText.Visibility = Visibility.Collapsed;
                ftimelinePB.Visibility = Visibility.Collapsed;

            }
        }


        // Event handler which runs after the feed is fully downloaded.
        private void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    // Showing the exact error message is useful for debugging. In a finalized application, 
                    // output a friendly and applicable string to the user instead. 

                    ftimeloadText.Text = "check your internet connectivity";
                    ftimelinePB.Visibility = Visibility.Collapsed;

                });
            }
            else
            {
                // Save the feed into the State property in case the application is tombstoned. 
                try
                {
                    this.State["feed"] = e.Result;
                    UpdateFeedList(e.Result);
                    ftimeloadText.Visibility = Visibility.Collapsed;
                    ftimelinePB.Visibility = Visibility.Collapsed;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Use Reload Button");
                }

            }
        }

        // This method determines whether the user has navigated to the application after the application was tombstoned.
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            // First, check whether the feed is already saved in the page state.
            if (this.State.ContainsKey("feed"))
            {
                // Get the feed again only if the application was tombstoned, which means the ListBox will be empty.
                // This is because the OnNavigatedTo method is also called when navigating between pages in your application.
                // You would want to rebind only if your application was tombstoned and page state has been lost. 
                if (ffeedListBox.Items.Count == 0)
                {
                    ftimelinePB.Visibility = Visibility.Collapsed;
                    ftimeloadText.Text = "check your internet connectivity";
                    UpdateFeedList(State["feed"] as string);
                }
            }
        }


        // This method sets up the feed and binds it to our ListBox. 
        private void UpdateFeedList(string feedXML)
        {
            // Load the feed into a SyndicationFeed instance
            StringReader stringReader = new StringReader(feedXML);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            SyndicationFeed feed = SyndicationFeed.Load(xmlReader);
            int noItems = feed.Items.Count();
            if (noItems < 1)
            {

                ftimeloadText.Visibility = Visibility.Visible;
                ftimeloadText.Text = "no results for this query, filter again";

            }

            // In Windows Phone OS 7.1, WebClient events are raised on the same type of thread they were called upon. 
            // For example, if WebClient was run on a background thread, the event would be raised on the background thread. 
            // While WebClient can raise an event on the UI thread if called from the UI thread, a best practice is to always 
            // use the Dispatcher to update the UI. This keeps the UI thread free from heavy processing.
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                // Bind the list of SyndicationItems to our ListBox
                ffeedListBox.ItemsSource = feed.Items;

                // loadFeedButton.Content = "Refresh Feed";
            });

        }

        //Menu for menu icons		
        private void backIcon_Menu(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/filterNow.xaml", UriKind.Relative));
        }
        private void homeIcon_Menu(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Home.xaml", UriKind.Relative));
        }
        private void contact_Menu(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/contactUs.xaml", UriKind.Relative));
        }

        //Menu for menu links		
        private void timeLine_Menu(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/meRequests.xaml", UriKind.Relative));
        }
        private void organD_Menu(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/organDonor.xaml", UriKind.Relative));
        }
        private void bloodD_Menu(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/bloodDonor.xaml", UriKind.Relative));
        }
        private void whyO_Menu(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/whyOrgan.xaml", UriKind.Relative));
        }
        private void whyB_Menu(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/whyBlood.xaml", UriKind.Relative));
        }
        private void organR_Menu(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/organRequest.xaml", UriKind.Relative));
        }
        private void bloodR_Menu(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/bloodRequest.xaml", UriKind.Relative));
        }
        private void cityChange_Menu(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/city.xaml", UriKind.Relative));
        }
        private void filterNow_Menu(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/filterNow.xaml", UriKind.Relative));
        }
        private void supportUs_Menu(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/help.xaml", UriKind.Relative));
        }
        private void aboutUs_Menu(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/aboutUs.xaml", UriKind.Relative));
        }
        private void howTo_Menu(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/howTo.xaml", UriKind.Relative));
        }



        private void ffeedListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            ListBox listBox = sender as ListBox;

            if (listBox != null && listBox.SelectedItem != null)
            {
                // Get the SyndicationItem that was tapped.
                SyndicationItem sItem = (SyndicationItem)listBox.SelectedItem;

                //Set up the page navigation only if a link actually exists in the feed item.
                if (sItem.Links.Count > 0)
                {
                    //Get the associated URI of the feed item.
                    //String myTitle = sItem.Title.Text;
                    String myTitle = sItem.Summary.Text;

                    // Create a new WebBrowserTask Launcher to navigate to the feed item. 
                    ShareStatusTask shareStatusTask = new ShareStatusTask();

                    // String myUpdate = "@techieanurag #meNeedLife #organRequest for " + rOrganName + " at " + ohos.Text + " in #" + orCityName + ", contact " + omyname.Text + " - " + omob.Text;
                    //MessageBox.Show(text);
                    MessageBox.Show("Do you want to Share this Request/Donation to Help/Promote?");

                    shareStatusTask.Status = myTitle;
                    shareStatusTask.Show();
                }
            }
        }
    }
}
