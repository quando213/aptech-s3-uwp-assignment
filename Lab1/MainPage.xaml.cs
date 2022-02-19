using Lab1.Entities;
using Newtonsoft.Json;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Lab1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private int genderValue;
        private string birthdayValue;
        private bool isValid = true;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Handle_Check(object sender, RoutedEventArgs e)
        {
            var check = sender as RadioButton;
            switch (check.Content)
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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Validate();
            if (!isValid)
            {
                return;
            }
            var user = new User()
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
            var dialog = new ContentDialog();
            var jsonString = JsonConvert.SerializeObject(user);
            dialog.Content = jsonString;
            dialog.CloseButtonText = "OK";
            await dialog.ShowAsync();
        }

        private void Birthday_Date_Changed(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            var date = sender;
            birthdayValue = date.Date.Value.ToString("yyyy-MM-dd");
        }

        private void Validate()
        {
            if (string.IsNullOrEmpty(firstName.Text))
            {
                checkFirstName.Text = "First name is required";
                isValid = false;
            }
            if (string.IsNullOrEmpty(lastName.Text))
            {
                checkLastName.Text = "Last name is required";
                isValid = false;
            }
            if (string.IsNullOrEmpty(email.Text))
            {
                checkEmail.Text = "Email is required";
                isValid = false;
            }
            if (string.IsNullOrEmpty(address.Text))
            {
                checkAddress.Text = "Address is required";
                isValid = false;
            }
            if (string.IsNullOrEmpty(phone.Text))
            {
                checkPhone.Text = "Phone is required";
                isValid = false;
            }
            if (string.IsNullOrEmpty(password.Password))
            {
                checkPassword.Text = "Password is required";
                isValid = false;
            }
        }
    }
}
