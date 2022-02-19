using Assignment.Entities;
using Assignment.Services;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
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
    public sealed partial class CreateSong : Windows.UI.Xaml.Controls.Page
    {
        private static string publicIDCloudinary;
        private CloudinaryDotNet.Account accountCloudinary;
        private Cloudinary cloudinary;
        private SongService songService = new SongService();
        private bool isValid = true;

        public CreateSong()
        {
            this.InitializeComponent();
            this.Loaded += CreateSong_Loaded;
        }

        private void CreateSong_Loaded(object sender, RoutedEventArgs e)
        {
            accountCloudinary = new CloudinaryDotNet.Account(
            "dn3bmj5ex",
            "344297185835677",
            "SanBwHJT4cGsaTibpYRpt0GzzmE"
            );
            cloudinary = new Cloudinary(accountCloudinary);
            cloudinary.Api.Secure = true;
        }

        private async void Submit(object sender, RoutedEventArgs e)
        {
            LoadingControl.IsLoading = true;
            Validate();
            if (!isValid)
            {
                return;
            }
            Song song = new Song()
            {
                name = name.Text,
                description = description.Text,
                singer = singer.Text,
                author = author.Text,
                thumbnail = thumbnail.Text,
                link = link.Text,
            };
            bool result = await songService.CreateSong(song);
            ContentDialog contentDialog = new ContentDialog();
            if (result)
            {
                contentDialog.Title = "Success";
                contentDialog.Content = "Song added successfully.";
            }
            else
            {
                contentDialog.Title = "Error";
                contentDialog.Content = "An error occured. Please check your information or try again later.";
            }
            contentDialog.CloseButtonText = "OK";
            await contentDialog.ShowAsync();
            //LoadingControl.IsLoading = false;
        }

        private async void Button_CreateThumbnail(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.List;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                RawUploadParams imageUploadParams = new RawUploadParams()
                {
                    File = new FileDescription(file.Name, await file.OpenStreamForReadAsync())
                };
                RawUploadResult result = await cloudinary.UploadAsync(imageUploadParams);
                publicIDCloudinary = result.PublicId;
                thumbnail.Text = result.Url.ToString();
                createThumbnail.Visibility = Visibility.Collapsed;
                deleteThumbnail.Visibility = Visibility.Visible;
            }
            else
            {
                Debug.WriteLine("Image creation failed!");
            }
        }

        private async void Button_AddSong(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.List;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".mp3");
            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                await file.OpenReadAsync();
                RawUploadParams songUploadParams = new RawUploadParams()
                {
                    File = new FileDescription(file.Name, await file.OpenStreamForReadAsync())
                };
                Debug.WriteLine(songUploadParams);
                RawUploadResult result = await cloudinary.UploadAsync(songUploadParams);
                publicIDCloudinary = result.PublicId;
                link.Text = result.Url.ToString();
                createSong.Visibility = Visibility.Collapsed;
                deleteSong.Visibility = Visibility.Visible;
            }
            else
            {
                Debug.WriteLine("Add song failed!");
            }
        }

        private async void Button_DeleteThumbnail(object sender, RoutedEventArgs e)
        {
            List<string> listPublicIdCouldinary = new List<string>();
            listPublicIdCouldinary.Add(publicIDCloudinary);
            string[] arrayPublicIdCouldinary = listPublicIdCouldinary.ToArray();
            await cloudinary.DeleteResourcesAsync(arrayPublicIdCouldinary);
            deleteThumbnail.Visibility = Visibility.Collapsed;
            createThumbnail.Visibility = Visibility.Visible;
            thumbnail.Text = "";
        }
        private async void Button_DeleteSong(object sender, RoutedEventArgs e)
        {
            List<string> listPublicIdCouldinary = new List<string>();
            listPublicIdCouldinary.Add(publicIDCloudinary);
            string[] arrayPublicIdCouldinary = listPublicIdCouldinary.ToArray();
            await cloudinary.DeleteResourcesAsync(arrayPublicIdCouldinary);
            deleteSong.Visibility = Visibility.Collapsed;
            createSong.Visibility = Visibility.Visible;
            link.Text = "";
        }

        private void Validate()
        {
            // Reset validation
            isValid = true;
            validationMessage.Text = "";
            // Validate fields
            if (string.IsNullOrEmpty(name.Text))
            {
                AddValidationMessage("Name is required");
            }
            if (string.IsNullOrEmpty(singer.Text))
            {
                AddValidationMessage("Singer is required");
            }
            if (string.IsNullOrEmpty(author.Text))
            {
                AddValidationMessage("Author is required");
            }
            if (string.IsNullOrEmpty(thumbnail.Text))
            {
                AddValidationMessage("Thumbnail is required");
            }
            if (string.IsNullOrEmpty(link.Text))
            {
                AddValidationMessage("Link is required");
            }
            else if (!link.Text.Contains(".mp3"))
            {
                AddValidationMessage("Link must contain .mp3 string");
            }
            if (string.IsNullOrEmpty(description.Text))
            {
                AddValidationMessage("Description is required");
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
    }
}
