using System;
using Xamarin.Forms;
namespace SUGAR_CrossPlatform
{
    public class DeleteProfileCell : ViewCell
    {
        private Label nameLabel;

        public static readonly BindableProperty NameProperty = BindableProperty.Create("Name", typeof(string), typeof(DeleteProfileCell), "DefaultName", BindingMode.TwoWay);

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public DeleteProfileCell()
        {
            Frame container = new Frame();
            container.OutlineColor = Color.Black;
            container.Margin = new Thickness(25, 5, 25, 5);
            container.Padding = new Thickness(15, 10, 15, 10);
            container.CornerRadius = 25;

            Image deleteImage = new Image()
            {
                WidthRequest = 50,
                HeightRequest = 50,
                Source = ImageSource.FromResource("Delete.png")
            };
            nameLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                FontSize = 25,
                FontAttributes = FontAttributes.Bold,
                Text = "Test"
            };
            StackLayout cellContainer = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal
            };
            cellContainer.Children.Add(deleteImage);
            cellContainer.Children.Add(nameLabel);

            container.Content = cellContainer;

            View = container;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (BindingContext != null)
            {
                nameLabel.Text = Name;
            }
        }
    }
}
