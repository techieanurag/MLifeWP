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
using System.Text;

namespace sdkRSSReaderCS
{
    public partial class organDonor : PhoneApplicationPage
    {

        static string odonor = "http://www.techiesin.com/dogood/?s=organdonor&feed=rss2";
        string odSiteUrl = "http://www.techiesin.com/dogood/donateOrgan.php?";
        string odSendUrl = "";
        string organName="All";
        string odPrivacy = "No";
        string odBloodG = "Ap";
        string odCityName = "Mumbai";
        int odcount = 0;
        int onextFlag = 0;
        int TxtBoxTxtlen = 0;

        private void odBlood(object sender, SelectionChangedEventArgs e)
        {
            odBloodG = (oBloodBox.SelectedItem as ListBoxItem).Content.ToString();
            MessageBox.Show("Your BloodGroup is  " + odBloodG + " p is for Positive and vice versa.");
        }

        private void odCity(object sender, SelectionChangedEventArgs e)
        {
            odCityName = (oCityBox.SelectedItem as ListBoxItem).Content.ToString();
            MessageBox.Show("Your City is " + odCityName);
        }
        
        private void odOrgans(object sender, SelectionChangedEventArgs e)
        {

            organName = (organBox.SelectedItem as ListBoxItem).Content.ToString();
            MessageBox.Show("You seleted to Donate "+organName);
        }
        private void odPrivacyChecker()
        {
            for (int i = 0; i < this.panel.Children.Count; i++)
            {
                if (this.panel.Children[i].GetType().Name == "CheckBox")
                {
                    CheckBox chkbx = (CheckBox)this.panel.Children[i];
                    if ((bool)chkbx.IsChecked)
                    {
                        odPrivacy = chkbx.Name.ToString();
                    }
                }
            }
        }


        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (odmobile.Text == "" || TxtBoxTxtlen<10 || odname.Text == "" || odname.Text == null)
            {
                MessageBox.Show("Please fill all the details first.");
            }

            else
            {

                odPrivacyChecker();
                if (odPrivacy.Equals("odP")) { odPrivacy = "Yes"; } else { odPrivacy = "No"; }

                odSendUrl = "name=" + odname.Text + "&mobile=" + odmobile.Text + "&bg=" + odBloodG + "&city=" + odCityName + "&organ=" + organName + "&privacy=" + odPrivacy;
                onextFlag = 0;
                odSendData();
            }
        }

        void odSendData()
        {
            osendToServer();
        }

        void osendToServer()
        {

            HttpWebRequest omyRequest = (HttpWebRequest)HttpWebRequest.Create(odSiteUrl);
            omyRequest.Method = "POST";
            omyRequest.ContentType = "application/x-www-form-urlencoded";
            omyRequest.BeginGetRequestStream(new AsyncCallback(oGetRequestStreamCallback), omyRequest);
            MessageBox.Show("Your Mobile No. " + odmobile.Text + " is your Reference#, please use it when needed");

        }

        void oGetRequestStreamCallback(IAsyncResult ocallbackResult)
        {
            HttpWebRequest omyRequest = (HttpWebRequest)ocallbackResult.AsyncState;
            // End the stream request operation
            Stream opostStream = omyRequest.EndGetRequestStream(ocallbackResult);

            // Create the post data
            string opostData = odSendUrl;
            byte[] obyteArray = Encoding.UTF8.GetBytes(opostData);

            // Add the post data to the web request
            opostStream.Write(obyteArray, 0, obyteArray.Length);
            opostStream.Close();

            // Start the web request
            omyRequest.BeginGetResponse(new AsyncCallback(orequest_CallBack), omyRequest);
        }


        void orequest_CallBack(IAsyncResult oresult)
        {
            try
            {
                var omyRequest = oresult.AsyncState as HttpWebRequest;
                var oresponse = (HttpWebResponse)omyRequest.EndGetResponse(oresult);
                var obaseStream = oresponse.GetResponseStream();

                MemoryStream otempStream = new MemoryStream();
                oresponse.GetResponseStream().CopyTo(otempStream);


                // if you want to read string response
                using (var oreader = new StreamReader(obaseStream))
                {

                }

            }
            catch (Exception mynewex)
            {
                onextFlag = 10;
            }
            onextStep();

        }

        public void onextStep()
        {
            Dispatcher.BeginInvoke(() =>
            {
                omovetoNext(onextFlag);
            });


        }

        public void omovetoNext(int omyflag)
        {
            if (omyflag > 0)
            {
                this.NavigationService.Navigate(new Uri("/OrganSend.xaml#Error", UriKind.Relative));
            }
            else
            {
                this.NavigationService.Navigate(new Uri("/OrganSend.xaml#NoError", UriKind.Relative));
                panel.Visibility = Visibility.Collapsed;
                oloadText.Text = "you have successfuly pledged your blood";
            }
        }

      
         void view()
        {
            WebClient webClient = new WebClient();

            // Subscribe to the DownloadStringCompleted event prior to downloading the RSS feed.
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);

