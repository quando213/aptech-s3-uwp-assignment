using Assignment.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Assignment.Pages
{
    public sealed partial class Navigation : Page
    {
        public Navigation()
        {
            this.InitializeComponent();
        }

        private void NavigationView_Loaded(object sender, RoutedEventArgs e)
        {
            NavigationView.SelectedItem = NavigationView.MenuItems[0];
            MainContent.Navigate(typeof(Pages.LatestSongs));
        }

        private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var item = sender.SelectedItem as NavigationViewItem;
            switch (item.Tag)
            {
                case "latestSongs":
                    MainContent.Navigate(typeof(Pages.LatestSongs));
                    break;
                case "mySongs":
                    MainContent.Navigate(typeof(Pages.MySongs));
                    break;
                case "profile":
                    MainContent.Navigate(typeof(Pages.Profile));
                    break;
                case "createSong":
                    MainContent.Navigate(typeof(Pages.CreateSong));
                    break;
                case "logout":
                    UserService userService = new UserService();
                    userService.Logout();
                    break;
            }
        }
    }
}
