using Assignment.Entities;
using Assignment.Services;
using System;
using System.Collections.Generic;
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
    public sealed partial class Register : Page
    {
        private int genderValue = 0;
        private string birthdayValue = null;
        private bool isValid = true;

        public Register()
        {
            this.InitializeComponent();
        }

        private void Handle_Gender_Change(object sender, SelectionChangedEventArgs e)
        {
            string gender = e.AddedItems[0].ToString();
            switch (gender)
            {
                case "Male":
                    genderValue = 1;
                    break;
                case "Female":
                    genderValue = 2;
                    break;
                case "Other":
                    genderValue = 3;
                    break;
            }
        }

        private void Handle_Birthday_Change(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            birthdayValue = sender.Date.Value.ToString("yyyy-MM-dd");
        }

        private async void Submit(object sender, RoutedEventArgs e)
        {
            Validate();
            if (!isValid)
            {
                return;
            }
            LoadingSpinner.Visibility = Visibility.Visible;
            ContentDialog dialog = new ContentDialog();
            UserService service = new UserService();
            User user = new User()
            {
                firstName = firstName.Text,
                lastName = lastName.Text,
                email = email.Text,
                phone = phone.Text,
                password = password.Password,
                address = address.Text,
                gender = genderValue,
                avatar = avatar.Text,
                birthday = birthdayValue,
                introduction = introduction.Text
            };
            var result = await service.Register(user);
            LoadingSpinner.Visibility = Visibility.Collapsed;
            if (result)
            {
                dialog.Title = "Success";
                dialog.Content = "Your account has been created. Please login to continue.";
                dialog.CloseButtonText = "Close";
                await dialog.ShowAsync();
                this.Frame.Navigate(typeof(Pages.Login));
            }
            else
            {
                dialog.Title = "Error";
                dialog.Content = "An error occured. Please try again later.";
                dialog.CloseButtonText = "Close";
                await dialog.ShowAsync();
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
            } else if (!Regex.IsMatch(email.Text, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
            {
                AddValidationMessage("Invalid email format");
            }
            // Validate passwords
            if (string.IsNullOrEmpty(password.Password) || string.IsNullOrEmpty(confirmPassword.Password))
            {
                AddValidationMessage("Password is required");
            } else if (password.Password != confirmPassword.Password)
            {
                AddValidationMessage("Passwords do not match");
            }
            // Validate birthday
            if (birthdayValue == null)
            {
                AddValidationMessage("Birthday is required");
            }
            else if (birthday.Date > DateTime.Now)
            {
                AddValidationMessage("Invalid birthday");
            }
            // Validate other required fields
            if (string.IsNullOrEmpty(firstName.Text))
            {
                AddValidationMessage("First name is required");
            }
            if (string.IsNullOrEmpty(lastName.Text))
            {
                AddValidationMessage("Last name is required");
            }
            if (genderValue == 0)
            {
                AddValidationMessage("Gender is required");
            }
            if (string.IsNullOrEmpty(phone.Text))
            {
                AddValidationMessage("Phone is required");
            }
            if (string.IsNullOrEmpty(address.Text))
            {
                AddValidationMessage("Address is required");
            }
            if (string.IsNullOrEmpty(avatar.Text))
            {
                AddValidationMessage("Avatar is required");
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

        private void Open_Login(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Pages.Login));
        }
    }
}
