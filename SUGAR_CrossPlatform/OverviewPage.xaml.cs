using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace SUGAR_CrossPlatform
{
    public partial class OverviewPage : ContentPage
    {
        private ProfileManager ProfManager;
        private Profile[] allProfiles;
        private ListView profList;
		private StackLayout TopBarOverview;
		private StackLayout TopBarDelete;
		private DataTemplate customCell;

		protected override void OnAppearing()
		{
			base.OnAppearing();
		}

		public OverviewPage()
        {
            InitializeComponent();

            ProfManager = new ProfileManager();
            allProfiles = ProfManager.GetAllProfiles();

			var AddProfileImage = new Image()
			{
				WidthRequest = 25,
				HeightRequest = 25,
				Source = ImageSource.FromResource("Add.png")
			};

			var DeleteProfileImage = new Image()
			{
				WidthRequest = 25,
				HeightRequest = 25,
				Source = ImageSource.FromResource("Delete.png")
			};
			var DownloadImage = new Image()
			{
				WidthRequest = 25,
				HeightRequest = 25,
				Source = ImageSource.FromResource("Download.png")
			};

            var AddTapGestureRecognizer = new TapGestureRecognizer();
            var DeleteTapGestureRecognizer = new TapGestureRecognizer();
            var DownloadTapGestureRecognizer = new TapGestureRecognizer();

            AddTapGestureRecognizer.Tapped += (s, e) =>
            {
                Application.Current.MainPage.Navigation.PushAsync(new CreateProfilePage(this));
            };

            DeleteTapGestureRecognizer.Tapped += (s, e) =>
            {
				TopBar.Content = TopBarDelete;
                var deleteCell = new DataTemplate(typeof(DeleteProfileCell));
                profList = new ListView()
                {
                    ItemTemplate = deleteCell,
                    ItemsSource = allProfiles,
                    RowHeight = 100,
                    SeparatorVisibility = SeparatorVisibility.None
                };
                deleteCell.SetBinding(DeleteProfileCell.NameProperty, "Name");
                profList.ItemTapped += (sender, args) =>
                {
                    Profile deadProfile = (Profile)((ListView)sender).SelectedItem;
                    ((ListView)sender).SelectedItem = null;
                    ProfManager.DeleteProfile(deadProfile.Name);
                    allProfiles = ProfManager.GetAllProfiles();
                    ((ListView)sender).ItemsSource = allProfiles;
                };
                container.Content = profList;
            };

            DownloadTapGestureRecognizer.Tapped += (s, e) =>
            {
				Navigation.PushAsync(new DownloadProfilesPage(this));
            };

            AddProfileImage.GestureRecognizers.Add(AddTapGestureRecognizer);
            DeleteProfileImage.GestureRecognizers.Add(DeleteTapGestureRecognizer);
            DownloadImage.GestureRecognizers.Add(DownloadTapGestureRecognizer);

			TopBarOverview = new StackLayout()
			{
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.End,
				Spacing = 20,
				Margin = new Thickness(0, 10, 20, 10)
			};
            
			TopBarOverview.Children.Add(AddProfileImage);
			TopBarOverview.Children.Add(DeleteProfileImage);
			TopBarOverview.Children.Add(DownloadImage);

			var FinishedImage = new Image()
			{
				WidthRequest = 25,
				HeightRequest = 25,
				Source = ImageSource.FromResource("Checked")
			};

		    var FinishedTapGestureRecognizer = new TapGestureRecognizer();
			FinishedTapGestureRecognizer.Tapped += (sender, args) =>
			{
				TopBar.Content = TopBarOverview;
				profList = new ListView()
				{
					ItemTemplate = customCell,
					ItemsSource = allProfiles,
					RowHeight = 100,
					SeparatorVisibility = SeparatorVisibility.None
				};
				profList.ItemSelected += (s, e) =>
				{
					((ListView)s).SelectedItem = null;
				};
				container.Content = profList;
			};
			FinishedImage.GestureRecognizers.Add(FinishedTapGestureRecognizer);

			TopBarDelete = new StackLayout()
			{
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.End,
				Spacing = 20,
				Margin = new Thickness(0, 10, 20, 10)
			};
			TopBarDelete.Children.Add(FinishedImage);
            
			TopBar.Content = TopBarOverview;

            customCell = new DataTemplate(typeof(ProfileCell));
            customCell.SetBinding(ProfileCell.NameProperty, "Name");
            customCell.SetBinding(ProfileCell.ActiveProperty, "Active");
            customCell.SetBinding(ProfileCell.AllowedProperty, "Allowed");

            profList = new ListView()
            {
                ItemTemplate = customCell,
                ItemsSource = allProfiles, // Input is Profile, so it will look for property in Profile class
                RowHeight = 100,
				SeparatorVisibility = SeparatorVisibility.None
            };

            profList.ItemSelected += (sender, e) =>
            {
                ((ListView)sender).SelectedItem = null;
            };

            container.Content = profList;
        }

        public void UpdateList()
        {
            allProfiles = ProfManager.GetAllProfiles();
            Console.WriteLine("Profiles Count: " + allProfiles.Length);
            profList.ItemsSource = allProfiles;
            container.Content = profList;
        }
    }
}