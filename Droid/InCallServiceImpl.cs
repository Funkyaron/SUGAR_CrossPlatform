
using System;

using Android.App;
using Android.Content;
using Android.Telecom;
using Android.Media;

namespace SUGAR_CrossPlatform.Droid
{
    [Service(Label = "InCallServiceImpl")]
    [IntentFilter(new String[] { "com.yourname.InCallServiceImpl" })]
    public class InCallServiceImpl : InCallService
    {
        internal static bool shouldBlockAbsolutely = false;

        private AudioManager mAudioManager;
        private RingerMode currentRingerMode;

        public override void OnCallAdded(Call call) {
            mAudioManager = (AudioManager)GetSystemService(Context.AudioService);
            currentRingerMode = mAudioManager.RingerMode;
            mAudioManager.RingerMode = RingerMode.Silent;

            string number = GetNumber(call);
            if(ShouldBlock(number)) {
                call.Disconnect();
            } else {
                mAudioManager.RingerMode = currentRingerMode;
                base.OnCallAdded(call);
            }
        }

		public override void OnCallRemoved(Call call) {
            mAudioManager.RingerMode = currentRingerMode;
            base.OnCallRemoved(call);
		}

        private bool ShouldBlock(string number) {
            if(shouldBlockAbsolutely) {
                return true;
            }

            ProfileManager mgr = new ProfileManager();
            Profile[] allProfiles = mgr.GetAllProfiles();

            foreach(Profile prof in allProfiles) {
                if(!prof.Allowed) {
                    BlockMode mode = prof.Mode;
                    switch(mode) {
                        case BlockMode.All:
                            return true;
                        case BlockMode.BlackList:
                            return prof.PhoneNumbersAsStrings.Contains(number);
                        case BlockMode.WhiteList:
                            return !prof.PhoneNumbersAsStrings.Contains(number);
                    }
                }
            }
            return false;
        }

		private string GetNumber(Call call) {
            if(call == null) {
                return null;
            }
            if(call.GetDetails().GatewayInfo != null) {
                return call.GetDetails().GatewayInfo.OriginalAddress.SchemeSpecificPart;
            } else {
                return null;
            }
        }
    }
}
