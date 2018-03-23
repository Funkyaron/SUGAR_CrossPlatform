using System;
using System.Threading;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using Xamarin.Forms;

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
                var fileName = prof.Name + ".xml";
                var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var filePath = Path.Combine(documentsPath, fileName);

                writer = XmlWriter.Create(filePath);

                writer.WriteStartDocument();

                writer.WriteStartElement("Profile");

                writer.WriteElementString("Name", prof.Name);

                //CreateError();

                writer.WriteStartElement("Days");
                foreach(bool day in prof.Days) {
                    writer.WriteStartElement("Day");
                    writer.WriteValue(day);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();

                writer.WriteStartElement("StartTimes");
                foreach(TimeUnit startTime in prof.StartTimes) {
                    writer.WriteElementString("StartTime", startTime.ToString());
                }
                writer.WriteEndElement();

                writer.WriteStartElement("EndTimes");
                foreach(TimeUnit endTime in prof.EndTimes) {
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
                writer.WriteValue((int) prof.Mode);
                writer.WriteEndElement();

                writer.WriteStartElement("PhoneNumbers");
                if (prof.PhoneNumbers.Count == 0)
                {
                    writer.WriteStartElement("PhoneNumber");
                    writer.WriteEndElement();
                }
                else
                {
                    foreach (string number in prof.PhoneNumbers)
                    {
                        writer.WriteElementString("PhoneNumber", number);
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
                string stopThisFuckingWarning = e.StackTrace;
                isSuccessful = false;
            }
            writer?.Close();
            _rwl.ExitWriteLock();

            return isSuccessful;
        }

        public Profile GetProfile(string name) {
            var fileName = name + ".xml";
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(folderPath, fileName);

            return ReadProfile(filePath);
        }

        public Profile[] GetAllProfiles() {
            List<Profile> readProfiles = new List<Profile>();
            var allFiles = Directory.EnumerateFiles(Environment.GetFolderPath(Environment.SpecialFolder.Personal));

            foreach(string filePath in allFiles) {
                Profile currentProfile = ReadProfile(filePath);
                if(currentProfile != null) {
                    readProfiles.Add(currentProfile);
                }
            }

            return readProfiles.ToArray();
        }


        public void InitProfile(Profile prof) {
            if(prof.Active == false) {
                prof.Allowed = true;
                return;
            }

            // Find out if the Profile is currently enabled or disabled
            DateTime now = DateTime.Now;
            DayOfWeek currentDay = now.DayOfWeek;
            int currentDayIndex = ToIndex(currentDay);

            if(prof.Days[currentDayIndex] == false) {
                prof.Allowed = false;
            } else {
                TimeUnit currentTime = new TimeUnit(now.Hour, now.Minute);
                TimeUnit startTime = prof.StartTimes[currentDayIndex];
                TimeUnit endTime = prof.EndTimes[currentDayIndex];

                if(currentTime >= startTime && currentTime < endTime) {
                    // The current time lies in the allowed time span.
                    prof.Allowed = true;
                } else {
                    prof.Allowed = false;
                }
            }

            SaveProfile(prof);

            // Now schedule the enabling and disabling actions.
            IScheduler scheduler = DependencyService.Get<IScheduler>();
            scheduler.ScheduleNextEnable(prof);
            scheduler.ScheduleNextDisable(prof);
        }




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

                List<string> phoneNumbers = new List<string>();
                reader.ReadStartElement();
                while(reader.Name == "PhoneNumber") {
                    phoneNumbers.Add(reader.ReadElementContentAsString());
                }
                reader.ReadEndElement();

                List<string> contactNames = new List<string>();
                reader.ReadStartElement();
                while(reader.Name == "ContactName") {
                    contactNames.Add(reader.ReadElementContentAsString());
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
                    phoneNumbers,
                    contactNames
                );
            }
            catch (Exception e)
            {
                string stopThisFuckingWarning = e.StackTrace;
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

        private void CreateError()
        {
            throw new Exception("Haha");
        }
    }
}
