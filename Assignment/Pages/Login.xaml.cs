using Assignment.Entities;
using Assignment.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Login : Page
    {
        private UserService service = new UserService();
        private bool isValid = true;

        public Login()
        {
            this.InitializeComponent();
        }

        private async void Submit(object sender, RoutedEventArgs e)
        {
            Validate();
            if (!isValid)
            {
                return;
            }
            LoadingSpinner.Visibility = Visibility.Visible;
            LoginInformation loginInformation = new LoginInformation()
            {
                email = email.Text,
                password = password.Password.ToString()
            };
            Credential credential = await service.Login(loginInformation);
            LoadingSpinner.Visibility = Visibility.Collapsed;
            if (credential == null)
            {
                ContentDialog contentDialog = new ContentDialog();
                contentDialog.Title = "Login failed";
                contentDialog.Content = "Please check your email and password, or try again later.";
                contentDialog.PrimaryButtonText = "Close";
                await contentDialog.ShowAsync();
            }
            else
            {
                ContentDialog contentDialog = new ContentDialog();
                contentDialog.Title = "Login succeeded";
                contentDialog.Content = "Welcome back!";
                contentDialog.PrimaryButtonText = "Close";
                await contentDialog.ShowAsync();
                this.Frame.Navigate(typeof(Pages.Navigation));
            }
        }

        private void Validate()
        {
            // Reset validation
            isValid = true;
            validationMessage.Text = "";
            // Validate email
            if (string.IsNullOrEmpty(email.Text))
            {
                AddValidationMessage("Email is required");
            }
            else if (!Regex.IsMatch(email.Text, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
            {
                AddValidationMessage("Invalid email format");
            }
            // Validate passwords
            if (string.IsNullOrEmpty(password.Password))
            {
                AddValidationMessage("Password is required");
            }
            // Show validation message
            if (!isValid)
            {
                validationMessage.Visibility = Visibility.Visible;
            }
        }

        private void AddValidationMessage(string message)
        {
            if (string.IsNullOrEmpty(validationMessage.Text))
            {
                validationMessage.Text = message;
            }
            else
            {
                validationMessage.Text += "\n";
                validationMessage.Text += message;
            }
            isValid = false;
        }

        private void Open_Register(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Pages.Register));
        }

        private void password_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Submit(sender, e);
            }
        }
    }
}
