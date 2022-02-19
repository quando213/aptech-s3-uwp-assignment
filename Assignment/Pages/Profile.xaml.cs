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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Assignment.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Profile : Page
    {
        private UserService service = new UserService();
        public Profile()
        {
            this.Loaded += Profile_Loaded;
            this.InitializeComponent();
        }

        private async void Profile_Loaded(object sender, RoutedEventArgs e)
        {
            var account = await service.GetProfile();
            if (account == null)
            {
                ContentDialog contentDialog = new ContentDialog();
                contentDialog.Title = "Login required";
                contentDialog.Content = $"Please login to continue!";
                contentDialog.PrimaryButtonText = "OK";
                await contentDialog.ShowAsync();
                Frame.Navigate(typeof(Pages.Login));
            }
            else
            {
                try
                {
                    avatar.Source = new BitmapImage(new Uri(account.avatar));
                }
                catch (Exception error)
                {
                    Debug.WriteLine(error.Message);
                }
                fullName.Text = $"{account.firstName} {account.lastName}";
                email.Text = account.email;
                phone.Text = account.phone;
                address.Text = account.address;
                birthday.Text = account.birthday;
                if (!string.IsNullOrEmpty(account.introduction))
                {
                    introduction.Text = account.introduction;
                } 
                else
                {
                    introductionLabel.Visibility = Visibility.Collapsed;
                }
                switch(account.gender)
                {
                    case 1:
                        gender.Text = "Male";
                        break;
                    case 2:
                        gender.Text = "Female";
                        break;
                    case 3:
                        gender.Text = "Other";
                        break;
                }
            }
        }
    }
}