            // Download the RSS feed. DownloadStringAsync was used instead of OpenStreamAsync because we do not need 
            // to leave a stream open, and we will not need to worry about closing the channel. 
            Random random = new Random();
            string url = odonor + "&random=" + random.Next().ToString();
            webClient.DownloadStringAsync(new System.Uri(url));



        }

        // Constructor
        public organDonor()
        {
            
            InitializeComponent();
            view();


            int cMinLength = 0;
            odmobile.MaxLength = 10; // let say max length for TextBox is 30
            string iLengthBuf = cMinLength + "/" + odmobile.MaxLength;  // display the text char count as : 0/max value (initially)
            ocountMob.Text = iLengthBuf;
            odmobile.TextChanged += new TextChangedEventHandler(textCountChanged); // callback method registered
        }

        private void textCountChanged(object sender, TextChangedEventArgs e)
        {
            TxtBoxTxtlen = odmobile.Text.ToString().Length;
            ocountMob.Text = TxtBoxTxtlen.ToString() + "/" + odmobile.MaxLength;  // display the text char count as : entered/max value

            if (TxtBoxTxtlen == odmobile.MaxLength)
            {
                //MessageBox.Show("Max Length Reached");
            }
        }

        private void refreshIcon_Menu(object sender, EventArgs e)
        {
            organListBox.Visibility = Visibility.Collapsed;
            oloadText.Visibility = Visibility.Visible;
            oloadText.Text = "refreshing the list, please wait";
            oDonorPB.Visibility = Visibility.Visible;
            // WebClient is used instead of HttpWebRequest in this code sample because 
            // the implementation is simpler and easier to use, and we do not need to use 
            // advanced functionality that HttpWebRequest provides, such as the ability to send headers.
            WebClient webClient = new WebClient();


            // Subscribe to the DownloadStringCompleted event prior to downloading the RSS feed.
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompletedRefresh);

            // Download the RSS feed. DownloadStringAsync was used instead of OpenStreamAsync because we do not need 
            // to leave a stream open, and we will not need to worry about closing the channel.
            Random random = new Random();
            string url = odonor + "&random=" + random.Next().ToString();
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
                    //MessageBox.Show(e.Error.Message);
                    oDonorPB.Visibility = Visibility.Collapsed;
                    oloadText.Text = "refresh failed, check your internet connectivity";
                    
                    

                });
            }
            else
            {
                UpdateFeedList(e.Result);
                organListBox.Visibility = Visibility.Visible;
                oloadText.Visibility = Visibility.Collapsed;
                oDonorPB.Visibility = Visibility.Collapsed;
                if (odcount > 0)
                {
                    MessageBox.Show("List refreshed!");
                }
                odcount++;
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
                    //MessageBox.Show(e.Error.Message);
                    oDonorPB.Visibility = Visibility.Collapsed;
                    oloadText.Text = "check your internet connectivity";
                   
                    
                    

                });
            }
            else
            {
                // Save the feed into the State property in case the application is tombstoned. 
                try
                {
                    this.State["feed"] = e.Result;
                    UpdateFeedList(e.Result);
                    oloadText.Visibility = Visibility.Collapsed;
                    oDonorPB.Visibility = Visibility.Collapsed;
                }
                catch(Exception ex){
                    ex.ToString();
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
                if (organListBox.Items.Count == 0)
                {
                    oDonorPB.Visibility = Visibility.Collapsed;
                    oloadText.Text = "check your internet connectivity";
                    //UpdateFeedList(State["feed"] as string);
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

            // In Windows Phone OS 7.1, WebClient events are raised on the same type of thread they were called upon. 
            // For example, if WebClient was run on a background thread, the event would be raised on the background thread. 
            // While WebClient can raise an event on the UI thread if called from the UI thread, a best practice is to always 
            // use the Dispatcher to update the UI. This keeps the UI thread free from heavy processing.
            int noItems = feed.Items.Count();
            if (noItems < 1)
            {

                oloadText.Visibility = Visibility.Visible;
                oloadText.Text = "no results found, check connection";

            }
            
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                // Bind the list of SyndicationItems to our ListBox
                organListBox.ItemsSource = feed.Items;

                // loadFeedButton.Content = "Refresh Feed";
            });

        }
        //Menu for menu icons		
        private void filterIcon_Menu(object sender, EventArgs e)
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



        private void organListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                    MessageBox.Show("Do you want to Share this Donation to Appreciate the Donor!!");

                    shareStatusTask.Status = myTitle;
                    shareStatusTask.Show();
                }
            }
        }
    }
}