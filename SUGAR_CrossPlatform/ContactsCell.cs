using System;
using Xamarin.Forms;

namespace SUGAR_CrossPlatform
{
	public partial class ContactsCell : ViewCell
    {
		private Frame Container;
        private StackLayout ContainerLayout;
        private Label NameLabel;
        private Image SelectedImage;

		public static readonly BindableProperty NameProperty = BindableProperty.Create("Name", typeof(string), typeof(ContactsCell), "DefaultName", BindingMode.TwoWay);
		private bool isSelected;

        public string Name 
		{
			get { return (string)GetValue(NameProperty); }
			set { SetValue(NameProperty,value); }
		}

		public ContactsCell()
		{
            // Initalize all visual elements
            Container = new Frame();
            ContainerLayout = new StackLayout();
            NameLabel = new Label();
            SelectedImage = new Image();

			// Setup all elements
			ContainerLayout.Orientation = StackOrientation.Horizontal;
			ContainerLayout.Children.Add(NameLabel);
			ContainerLayout.Children.Add(SelectedImage);
			Container.Content = ContainerLayout;
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			Console.WriteLine("ContactsCell: OnBindingContextChanged()");
            if(BindingContext != null)
			{
				NameLabel.Text = Name;
				List<String> profileContactNames = SelectContactsPage.GetProfileContactNames();


			}
		}
	}
}
