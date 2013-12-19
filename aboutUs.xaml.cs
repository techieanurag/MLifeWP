using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Device.Location;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Tasks;
namespace sdkRSSReaderCS
{
    public partial class aboutUs : PhoneApplicationPage
    {
        private GeoCoordinateWatcher man = null;
        BingMapsTask bmt = new BingMapsTask();
        
        // Constructor
        public aboutUs()
        {
            InitializeComponent();
            map1.ZoomBarVisibility = System.Windows.Visibility.Visible;
            map1.LogoVisibility = Visibility.Collapsed;
            map1.CopyrightVisibility = Visibility.Collapsed;
          }
        

        private void hospitals_Click(object sender, RoutedEventArgs e)
        {

            bmt.SearchTerm = "Hospitals";
            bmt.Show();   
        }

        private void doctors(object sender, EventArgs e)
        {
            bmt.SearchTerm = "Doctors";
            bmt.Show();
        }

        private void bloodbank(object sender, EventArgs e)
        {
            bmt.SearchTerm = "Blood Bank";
            bmt.Show(); 
        }
        
        private void redcross(object sender, EventArgs e)
        {

            bmt.SearchTerm = "Hospitals";
            bmt.Show(); 
        }

       
        private void gpsload(object sender, EventArgs e)
        {
            if (man == null)
            {
                man = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
                man.StatusChanged += loc_StatusChanged;
            }
            if (man.Status == GeoPositionStatus.Disabled)
            {
                man.StatusChanged -= loc_StatusChanged;
                MessageBox.Show("Please Turn on Location Services");
                return;
            }
        }
        
        private void startLocationButton_Click(object sender, RoutedEventArgs e)
        {
            if (man == null)
            {
                man = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
                man.StatusChanged += loc_StatusChanged;
            }
            if (man.Status == GeoPositionStatus.Disabled)
            {
                man.StatusChanged -= loc_StatusChanged;
                MessageBox.Show("Please Turn on Location Services");
                return;
            }

            man.Start();
        }
        void loc_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            if (e.Status == GeoPositionStatus.Ready)
            {
                Pushpin pin = new Pushpin();
                pin.Template = this.Resources["pinMyLoc"] as ControlTemplate;
                pin.Location = man.Position.Location;
                mapControl.Items.Add(pin);
                map1.SetView(man.Position.Location, 15.0);
                
                man.Stop();
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