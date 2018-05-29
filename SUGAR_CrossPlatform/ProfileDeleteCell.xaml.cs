using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SUGAR_CrossPlatform
{

    public partial class ProfileDeleteCell : ContentPage
    {
		Frame container;
        StackLayout CellContainer;
        Image DeleteImage;
		public static List<String> deletionList;

        public ProfileDeleteCell()
        {
            // Initalize Cell components
            container = new Frame();
            container.OutlineColor = Color.Black;
            DeleteImage = new Image();
            CellContainer = new StackLayout();

            // Setup cell parameters 
            CellContainer.Orientation = StackOrientation.Horizontal;
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

                // Configure the Delete button 
                DeleteImage.WidthRequest = 50;
                DeleteImage.HeightRequest = 50;
                var imageTapGestureRecognizer = new TapGestureRecognizer();
                imageTapGestureRecognizer.Tapped += (s, e) =>
                {
					
                };
                DeleteImage.GestureRecognizers.Add(imageTapGestureRecognizer);

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
                    DeleteImage.Source = ImageSource.FromResource("PowerOn.png");
                }
                else if (!Active)
                {
                    DeleteImage.Source = ImageSource.FromResource("PowerOff.png");
                }            
            }
        }
        }
    }
}
