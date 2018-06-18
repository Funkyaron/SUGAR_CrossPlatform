using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using UserNotifications;
using Contacts;

namespace SUGAR_CrossPlatform.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            // Code for starting up the Xamarin Test Cloud Agent
#if DEBUG
			Xamarin.Calabash.Start();
#endif
			/*UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert, (approved, err) =>
            {
                // Handle approval..
            });*/

			RequestThings();

            UNUserNotificationCenter.Current.Delegate = new ProfileUpdateDelegate();

            //new CNContactStore().RequestAccess(CNEntityType.Contacts, (granted, error) => { });

			LoadApplication(new App());

			return base.FinishedLaunching(app, options);
        }

		private async void RequestThings() {
			await UNUserNotificationCenter.Current.RequestAuthorizationAsync(UNAuthorizationOptions.Alert);
			await new CNContactStore().RequestAccessAsync(CNEntityType.Contacts);
		}
    }
}
