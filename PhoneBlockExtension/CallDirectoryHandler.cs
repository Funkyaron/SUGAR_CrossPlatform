﻿using System;
using System.Collections.Generic;

using Foundation;
using CallKit;

using SUGAR_CrossPlatform;

namespace PhoneBlockExtension
{
    [Register("CallDirectoryHandler")]
    public class CallDirectoryHandler : CXCallDirectoryProvider, ICXCallDirectoryExtensionContextDelegate
    {
        protected CallDirectoryHandler(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void BeginRequestWithExtensionContext(NSExtensionContext context)
        {
            // Note that there has to be an entry in the user's contacts to make iOS
            // block any call form a given number... I think.

            var cxContext = (CXCallDirectoryExtensionContext)context;
            cxContext.Delegate = this;

            if (!AddBlockingPhoneNumbers(cxContext))
            {
                Console.WriteLine("Unable to add blocking phone numbers");
                var error = new NSError(new NSString("CallDirectoryHandler"), 1, null);
                cxContext.CancelRequest(error);
                return;
            }

            cxContext.CompleteRequest(null);
        }

        bool AddBlockingPhoneNumbers(CXCallDirectoryExtensionContext context)
        {
            // Retrieve phone numbers to block from data store. For optimal performance and memory usage when there are many phone numbers,
            // consider only loading a subset of numbers at a given time and using autorelease pool(s) to release objects allocated during each batch of numbers which are loaded.
            //
            // Numbers must be provided in numerically ascending order.

            context.RemoveAllBlockingEntries();

            ProfileManager mgr = new ProfileManager();
            Profile[] allProfiles = mgr.GetAllProfiles();

            List<long> allNumbers = new List<long>();

            foreach(Profile prof in allProfiles) {
                if(!prof.Allowed) {
                    foreach(long phoneNumber in prof.PhoneNumbersAsLongs) {
                        allNumbers.Add(phoneNumber);
                    }
                }
            }
            allNumbers.Sort();
            foreach(long phoneNumber in allNumbers) {
                context.AddBlockingEntry(phoneNumber);
            }

            return true;
        }

        public void RequestFailed(CXCallDirectoryExtensionContext extensionContext, NSError error)
        {
            // An error occurred while adding blocking or identification entries, check the NSError for details.
            // For Call Directory error codes, see the CXErrorCodeCallDirectoryManagerError enum.
            //
            // This may be used to store the error details in a location accessible by the extension's containing app, so that the
            // app may be notified about errors which occured while loading data even if the request to load data was initiated by
            // the user in Settings instead of via the app itself.
        }
    }
}
