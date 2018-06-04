using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SUGAR_CrossPlatform
{
	public partial class SelectContactsPage : ContentPage
	{
		public static Profile SelectedProfile;
		private ListView ContactNamesList;

		public SelectContactsPage(Profile passedProfile)
		{
			InitializeComponent();
			SelectedProfile = passedProfile;

			var customCell = new DataTemplate(typeof(ContactsCell));
			customCell.SetBinding(ContactsCell.NameProperty, "Name");
			customCell.SetBinding(ContactsCell.PhoneNumbersAsStringProperty,"PhoneNumbersAsStrings");
			customCell.SetBinding(ContactsCell.PhoneNumbersAsLongProperty, "PhoneNumbersAsLongs");
            
			ContactNamesList = new ListView()
			{
				ItemTemplate = customCell,
				ItemsSource = DependencyService.Get<IContactsFetcher>().GetAllContacts(),
                RowHeight = 100,
                SeparatorVisibility = SeparatorVisibility.None
			};

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

		public static List<string> GetProfileContactNumbers()
        {
			return SelectedProfile.PhoneNumbersAsStrings;
        }
        
        public Profile GetProfile()
        {
			return SelectedProfile;
        }
        
        
        public static void ModifyProfileContact(char type,string name,List<string> numbers,List<long> numbersAsLongs)
		{
			switch(type)
			{
				case 'a':
				{
						SelectedProfile.ContactNames.Add(name);
                        foreach(string phoneNumber in numbers)
						{
							SelectedProfile.PhoneNumbersAsStrings.Add(phoneNumber);
						}
                        foreach(long phoneNumber in numbersAsLongs)
                        {
							SelectedProfile.PhoneNumbersAsLongs.Add(phoneNumber);
                        }
						break;
                }

				case 'r':
				{
						SelectedProfile.ContactNames.Remove(name);
						foreach (string phoneNumber in numbers)
                        {
                            SelectedProfile.PhoneNumbersAsStrings.Remove(phoneNumber);
                        }
                        foreach(long phoneNumber in numbersAsLongs)
                        {
                            SelectedProfile.PhoneNumbersAsLongs.Add(phoneNumber);
                        }
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
