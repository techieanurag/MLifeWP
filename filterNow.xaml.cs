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

namespace sdkRSSReaderCS
{
    public partial class filterNow : PhoneApplicationPage
    {
        string filterUri = "";
        string fCheckText = "";
        string fBloodG = "";
        string fCityName = "";
        string fOrganName = "";
        string fCatChoice = "";
        public filterNow()
        {
            InitializeComponent();
            fOrganBox.IsHitTestVisible = false;
        }

        private void fRadioChecker()
                {
                    for (int i = 0; i < this.mypanel.Children.Count; i++)
                    {
                        if (this.mypanel.Children[i].GetType().Name == "RadioButton")
                        {
                            RadioButton radio = (RadioButton)this.mypanel.Children[i];
                            if ((bool)radio.IsChecked)
                            {
                                fCheckText = radio.Name.ToString();
                                
                              }
                            }
                        }

                    if (fCheckText.Equals("Organ"))
                    {
                        fOrganBox.IsHitTestVisible = true;
                        notify.Text = "you are filtering organ donors and requests";
                        fOrganBox.Foreground = new SolidColorBrush(Colors.Black);
                    }
                    else
                    {
                        fOrganBox.Foreground = new SolidColorBrush(Colors.Red);
                        fOrganName = "";
                        fOrganBox.IsHitTestVisible = false;
                        notify.Text = "you are filtering blood donors and requests";
                        
                        
                    }
        
        
        }
        
        
        private void fBlood(object sender, SelectionChangedEventArgs e)
        {
            
            fBloodG = (fBloodBox.SelectedItem as ListBoxItem).Content.ToString();
            MessageBox.Show("Your BloodGroup is  " + fBloodG);
            
        }

        private void fCity(object sender, SelectionChangedEventArgs e)
        {
            
            fCityName = (fCityBox.SelectedItem as ListBoxItem).Content.ToString();
            MessageBox.Show("Your City is " + fCityName);
        }

        private void fCat(object sender, SelectionChangedEventArgs e)
        {
            
            fCatChoice = (fCatBox.SelectedItem as ListBoxItem).Content.ToString();
            MessageBox.Show("Your are filtering " + fCatChoice);
        }

        private void fOrgans(object sender, SelectionChangedEventArgs e)
        {
            
            fOrganName = (fOrganBox.SelectedItem as ListBoxItem).Content.ToString();
            MessageBox.Show("Your selected " + fOrganName);
        }

        private void Organ_Checked(object sender, RoutedEventArgs e)
        {
            fRadioChecker();
        }

        private void Blood_Checked(object sender, RoutedEventArgs e)
        {
            fRadioChecker();
            
        }

        private void filterNow_Click(object sender, RoutedEventArgs e)
        {

            filterUri = fCheckText+""+fCatChoice+"+" + fCityName + "+" + fBloodG + "+" + fOrganName;
           this.NavigationService.Navigate(new Uri("/filterTimeline.xaml#" + filterUri, UriKind.Relative));
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



     }
}