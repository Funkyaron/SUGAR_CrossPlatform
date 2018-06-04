using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace SUGAR_CrossPlatform
{
	public partial class ContactsCell : ViewCell
    {
		private Frame Container;
        private StackLayout ContainerLayout;
        private Label NameLabel;
        private Image SelectedImage;
		public static List<string> ProfileContactNames;
		public static List<string> ProfileNumbersAsStrings;
		public static List<long> ProfileNumbersAsLongs;

		public static readonly BindableProperty NameProperty = BindableProperty.Create("Name", typeof(string), typeof(ContactsCell), "DefaultName", BindingMode.TwoWay);
		public static readonly BindableProperty PhoneNumbersAsStringProperty = BindableProperty.Create("PhoneNumbersAsStrings", typeof(List<string>), typeof(ContactsCell),new List<string>(), BindingMode.TwoWay);
		public static readonly BindableProperty PhoneNumbersAsLongProperty = BindableProperty.Create("PhoneNumbersAsLongs", typeof(List<long>), typeof(ContactsCell), new List<long>(), BindingMode.TwoWay);
        
        public string Name 
		{
			get { return (string)GetValue(NameProperty); }
			set { SetValue(NameProperty,value); }
		}
        
        public List<string> PhoneNumbersAsStrings
		{
			get { return (List<string>)GetValue(PhoneNumbersAsStringProperty);  }
			set { SetValue(PhoneNumbersAsStringProperty, value);  }
		}
        
        public List<long> PhoneNumberssAsLong
        {
            get { return (List<long>)GetValue(PhoneNumbersAsLongProperty); }
            set { SetValue(PhoneNumbersAsLongProperty, value); }
        }

		public ContactsCell()
		{

            // Initalize all visual elements
            Container = new Frame();
            ContainerLayout = new StackLayout();
            NameLabel = new Label();
            SelectedImage = new Image();
            
			// Setup all elements
			Container.WidthRequest = 150;
            Container.Margin = new Thickness(10,0,10,0);
			Container.CornerRadius = 0;
            Container.OutlineColor = Color.Black;
			ContainerLayout.Orientation = StackOrientation.Horizontal;
			NameLabel.VerticalOptions = LayoutOptions.Center;
			SelectedImage.VerticalOptions = LayoutOptions.Center;
			SelectedImage.HorizontalOptions = LayoutOptions.EndAndExpand;
			ContainerLayout.Children.Add(NameLabel);
			ContainerLayout.Children.Add(SelectedImage);
			Container.Content = ContainerLayout;

			ProfileContactNames = SelectContactsPage.GetProfileContactNames();           
			ProfileNumbersAsStrings = SelectContactsPage.GetProfileContactNumbers();
			ProfileNumbersAsLongs = SelectContactsPage.GetProfileContactNumbersAsLongs();

			View = Container;
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			Debug.WriteLine("ContactsCell: OnBindingContextChanged()");
            if(BindingContext != null)
			{
				NameLabel.Text = Name;

				if (PhoneNumbersAsStrings.Count == 0)
					Debug.WriteLine("Error: No phone numbers for this contact!");
				if (PhoneNumberssAsLong.Count == 0)
					Debug.WriteLine("Error: No phone numbers as long for this contact!");

				if (ProfileContactNames.Contains(Name))
				{
					SelectedImage.Source = ImageSource.FromResource("Checked");
				} else if(!ProfileContactNames.Contains(Name)){
					SelectedImage.Source = ImageSource.FromResource("NotChecked");
				}

				var imageTapGestureRecognizer = new TapGestureRecognizer();
				imageTapGestureRecognizer.Tapped += (sender, e) =>
				{
					Debug.WriteLine("Selected Contact: " + Name);
					if (ProfileContactNames.Contains(Name))
                    {
						Debug.WriteLine("Contact is " + Name + " is now disselected ...");
                        SelectedImage.Source = ImageSource.FromResource("NotChecked");
						SelectContactsPage.ModifyProfileContact('r',Name,PhoneNumbersAsStrings,PhoneNumberssAsLong);
						foreach(string name in SelectContactsPage.GetProfileContactNames())
						{
							Debug.WriteLine(name);
						}
						foreach (string number in SelectContactsPage.GetProfileContactNumbers())
                        {
                            Debug.WriteLine(number);
                        }
                        foreach(long number in SelectContactsPage.GetProfileContactNumbersAsLongs())
                        {
							Debug.WriteLine(number);
                        }
                    }
                    else if(!ProfileContactNames.Contains(Name))
                    {
						Debug.WriteLine("Contact is " + Name + " is now selected ...");
                        SelectedImage.Source = ImageSource.FromResource("Checked");
						SelectContactsPage.ModifyProfileContact('a', Name,PhoneNumbersAsStrings,PhoneNumberssAsLong);
						foreach (string name in SelectContactsPage.GetProfileContactNames())
                        {
                            Debug.WriteLine(name);
                        }
						foreach (string number in SelectContactsPage.GetProfileContactNumbers())
                        {
                            Debug.WriteLine(number);
                        }
                        foreach(long number in SelectContactsPage.GetProfileContactNumbersAsLongs())
                        {
                            Debug.WriteLine(number);
                        }
                    }
				};
				SelectedImage.GestureRecognizers.Add(imageTapGestureRecognizer);
			}
		}
	}
}
