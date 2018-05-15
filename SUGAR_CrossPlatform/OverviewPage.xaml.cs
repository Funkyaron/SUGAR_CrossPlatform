using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace SUGAR_CrossPlatform
{
    public partial class OverviewPage : ContentPage
    {
		public OverviewPage()
		{
			InitializeComponent();

			var AddTapGestureRecognizer = new TapGestureRecognizer();
			var DeleteTapGestureRecognizer = new TapGestureRecognizer();
			var DownloadTapGestureRecognizer = new TapGestureRecognizer();

			AddTapGestureRecognizer.Tapped += (s, e) =>
			{
				System.Console.WriteLine("It worked!");
				Application.Current.MainPage.Navigation.PushAsync(new CreateProfilePage());
			};

			DeleteTapGestureRecognizer.Tapped += (s, e) =>
            {
                Application.Current.MainPage.Navigation.PushAsync(new CreateProfilePage());
            };

			DownloadTapGestureRecognizer.Tapped += (s, e) =>
            {
                Application.Current.MainPage.Navigation.PushAsync(new CreateProfilePage());
            };

			AddProfile.Source = ImageSource.FromResource("Add.png");
            DeleteProfile.Source = ImageSource.FromResource("Delete.png");
            DownloadProfile.Source = ImageSource.FromResource("Download.png");

			AddProfile.GestureRecognizers.Add(AddTapGestureRecognizer);
			DeleteProfile.GestureRecognizers.Add(DeleteTapGestureRecognizer);
			DownloadProfile.GestureRecognizers.Add(DownloadTapGestureRecognizer);

			var customCell = new DataTemplate(typeof(ProfileCell));
			customCell.SetBinding(ProfileCell.NameProperty, "Name");
			customCell.SetBinding(ProfileCell.ActiveProperty, "Active");

			ListView profList = new ListView()
			{
				ItemTemplate = customCell
			};

			profList.RowHeight = 100;

			List<Profile> testProfiles = new List<Profile>
			{
				new Profile() { Name = "Kurz", Active = true },
				new Profile() { Name = "Lang", Active = false }
			};

			profList.ItemsSource = testProfiles;
			profList.SeparatorVisibility = SeparatorVisibility.None;
			profList.ItemSelected += (sender, e) =>
			{
				((ListView)sender).SelectedItem = null;
			};

			container.Content = profList;
		}
    }
}
