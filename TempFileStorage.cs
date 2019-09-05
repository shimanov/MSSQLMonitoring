using System;
using System.IO;
using System.IO.IsolatedStorage;

namespace DatabaseMaintenance
{
    class TempFileStorage
    {
        public string Name { get; set; }

        //Create folder in IsolatedStorage
        public void CreateFolder()
        {
            using (IsolatedStorageFile isolated = IsolatedStorageFile.GetStore(IsolatedStorageScope.User
                | IsolatedStorageScope.Domain
                | IsolatedStorageScope.Assembly, null, null))
            {
                if (!isolated.DirectoryExists("DatabaseMaintenance"))
                {
                    try
                    {
                        isolated.CreateDirectory("DatabaseMaintenance");
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
        }

        //Create file in IsolatedStorage
        public bool CreateFile(string fileName, string text)
        {
            using (IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetStore(IsolatedStorageScope.User
                | IsolatedStorageScope.Domain
                | IsolatedStorageScope.Assembly, null, null))
            {
                try
                {
                    isolatedStorage.CreateFile("DatabaseMaintenance/" + fileName);
                    using (StreamWriter writer = new StreamWriter("DatabaseMaintenance/" + fileName))
                    {
                        writer.WriteLine(text);
                    }
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        //Delete file in IsolatedStorage
        public bool DeleteFile(string fileName)
        {
            using (IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetStore(IsolatedStorageScope.User
                | IsolatedStorageScope.Domain
                | IsolatedStorageScope.Assembly, null, null))
            {
                if (isolatedStorage.DirectoryExists("DatabaseMaintenance"))
                {
                    isolatedStorage.DeleteDirectory("DatabaseMaintenance");
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        //Read file from IsolatedStorage
        public string Read(string fileName)
        {
            string text = string.Empty;

            using (IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetStore(IsolatedStorageScope.User
                | IsolatedStorageScope.Domain
                | IsolatedStorageScope.Assembly, null, null))
            {
                if (isolatedStorage.FileExists("DatabaseMaintenance/" + fileName))
                {
                    using (StreamReader reader = new StreamReader("DatabaseMaintenance/" + fileName))
                    {
                        text = reader.ReadToEnd();
                    }
                }
                return text;
            }
        }
    }
}
