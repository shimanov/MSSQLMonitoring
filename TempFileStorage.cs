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
        public bool CreateFile(string name, string text)
        {
            using (IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetStore(IsolatedStorageScope.User
                | IsolatedStorageScope.Domain
                | IsolatedStorageScope.Assembly, null, null))
            {
                try
                {
                    isolatedStorage.CreateFile("DatabaseMaintenance/" + name);
                    using (StreamWriter writer = new StreamWriter("DatabaseMaintenance/" + name))
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
        public bool DeleteFile(string name)
        {
            return true;
        }

        public string Read()
        {
            return "";
        }
    }
}
