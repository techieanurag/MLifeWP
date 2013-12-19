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


namespace sdkRSSReaderCS
{
    public partial class Home : PhoneApplicationPage
    {


        private void bloodDonors_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/bloodDonor.xaml", UriKind.Relative));
        }

        private void organDonors_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/organDonor.xaml", UriKind.Relative));
        }

        private void requestOrgan_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/organRequest.xaml", UriKind.Relative));
        }

        private void requestBlood_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/bloodRequest.xaml", UriKind.Relative));
        }

        private void myCity_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/city.xaml", UriKind.Relative));
        }

        private void filterNow_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/filterNow.xaml", UriKind.Relative));
        }

        private void supportUs_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/help.xaml", UriKind.Relative));
        }

        private void aboutUs_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/aboutUs.xaml", UriKind.Relative));
        }

        private void whyDonateOrgan_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/whyOrgan.xaml", UriKind.Relative));
        }

        private void whyDonateBlood_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/whyBlood.xaml", UriKind.Relative));
        }

        private void myTimeline_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/meRequests.xaml", UriKind.Relative));
        }


        // Constructor
        public Home()
        {
            InitializeComponent();
            
        }
    }
}
