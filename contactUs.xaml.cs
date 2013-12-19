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
    public partial class contactUs : PhoneApplicationPage
    {
        string cSiteUrl = "http://www.techiesin.com/dogood/contact.php?";
        string cSendUrl = "";
        int cnextFlag = 0;
        int TxtBoxTxtlen = 0;
        
        public contactUs()
        {
            InitializeComponent();
            myContentPanel.Visibility = Visibility.Visible;
            int cMinLength = 0;
            cMob.MaxLength = 10; // let say max length for TextBox is 30
            string iLengthBuf = cMinLength + "/" + cMob.MaxLength;  // display the text char count as : 0/max value (initially)
            countCheck.Text = iLengthBuf;
            cMob.TextChanged += new TextChangedEventHandler(textCountChanged); // callback method registered
        }

        private void textCountChanged(object sender, TextChangedEventArgs e)
        {
            TxtBoxTxtlen = cMob.Text.ToString().Length;
            countCheck.Text = TxtBoxTxtlen.ToString() + "/" + cMob.MaxLength;  // display the text char count as : entered/max value

            if (TxtBoxTxtlen == cMob.MaxLength)
            {
                //MessageBox.Show("Max Length Reached");
            }
        }
        
        private void contactCLick(object sender, RoutedEventArgs e)
        {
            if (cName.Text == "" || TxtBoxTxtlen<10 || cMail.Text == "" || cMsg.Text == "")
            {
                MessageBox.Show("Please fill all the fields correctly....");
            }

            else{
                cSendUrl = "name=" + cName.Text + "&mobile=" + cMob.Text + "&email=" + cMail.Text + "&msg=" + cMsg.Text;
                csendToServer();
               }
        }

        void csendToServer()
        {

            HttpWebRequest cmyRequest = (HttpWebRequest)HttpWebRequest.Create(cSiteUrl);
            cmyRequest.Method = "POST";
            cmyRequest.ContentType = "application/x-www-form-urlencoded";
            cmyRequest.BeginGetRequestStream(new AsyncCallback(cGetRequestStreamCallback), cmyRequest);
            MessageBox.Show("Your Mobile No. " + cMob.Text + " is your Reference#, please use it when needed");

        }

        void cGetRequestStreamCallback(IAsyncResult ccallbackResult)
        {
            HttpWebRequest cmyRequest = (HttpWebRequest)ccallbackResult.AsyncState;
            // End the stream request operation
            Stream cpostStream = cmyRequest.EndGetRequestStream(ccallbackResult);

            // Create the post data
            string cpostData = cSendUrl;
            byte[] cbyteArray = Encoding.UTF8.GetBytes(cpostData);

            // Add the post data to the web request
            cpostStream.Write(cbyteArray, 0, cbyteArray.Length);
            cpostStream.Close();

            // Start the web request
            cmyRequest.BeginGetResponse(new AsyncCallback(crequest_CallBack), cmyRequest);
        }


        void crequest_CallBack(IAsyncResult cresult)
        {
            try
            {
                var cmyRequest = cresult.AsyncState as HttpWebRequest;
                var cresponse = (HttpWebResponse)cmyRequest.EndGetResponse(cresult);
                var cbaseStream = cresponse.GetResponseStream();

                MemoryStream ctempStream = new MemoryStream();
                cresponse.GetResponseStream().CopyTo(ctempStream);


                // if you want to read string response
                using (var creader = new StreamReader(cbaseStream))
                {

                }

            }
            catch (Exception mynewex)
            {
                cnextFlag = 10;
            }
            cnextStep();

        }

        public void cnextStep()
        {
            Dispatcher.BeginInvoke(() =>
            {
                cmovetoNext(cnextFlag);
            });


        }

        public void cmovetoNext(int cmyflag)
        {
            if (cmyflag > 0)
            {
                cNotify.Text = "error! please check your internet connection";
            }
            else
            {
                cNotify.Text = "success! we will contact you back in max. 48 hours";
                myContentPanel.Visibility = Visibility.Collapsed;
            }
        }


            //Menu for menu icons		
            private void contactNow_menu(object sender, EventArgs e)
            {
                if (cName.Text == "" || TxtBoxTxtlen < 10 || cMail.Text == "" || cMsg.Text == "")
                {
                    MessageBox.Show("Please fill all the fields correctly....");
                }

                else
                {
                    cSendUrl = "name=" + cName.Text + "&mobile=" + cMob.Text + "&email=" + cMail.Text + "&msg=" + cMsg.Text;
                    csendToServer();
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