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
using Microsoft.Phone.Tasks;

namespace sdkRSSReaderCS
{

    public partial class bloodRequest : PhoneApplicationPage
    {

        string rBloodGroup = "A+";
        string brCityName = "Mumbai";
        int TxtBoxTxtlen = 0;
        int nextFlag = 0;
        string brUri = "http://techiesin.com/dogood/aRequestBlood.php?";
        string bdPost = "";

        public bloodRequest()
        {
            InitializeComponent();
            lpb.Visibility = Visibility.Collapsed;
            int iMinLength = 0; 
            mob.MaxLength = 10; // let say max length for TextBox is 30
            string iLengthBuf = iMinLength + "/" + mob.MaxLength;  // display the text char count as : 0/max value (initially)
            countCheck.Text = iLengthBuf;
            mob.TextChanged += new TextChangedEventHandler(textCountChanged); // callback method registered
        }

        private void textCountChanged(object sender, TextChangedEventArgs e)
        {
            TxtBoxTxtlen = mob.Text.ToString().Length;
            countCheck.Text = TxtBoxTxtlen.ToString() + "/" + mob.MaxLength;  // display the text char count as : entered/max value

            if (TxtBoxTxtlen == mob.MaxLength)
            {
                //MessageBox.Show("Max Length Reached");
            }
        }

        private void RequestBlood(object sender, SelectionChangedEventArgs e)
        {
            rBloodGroup = (rBloodBox.SelectedItem as ListBoxItem).Content.ToString();
            MessageBox.Show("You seleted  " + rBloodGroup);

            if (rBloodGroup == "A+")
            {
                rBloodGroup = "Ap";
            }
            else if (rBloodGroup == "B+")
            {
                rBloodGroup = "Bp";
            }
            else if (rBloodGroup == "AB+")
            {
                rBloodGroup = "ABp";
            }
            else if (rBloodGroup == "O+")
            {
                rBloodGroup = "Op";
            }
            else if (rBloodGroup == "A-")
            {
                rBloodGroup = "An";
            }
            else if (rBloodGroup == "B-")
            {
                rBloodGroup = "Bn";
            }
            else if (rBloodGroup == "AB-")
            {
                rBloodGroup = "ABn";
            }
            else
            {
                rBloodGroup = "On";
            }

        }

        private void brCity(object sender, SelectionChangedEventArgs e)
        {
            brCityName = (brCityBox.SelectedItem as ListBoxItem).Content.ToString();
            MessageBox.Show("Your City is " + brCityName);
        }
        
        
        private void blood_request(object sender, RoutedEventArgs e)
        {
            if (hos.Text == "" || myname.Text == "" || TxtBoxTxtlen<10)
            {
                MessageBox.Show("Please fill all the details first.");
            }

            else
            {
                bdPost = "name=" + myname.Text + "&mobile=" + mob.Text + "&bg=" + rBloodGroup + "&city=" + brCityName + "&hos=" + hos.Text;
                //MessageBox.Show(brUri+bdPost);
                lpb.Visibility = Visibility.Visible;
                bdSendData();
                
            }
        }

        private void requestNow_menu(object sender, EventArgs e)
        {
            if (hos.Text == "" || myname.Text == "" || TxtBoxTxtlen < 10)
            {
                MessageBox.Show("Please fill all the details first.");
            }
            
            bdPost = "name=" + myname.Text + "&mobile=" + mob.Text + "&blood=" + rBloodGroup + "&city=" + brCityName + "&hos=" + hos.Text;
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
                loading.Text = "request successful";
                lpb.Visibility = Visibility.Collapsed;
            }
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

      }
}