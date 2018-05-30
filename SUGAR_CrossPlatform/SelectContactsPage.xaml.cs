using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SUGAR_CrossPlatform
{
	public partial class SelectContactsPage : ContentPage
	{
		private static Profile SelectedProfile;
		private ListView ContactNamesList;

		public SelectContactsPage(Profile passedProfile)
		{
			InitializeComponent();
			SelectedProfile = passedProfile;

			var customCell = new DataTemplate(typeof(ContactsCell));
			customCell.SetBinding(ContactsCell.NameProperty, "Name");
			customCell.SetBinding(ContactsCell.PhoneNumberProperty, "PhoneNumber");
            
			ContactNamesList = new ListView()
			{
				ItemTemplate = customCell,
				ItemsSource = DependencyService.Get<IContactsFetcher>().GetAllContacts()
			};

			ContactNamesList.RowHeight = 100;

            ContactNamesList.SeparatorVisibility = SeparatorVisibility.None;
            ContactNamesList.ItemSelected += (sender, e) =>
            {
                ((ListView)sender).SelectedItem = null;
            };

			Layout.Children.Add(ContactNamesList);
		}

		public static List<string> GetProfileContactNames()
		{
			return SelectedProfile.ContactNames;
		}

        public static void ModifyProfileContact(char type,string name,string number)
		{
			switch(type)
			{
				case 'a':
				{
						SelectedProfile.ContactNames.Add(name);
						SelectedProfile.PhoneNumbersAsStrings.Add(number);
						break;
                }

				case 'r':
				{
						SelectedProfile.ContactNames.Remove(name);
                        SelectedProfile.PhoneNumbersAsStrings.Remove(number);
                        break;
				}

				default:
				{
					break;
				}
			}
		}
	}
}
