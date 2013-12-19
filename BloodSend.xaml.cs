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
using System.Windows.Controls.Primitives;
using System.ComponentModel;
using System.Threading;
using System.Text;
using System.Diagnostics;



namespace sdkRSSReaderCS
{
    public partial class BloodSend : PhoneApplicationPage
    {
        string Err = "NoError";

        public BloodSend()
        {
            InitializeComponent();                     
        }
       
        protected override void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            if (e.Fragment.ToString() == "Error")
            {
                
                ErrorView();
            }
            else
            {
                View();
            } 
                    
        }

        public void ErrorView(){
            headerText.Text = "Failed";
            titleText.Text = "No Internet Connection!";
            bSuccessText.Text = "unable to connect, go back and try again";
            msgText.Visibility = Visibility.Collapsed;
            bSuccessText.Visibility=Visibility.Visible;
            bSuccessPB.Visibility = Visibility.Visible;

        }
        
        public void View()
        {
            
            headerText.Text = "Success";
            titleText.Visibility = Visibility.Visible;
            msgText.Visibility = Visibility.Visible;
            bSuccessText.Visibility=Visibility.Visible;
            bSuccessPB.Visibility = Visibility.Collapsed;

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