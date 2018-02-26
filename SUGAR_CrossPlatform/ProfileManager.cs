using System;
using System.Threading;
using System.IO;
using System.Xml;
using System.Collections.Generic;

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

        public Profile ReadProfile(string name)
        {
            _rwl.EnterReadLock();

            Profile result;

            XmlReader reader = null;

            try
            {
                var fileName = name + ".xml";
                var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var filePath = Path.Combine(documentsPath, fileName);

                reader = XmlReader.Create(filePath);

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

        private void CreateError()
        {
            throw new Exception("Haha");
        }
    }
}
