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
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Shell;
using System.Diagnostics;
using System.Windows.Controls.Primitives;
using System.Device.Location;
using System.Collections.ObjectModel;
using System.ServiceModel;

namespace sdkRSSReaderCS
{
    public partial class city : PhoneApplicationPage
    {
        String cityname = "Bhopal";
        public city()
        {            
            InitializeComponent();
        }

        private void mapError(object sender, LoadingErrorEventArgs e)
        {
            maploadText.Text = "error encoutered while map was loading";
            maploadText.Visibility = Visibility.Visible;
            mapPB.Visibility = Visibility.Visible;
        }

        private void ContactListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                cityname = (listBox1.SelectedItem as ListBoxItem).Content.ToString();
                mymap.ZoomLevel = 10;

                if (cityname == "Mumbai")
                {
                    mymap.Center = new GeoCoordinate(19.09132, 72.862587);
                }
                else if (cityname == "Delhi")
                {
                    mymap.Center = new GeoCoordinate(28.661671, 77.192459);
                }
                else if (cityname == "Pune")
                {
                    mymap.Center = new GeoCoordinate(18.518679, 73.859596);
                }
                else if (cityname == "Bangalore")
                {
                    mymap.Center = new GeoCoordinate(12.969766, 77.587967);
                }
                else if (cityname == "Bhopal")
                {
                    mymap.Center = new GeoCoordinate(23.24387, 77.41848);
                }
                else if (cityname == "Hyderabad")
                {
                    mymap.Center = new GeoCoordinate(17.380784, 78.486214);
                }
                else if (cityname == "Ahmedabad")
                {
                    mymap.Center = new GeoCoordinate(23.026502, 72.579432);
                }
                else if (cityname == "Jaipur")
                {
                    mymap.Center = new GeoCoordinate(26.910437, 75.79731);
                }
                else if (cityname == "Kolkata")
                {
                    mymap.Center = new GeoCoordinate(22.560757, 88.373566);
                }
                else if (cityname == "Patna")
                {
                    mymap.Center = new GeoCoordinate(25.609023, 85.142612);
                }
                else if (cityname == "Ranchi")
                {
                    mymap.Center = new GeoCoordinate(23.345408, 85.30941);
                }
                else if (cityname == "Srinagar")
                {
                    mymap.Center = new GeoCoordinate(34.085649, 74.798241);
                }
                else if (cityname == "Chandigarh")
                {
                    mymap.Center = new GeoCoordinate(30.727376, 76.778584);
                }
                else if (cityname == "Amritsar")
                {
                    mymap.Center = new GeoCoordinate(31.633799, 74.877262);
                }
                else if (cityname == "Lucknow")
                {
                    mymap.Center = new GeoCoordinate(26.850416, 80.949898);
                }
                else if (cityname == "Kanpur")
                {
                    mymap.Center = new GeoCoordinate(26.451517, 80.316868);
                }
                else if (cityname == "Raipur")
                {
                    mymap.Center = new GeoCoordinate(21.252262, 81.631794);
                }
                else if (cityname == "Panaji")
                {
                    mymap.Center = new GeoCoordinate(15.492062, 73.819857);
                }
                else if (cityname == "Trivendrum")
                {
                    mymap.Center = new GeoCoordinate(8.483918, 76.944695);
                }
                else if (cityname == "Chennai")
                {
                    mymap.Center = new GeoCoordinate(13.056737, 80.253639);
                }
                else if (cityname == "Bhubaneshwar")
                {
                    mymap.Center = new GeoCoordinate(20.293113, 85.826454);
                }
                else if (cityname == "Guwahati")
                {
                    mymap.Center = new GeoCoordinate(26.150199, 91.744366);
                }
                else if (cityname == "Shimla")
                {
                    mymap.Center = new GeoCoordinate(31.099394, 77.175808);
                }

                else if (cityname == "Dehradun")
                {
                    mymap.Center = new GeoCoordinate(30.31747, 78.033428);
                }


                else
                {
                    mymap.Center = new GeoCoordinate(28.652031, 77.162247);
                    mymap.ZoomLevel = 4;
                }
            }
            catch(Exception exep){

            }

        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/meRequests.xaml#" + cityname, UriKind.Relative));
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