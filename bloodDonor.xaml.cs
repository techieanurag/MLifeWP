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
    public partial class bloodDonor : PhoneApplicationPage
    {

        static string bdonor = "http://www.techiesin.com/dogood/?s=blooddonor&feed=rss2";
        string bdSiteUrl = "http://www.techiesin.com/dogood/donateBlood.php?";
        string bdSendUrl = "";
        string bdRadioBtn = "";
        string bdYesNo = "No";
        string bdPrivacy = "No";
        string bdCityName = "Mumbai";
        string bdBloodG = "Ap";
        int bdcount = 0;
        int nextFlag = 0;
        int TxtBoxTxtlen = 0;

        private void bdBlood(object sender, SelectionChangedEventArgs e)
        {
            bdBloodG = (bBloodBox.SelectedItem as ListBoxItem).Content.ToString();
            MessageBox.Show("Your BloodGroup is  " + bdBloodG + " p is for Positive and vice versa.");
        }

        private void bdCity(object sender, SelectionChangedEventArgs e)
        {
            bdCityName = (bCityBox.SelectedItem as ListBoxItem).Content.ToString();
            MessageBox.Show("Your City is " + bdCityName);
        }
        
        
        
        private void bdRadioChecker(){
            for (int i = 0; i < this.panel.Children.Count; i++)
            {
                if (this.panel.Children[i].GetType().Name == "RadioButton")
                {
                    RadioButton radio = (RadioButton)this.panel.Children[i];
                    if ((bool)radio.IsChecked)
                    {
                        bdRadioBtn=radio.Name.ToString();
                    }
                }
            }
        }

        private void bdPrivacyChecker()
        {
            for (int i = 0; i < this.panel.Children.Count; i++)
            {
                if (this.panel.Children[i].GetType().Name == "CheckBox")
                {
                    CheckBox chkbx = (CheckBox)this.panel.Children[i];
                    if ((bool)chkbx.IsChecked)
                    {
                        bdPrivacy = chkbx.Name.ToString();
                    }
                }
            }
        }


        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (bdmobile.Text == "" || TxtBoxTxtlen<10 || bdname.Text == "" || bdname.Text == null)
            {
                MessageBox.Show("Please fill all the details first.");
            }
            else
            {
                bdPrivacyChecker();
                bdRadioChecker();
                if (bdRadioBtn.Equals("bdYes")) { bdYesNo = "Yes"; } else { bdYesNo = "No"; }
                if (bdPrivacy.Equals("bdP")) { bdPrivacy = "Yes"; } else { bdPrivacy = "No"; }
                bdSendUrl = "name=" + bdname.Text + "&mobile=" + bdmobile.Text + "&bg=" + bdBloodG + "&city=" + bdCityName + "&disease=" + bdYesNo + "&privacy=" + bdPrivacy;
                nextFlag = 0;
                bdSendData();
            }
        }

        void bdSendData()
        {      
            sendToServer();
                      
          }

        void sendToServer()
        {

            HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(bdSiteUrl);
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), myRequest);
            MessageBox.Show("Your Mobile No. "+bdmobile.Text+" is your Reference#, please use it when needed");
            
        }
        
        void GetRequestStreamCallback(IAsyncResult callbackResult)
        {
            HttpWebRequest myRequest = (HttpWebRequest)callbackResult.AsyncState;
            // End the stream request operation
            Stream postStream = myRequest.EndGetRequestStream(callbackResult);

            // Create the post data
            string postData = bdSendUrl;
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            // Add the post data to the web request
            postStream.Write(byteArray, 0, byteArray.Length);
            postStream.Close();

            // Start the web request
            myRequest.BeginGetResponse(new AsyncCallback(request_CallBack), myRequest);
        }


        void request_CallBack(IAsyncResult result)
        {
            try
            {
                var myRequest = result.AsyncState as HttpWebRequest;
                var response = (HttpWebResponse)myRequest.EndGetResponse(result);
                var baseStream = response.GetResponseStream();

                MemoryStream tempStream = new MemoryStream();
                response.GetResponseStream().CopyTo(tempStream);


                // if you want to read string response
                using (var reader = new StreamReader(baseStream))
                {

                }

            }
            catch (Exception newex)
            {
                nextFlag = 10;
             }
            nextStep();

        }

        public void nextStep()
        {

            
            Dispatcher.BeginInvoke(() => {
                movetoNext(nextFlag);                                
            });


        }

        public void movetoNext(int myflag)
        {
            if (myflag > 0)
            {
                this.NavigationService.Navigate(new Uri("/BloodSend.xaml#Error", UriKind.Relative));
            }
            else
            {
                this.NavigationService.Navigate(new Uri("/BloodSend.xaml#NoError", UriKind.Relative));
                panel.Visibility = Visibility.Collapsed;
                loadText.Text = "you have successfuly pledged your blood";
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
            string url = bdonor + "&random=" + random.Next().ToString();
            webClient.DownloadStringAsync(new System.Uri(url));



        }

        // Constructor
        public bloodDonor()
        {
            
            InitializeComponent();
            view();

            int cMinLength = 0;
            bdmobile.MaxLength = 10; // let say max length for TextBox is 30
            string iLengthBuf = cMinLength + "/" + bdmobile.MaxLength;  // display the text char count as : 0/max value (initially)
            countMob.Text = iLengthBuf;
            bdmobile.TextChanged += new TextChangedEventHandler(textCountChanged); // callback method registered
        }

        private void textCountChanged(object sender, TextChangedEventArgs e)
        {
            TxtBoxTxtlen = bdmobile.Text.ToString().Length;
            countMob.Text = TxtBoxTxtlen.ToString() + "/" + bdmobile.MaxLength;  // display the text char count as : entered/max value

            if (TxtBoxTxtlen == bdmobile.MaxLength)
            {
                //MessageBox.Show("Max Length Reached");
            }
        }
        
        private void refreshIcon_Menu(object sender, EventArgs e)
        {
            bloodListBox.Visibility = Visibility.Collapsed;
            loadText.Visibility = Visibility.Visible;
            loadText.Text = "refreshing the list, please wait";
            bDonorPB.Visibility = Visibility.Visible;
            // WebClient is used instead of HttpWebRequest in this code sample because 
            // the implementation is simpler and easier to use, and we do not need to use 
            // advanced functionality that HttpWebRequest provides, such as the ability to send headers.
            WebClient webClient = new WebClient();


            // Subscribe to the DownloadStringCompleted event prior to downloading the RSS feed.
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompletedRefresh);

            // Download the RSS feed. DownloadStringAsync was used instead of OpenStreamAsync because we do not need 
            // to leave a stream open, and we will not need to worry about closing the channel.
            Random random = new Random();
            string url = bdonor + "&random=" + random.Next().ToString();
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
                    bDonorPB.Visibility = Visibility.Collapsed;
                    loadText.Text = "refresh failed, check your internet connectivity";
                    
                    

                });
            }
            else
            {
                UpdateFeedList(e.Result);
                bloodListBox.Visibility = Visibility.Visible;
                loadText.Visibility = Visibility.Collapsed;
                bDonorPB.Visibility = Visibility.Collapsed;
                if (bdcount > 0)
                {
                    MessageBox.Show("List refreshed!");
                }
                bdcount++;
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
                    bDonorPB.Visibility = Visibility.Collapsed;
                    loadText.Text = "check your internet connectivity";
                   
                });
            }
            else
            {
                // Save the feed into the State property in case the application is tombstoned. 
                try
                {
                    this.State["feed"] = e.Result;
                    UpdateFeedList(e.Result);
                    loadText.Visibility = Visibility.Collapsed;
                    bDonorPB.Visibility = Visibility.Collapsed;
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
                if (bloodListBox.Items.Count == 0)
                {
                    bDonorPB.Visibility = Visibility.Collapsed;
                    loadText.Text = "check your internet connectivity";
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

                loadText.Visibility = Visibility.Visible;
                loadText.Text = "no results found, check connection";

            }
            
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                // Bind the list of SyndicationItems to our ListBox
                bloodListBox.ItemsSource = feed.Items;

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



        private void bloodListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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