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
		public static List<string> profileContacts;

		public static readonly BindableProperty NameProperty = BindableProperty.Create("Name", typeof(string), typeof(ContactsCell), "DefaultName", BindingMode.TwoWay);
		public static readonly BindableProperty PhoneNumberAsStringProperty = BindableProperty.Create("PhoneNumbersAsString", typeof(List<string>), typeof(ContactsCell),new List<string>(), BindingMode.TwoWay);
        
        public string Name 
		{
			get { return (string)GetValue(NameProperty); }
			set { SetValue(NameProperty,value); }
		}
        
        public List<string> PhoneNumber
		{
			get { return (List<string>)GetValue(PhoneNumberAsStringProperty);  }
			set { SetValue(PhoneNumberAsStringProperty, value);  }
		}

		public ContactsCell()
		{

            // Initalize all visual elements
            Container = new Frame();
            ContainerLayout = new StackLayout();
            NameLabel = new Label();
            SelectedImage = new Image();

			// Setup all elements
			Container.WidthRequest = 230;
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

            if(profileContacts == null)
			    profileContacts = SelectContactsPage.GetProfileContactNames();

			View = Container;
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			Debug.WriteLine("ContactsCell: OnBindingContextChanged()");
            if(BindingContext != null)
			{
				NameLabel.Text = Name;

				if (profileContacts.Contains(Name))
				{
					SelectedImage.Source = ImageSource.FromResource("Checked");
				} else {
					SelectedImage.Source = ImageSource.FromResource("NotChecked");
				}

				var imageTapGestureRecognizer = new TapGestureRecognizer();
				imageTapGestureRecognizer.Tapped += (sender, e) =>
				{
					Debug.WriteLine("Selected Contact: " + Name);
					if (profileContacts.Contains(Name))
                    {
						Debug.WriteLine("Contact is " + Name + " is now disselected ...");
                        SelectedImage.Source = ImageSource.FromResource("NotChecked");
						SelectContactsPage.ModifyProfileContact('r',Name,PhoneNumber);
						foreach(string name in SelectContactsPage.GetProfileContactNames())
						{
							Debug.WriteLine(name);
						}
						foreach (string number in SelectContactsPage.GetProfileContactNumbers())
                        {
                            Debug.WriteLine(number);
                        }
                    }
                    else if(!profileContacts.Contains(Name))
                    {
						Debug.WriteLine("Contact is " + Name + " is now selected ...");
                        SelectedImage.Source = ImageSource.FromResource("Checked");
						SelectContactsPage.ModifyProfileContact('a', Name, PhoneNumber);
						foreach (string name in SelectContactsPage.GetProfileContactNames())
                        {
                            Debug.WriteLine(name);
                        }
						foreach (string number in SelectContactsPage.GetProfileContactNumbers())
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
