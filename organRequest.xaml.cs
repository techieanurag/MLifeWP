using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace sdkRSSReaderCS
{
    public partial class organRequest : PhoneApplicationPage
    {
        string rOrganName = "Eyes";
        string orCityName = "Mumbai";
        int TxtBoxTxtlen = 0;

        public organRequest()
        {
            InitializeComponent();
            int iMinLength = 0;
            omob.MaxLength = 10; // let say max length for TextBox is 30
            string iLengthBuf = iMinLength + "/" + omob.MaxLength;  // display the text char count as : 0/max value (initially)
            countCheck.Text = iLengthBuf;
            omob.TextChanged += new TextChangedEventHandler(textCountChanged); // callback method registered
        }

        private void textCountChanged(object sender, TextChangedEventArgs e)
        {
            TxtBoxTxtlen = omob.Text.ToString().Length;
            countCheck.Text = TxtBoxTxtlen.ToString() + "/" + omob.MaxLength;  // display the text char count as : entered/max value

            if (TxtBoxTxtlen == omob.MaxLength)
            {
                //MessageBox.Show("Max Length Reached");
            }
        }

        private void orCity(object sender, SelectionChangedEventArgs e)
        {
            orCityName = (orCityBox.SelectedItem as ListBoxItem).Content.ToString();
            MessageBox.Show("Your City is " + orCityName);
        }

        private void RequestOrgans(object sender, SelectionChangedEventArgs e)
        {

            rOrganName = (rOrganBox.SelectedItem as ListBoxItem).Content.ToString();
            MessageBox.Show("You seleted  " + rOrganName);
        }

        private void organ_request(object sender, RoutedEventArgs e)
        {
            if (ohos.Text == "" || omyname.Text == "" || TxtBoxTxtlen < 10)
            {
                MessageBox.Show("Please fill all the details first.");
            }

            string omobNo = omob.Text;

            bdPost = "name=" + omyname.Text + "&mobile=" + omobNo + "&organ=" + rOrganName + "&city=" + orCityName + "&hos=" + ohos.Text;
            lpb.Visibility = Visibility.Visible;
            bdSendData();
        }

        void bdSendData()
        {

            sendToServer();

        }

        void sendToServer()
        {

            HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(brUri + bdPost);
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), myRequest);
            MessageBox.Show("Your Mobile No. " + mob.Text + " is your Reference#, please use it when needed");

        }

        void GetRequestStreamCallback(IAsyncResult callbackResult)
        {
            HttpWebRequest myRequest = (HttpWebRequest)callbackResult.AsyncState;
            // End the stream request operation
            Stream postStream = myRequest.EndGetRequestStream(callbackResult);

            // Create the post data
            string postData = bdPost;
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


            Dispatcher.BeginInvoke(() =>
            {
                movetoNext(nextFlag);
            });


        }

        public void movetoNext(int myflag)
        {
            if (myflag > 0)
            {
                loading.Text = "Error encountered, please check your Internet Connection";
            }
            else
            {
                ContentPanel.Visibility = Visibility.Collapsed;
                loading.Text = "you have successfuly pledged your blood";
                lpb.Visibility = Visibility.Collapsed;
            }
        }
        //Menu for menu icons		
        private void requestNow_menu(object sender, EventArgs e)
        {
            if (ohos.Text == "" || omyname.Text == "" || omob.Text == "")
            {
                MessageBox.Show("Please fill all the details first.");
            }

            else
            {

                ShareStatusTask shareStatusTask = new ShareStatusTask();

                String myUpdate = "@meNeedLife #organRequest for " + rOrganName + " at " + ohos.Text + " in #" + orCityName + ", contact " + omyname.Text + " - " + omob.Text;
                shareStatusTask.Status = myUpdate;
                shareStatusTask.Show();
            }
        }


        
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
    }
}