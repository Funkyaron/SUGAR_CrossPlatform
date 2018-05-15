using System;
using Xamarin.Forms;

namespace SUGAR_CrossPlatform
{
	
    public class ProfileCell : ViewCell
    {
        Frame container;
        StackLayout CellContainer;
        StackLayout TextContainer;
        Image PowerImage;
        Label ProfileName;
        Label ProfileStatus;
        
        public static readonly BindableProperty NameProperty = BindableProperty.Create("Name", typeof(string), typeof(ProfileCell), "Profile", BindingMode.TwoWay);
        public static readonly BindableProperty ActiveProperty = BindableProperty.Create("Active", typeof(bool), typeof(ProfileCell), true, BindingMode.TwoWay);

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public bool Active
        {
            get { return (bool)GetValue(ActiveProperty); }
            set { SetValue(ActiveProperty, value); }
        }

        public ProfileCell()
        {
            // Initalize Cell components
            container = new Frame();
            container.OutlineColor = Color.Black;
            PowerImage = new Image();
            CellContainer = new StackLayout();
            TextContainer = new StackLayout();
            ProfileName = new Label();
            ProfileStatus = new Label();

            // Setup cell parameters 
            CellContainer.Orientation = StackOrientation.Horizontal;
            TextContainer.Orientation = StackOrientation.Vertical;
            TextContainer.BackgroundColor = CellContainer.BackgroundColor;
            CellContainer.Children.Add(PowerImage);
            TextContainer.Children.Add(ProfileName);
            TextContainer.Children.Add(ProfileStatus);
            TextContainer.HorizontalOptions = LayoutOptions.FillAndExpand;
            CellContainer.Children.Add(TextContainer);
            container.Content = CellContainer;
            View = container;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (BindingContext != null)
            {
                // Configure the top-level container of the cell
                container.Margin = new Thickness(25, 5, 25, 5);
                container.Padding = new Thickness(15, 10, 15, 10);
                container.CornerRadius = 25;

                // Configure the ON/OFF-Button 
                PowerImage.WidthRequest = 50;
                PowerImage.HeightRequest = 50;
                var imageTapGestureRecognizer = new TapGestureRecognizer();
                imageTapGestureRecognizer.Tapped += (s, e) =>
                {
                    if (this.Active)
                    {
                        PowerImage.Source = ImageSource.FromResource("PowerOff.png");
                        ProfileStatus.Text = "Anrufe sind erlaubt";
                        this.Active = false;
                    }
                    else if (!this.Active)
                    {
                        PowerImage.Source = ImageSource.FromResource("PowerOn.png");
                        ProfileStatus.Text = "Anrufe sind verboten";
                        this.Active = true;
                    }
                };
                PowerImage.GestureRecognizers.Add(imageTapGestureRecognizer);

                // Configure the text container which holds the profile name and status
                ProfileName.FontSize = 25;
                ProfileName.FontAttributes = FontAttributes.Bold;
                ProfileStatus.FontSize = 19;
                ProfileName.Text = Name;
                ProfileName.HorizontalOptions = LayoutOptions.Center;
                ProfileStatus.HorizontalOptions = LayoutOptions.Center;
                var textTapGestureRecognizer = new TapGestureRecognizer();
				textTapGestureRecognizer.Tapped += (s, e) =>
				{
					System.Console.WriteLine("Now opening the Profile '" + Name + "'");
					Application.Current.MainPage.Navigation.PushAsync(new EditProfilePage(Name));
				};
				TextContainer.GestureRecognizers.Add(textTapGestureRecognizer);
				if (Active)
				{
					PowerImage.Source = ImageSource.FromResource("PowerOn.png");
					ProfileStatus.Text = "Anrufe sind verboten";
				}
                else if(!Active)
                {
                    PowerImage.Source = ImageSource.FromResource("PowerOff.png");
                    ProfileStatus.Text = "Anrufe sind erlaubt";
                }

            }
        }
    }
}
