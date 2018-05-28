using System;
using System.Threading;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using Xamarin.Forms;

#if __IOS__
using Foundation;
using CallKit;
#endif

namespace SUGAR_CrossPlatform
{
    public class ProfileManager
    {
        private static readonly ReaderWriterLockSlim _rwl = new ReaderWriterLockSlim();

        public ProfileManager()
        {
        }

        public bool SaveProfile(Profile prof)
        {
            _rwl.EnterWriteLock();

            bool isSuccessful;

            XmlWriter writer = null;

            try
            {
                writer = XmlWriter.Create(Path.Combine(GetFolderPath(), prof.Name + ".xml"));

                writer.WriteStartDocument();

                writer.WriteStartElement("Profile");

                writer.WriteElementString("Name", prof.Name);

                writer.WriteStartElement("Days");
                foreach (bool day in prof.Days)
                {
                    writer.WriteStartElement("Day");
                    writer.WriteValue(day);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();

                writer.WriteStartElement("StartTimes");
                foreach (TimeUnit startTime in prof.StartTimes)
                {
                    writer.WriteElementString("StartTime", startTime.ToString());
                }
                writer.WriteEndElement();

                writer.WriteStartElement("EndTimes");
                foreach (TimeUnit endTime in prof.EndTimes)
                {
                    writer.WriteElementString("EndTime", endTime.ToString());
                }
                writer.WriteEndElement();

                writer.WriteStartElement("Active");
                writer.WriteValue(prof.Active);
                writer.WriteEndElement();

                writer.WriteStartElement("Allowed");
                writer.WriteValue(prof.Allowed);
                writer.WriteEndElement();

                writer.WriteStartElement("Mode");
                writer.WriteValue((int)prof.Mode);
                writer.WriteEndElement();

                writer.WriteStartElement("PhoneNumbersAsStrings");
                if (prof.PhoneNumbersAsStrings.Count == 0)
                {
                    writer.WriteStartElement("PhoneNumberString");
                    writer.WriteEndElement();
                }
                else
                {
                    foreach (string number in prof.PhoneNumbersAsStrings)
                    {
                        writer.WriteElementString("PhoneNumberString", number);
                    }
                }
                writer.WriteEndElement();

                writer.WriteStartElement("PhoneNumbersAsLongs");
                if (prof.PhoneNumbersAsLongs.Count == 0)
                {
                    writer.WriteStartElement("PhoneNumberLong");
                    writer.WriteEndElement();
                }
                else
                {
                    foreach (long number in prof.PhoneNumbersAsLongs)
                    {
                        writer.WriteStartElement("PhoneNumberLong");
                        writer.WriteValue(number);
                        writer.WriteEndElement();
                    }
                }
                writer.WriteEndElement();

                writer.WriteStartElement("ContactNames");
                if (prof.ContactNames.Count == 0)
                {
                    writer.WriteStartElement("ContactName");
                    writer.WriteEndElement();
                }
                else
                {
                    foreach (string name in prof.ContactNames)
                    {
                        writer.WriteElementString("ContactName", name);
                    }
                }
                writer.WriteEndElement();

                writer.WriteEndElement();

                isSuccessful = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                isSuccessful = false;
            }
            writer?.Close();
            _rwl.ExitWriteLock();

            return isSuccessful;
        }

		public void DeleteProfile(string name) {
			string path = Path.Combine(GetFolderPath(), name + ".xml");
			if(File.Exists(path)) {
				File.Delete(path);
			}
		}

        public Profile GetProfile(string name)
        {
            return ReadProfile(Path.Combine(GetFolderPath(), name + ".xml"));
        }

        // If there is a file that is not a valid profile, it is just skipped.
        public Profile[] GetAllProfiles()
        {
            List<Profile> readProfiles = new List<Profile>();
            var allFiles = Directory.EnumerateFiles(GetFolderPath());

            foreach (string filePath in allFiles)
            {
                Profile currentProfile = ReadProfile(filePath);
                if (currentProfile != null)
                {
                    readProfiles.Add(currentProfile);
                }
            }

            return readProfiles.ToArray();
        }


        public void InitProfile(Profile prof)
        {
            if (prof.Active == false)
            {
                prof.Allowed = true;
            }
            else
            {

                // Find out if the Profile is currently enabled or disabled
                DateTime now = DateTime.Now;
                int currentDayIndex = ToIndex(now.DayOfWeek);
                int previousDayIndex = ((currentDayIndex - 1) % 7 + 7) % 7;
                TimeUnit currentTimeOfDay = new TimeUnit(now.Hour, now.Minute);

                if (prof.Days[previousDayIndex] && prof.EndTimes[previousDayIndex] < prof.StartTimes[previousDayIndex] 
                    && currentTimeOfDay < prof.EndTimes[previousDayIndex])
                {
                    // Profile is enabled
                    prof.Allowed = false;
                }
                else if(prof.Days[currentDayIndex] && prof.StartTimes[currentDayIndex] < prof.EndTimes[currentDayIndex] 
                        && prof.StartTimes[currentDayIndex] < currentTimeOfDay && currentTimeOfDay < prof.EndTimes[currentDayIndex])
                {
                    // Profile is enabled
                    prof.Allowed = false;
                }
                else if(prof.Days[currentDayIndex] && prof.EndTimes[currentDayIndex] < prof.StartTimes[currentDayIndex]
                        && prof.StartTimes[currentDayIndex] < currentTimeOfDay)
                {
                    // Profile is enabled
                    prof.Allowed = false;
                }
                else
                {
                    // Profile is disabled.
                    prof.Allowed = true;
                }

                SaveProfile(prof);

                // Now schedule the enabling and disabling actions.
                IScheduler scheduler = DependencyService.Get<IScheduler>();
                scheduler.ScheduleNextEnable(prof);
                scheduler.ScheduleNextDisable(prof);
            }

#if __IOS__
            var callDirManager = CXCallDirectoryManager.SharedInstance;

            callDirManager.ReloadExtension(
                "de.unisiegen.SUGAR-CrossPlatform.PhoneBlockExtension",
               error =>
               {
                   if (error == null)
                   {
                       // Everything's fine
                   }
                   else
                   {
                       // Error
                   }
               });
#endif
        }



        private string GetFolderPath()
        {
            string folderPath = "";
#if __Android__
            folderPath += Environment.GetFolderPath(Environment.SpecialFolder.Personal);
#endif
#if __IOS__
            NSFileManager fileMgr = NSFileManager.DefaultManager;
            NSUrl url = fileMgr.GetContainerUrl("group.de.unisiegen.SUGAR-CrossPlatform");
            folderPath += url.Path;
#endif
            return folderPath;
        }


        // Returns null if an error occurs while reading.
        private Profile ReadProfile(string path)
        {
            _rwl.EnterReadLock();

            Profile result;

            XmlReader reader = null;

            try
            {
                reader = XmlReader.Create(path);

                reader.ReadToFollowing("Name");


                string profileName = reader.ReadElementContentAsString();

                bool[] days = new bool[7];
                reader.ReadStartElement();
                for (int i = 0; reader.Name == "Day" && i < 7; i++) {
                    days[i] = reader.ReadElementContentAsBoolean();
                }
                reader.ReadEndElement();

                TimeUnit[] startTimes = new TimeUnit[7];
                reader.ReadStartElement();
                for (int i = 0; reader.Name == "StartTime" && i < 7; i++) {
                    string startTimeString = reader.ReadElementContentAsString();
                    startTimes[i] = TimeUnit.Parse(startTimeString);
                }
                reader.ReadEndElement();

                TimeUnit[] endTimes = new TimeUnit[7];
                reader.ReadStartElement();
                for (int i = 0; reader.Name == "EndTime" && i < 7; i++) {
                    string endTimeString = reader.ReadElementContentAsString();
                    endTimes[i] = TimeUnit.Parse(endTimeString);
                }
                reader.ReadEndElement();

                bool active = reader.ReadElementContentAsBoolean();

                bool allowed = reader.ReadElementContentAsBoolean();

                BlockMode mode = (BlockMode) reader.ReadElementContentAsInt();

                List<string> phoneNumbersAsStrings = new List<string>();
                reader.ReadStartElement();
                while(reader.Name == "PhoneNumberString") {
                    string readNumber = reader.ReadElementContentAsString();
                    if (!string.IsNullOrEmpty(readNumber)) {
                        phoneNumbersAsStrings.Add(readNumber);
                    }
                }
                reader.ReadEndElement();

                List<long> phoneNumbersAsLongs = new List<long>();
                reader.ReadStartElement();
                while(reader.Name == "PhoneNumberLong") {
                    string readNumber = reader.ReadElementContentAsString();
                    if (!string.IsNullOrEmpty(readNumber)) {
                        phoneNumbersAsLongs.Add(long.Parse(readNumber));
                    }
                }
                reader.ReadEndElement();

                List<string> contactNames = new List<string>();
                reader.ReadStartElement();
                while(reader.Name == "ContactName") {
                    string readName = reader.ReadElementContentAsString();
                    if (!string.IsNullOrEmpty(readName)) {
                        contactNames.Add(readName);
                    }
                }
                reader.ReadEndElement();

                reader.ReadEndElement();


                result = new Profile(
                    profileName,
                    days,
                    startTimes,
                    endTimes,
                    active,
                    allowed,
                    mode,
                    phoneNumbersAsStrings,
                    phoneNumbersAsLongs,
                    contactNames
                );
            } catch(Exception e) {
                Console.WriteLine(e);
                result = null;
            }
            reader?.Close();
            _rwl.ExitReadLock();

            return result;
        }



        private int ToIndex(DayOfWeek day) {
            switch(day) {
                case DayOfWeek.Monday:
                    return 0;
                case DayOfWeek.Tuesday:
                    return 1;
                case DayOfWeek.Wednesday:
                    return 2;
                case DayOfWeek.Thursday:
                    return 3;
                case DayOfWeek.Friday:
                    return 4;
                case DayOfWeek.Saturday:
                    return 5;
                case DayOfWeek.Sunday:
                    return 6;
                default:
                    return 0;
            }
        }

        private DayOfWeek ToDayOfWeek(int index) {
            switch(index) {
                case 0:
                    return DayOfWeek.Monday;
                case 1:
                    return DayOfWeek.Tuesday;
                case 2:
                    return DayOfWeek.Wednesday;
                case 3:
                    return DayOfWeek.Thursday;
                case 4:
                    return DayOfWeek.Friday;
                case 5:
                    return DayOfWeek.Saturday;
                case 6:
                    return DayOfWeek.Sunday;
                default:
                    return DayOfWeek.Monday;
            }
        }
    }
}
