using Assignment.Entities;
using Assignment.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
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
    public sealed partial class MySongs : Page
    {
        private SongService songService = new SongService();
        public MySongs()
        {
            this.Loaded += MySongs_Loaded;
            this.InitializeComponent();
        }

        private async void MySongs_Loaded(object sender, RoutedEventArgs e)
        {
            LoadingControl.IsLoading = true;
            var listSong = await songService.GetMySongs();
            MyGridView.ItemsSource = listSong;
            LoadingControl.IsLoading = false;
        }

        private void MyGridView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var selectedItem = (Song)MyGridView.SelectedItem;
            MyMediaPlayer.Source = MediaSource.CreateFromUri(new Uri(selectedItem.link));
        }
    }
}
