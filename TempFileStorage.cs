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
                if (isolated.DirectoryExists("DatabaseMaintenance"))
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

        private readonly object _readLock = new object();
        //Create file in IsolatedStorage
        public bool CreateFile(string fileName, string text)
        {
            using (IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetStore(IsolatedStorageScope.User
                | IsolatedStorageScope.Domain
                | IsolatedStorageScope.Assembly, null, null))
            {
                try
                {
                    lock (_readLock)
                    {
                        using (IsolatedStorageFileStream fileStream = new IsolatedStorageFileStream("DatabaseMaintenance/" + fileName, FileMode.CreateNew, isolatedStorage))
                        {
                            using (StreamWriter writer = new StreamWriter(fileStream))
                            {
                                writer.WriteLine(text);
                                writer.Close();
                            }
                            fileStream.Close();
                        }
                        return true;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        //Delete file in IsolatedStorage
        public bool DeleteFolder(string fileName)
        {
            using (IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetStore(IsolatedStorageScope.User
                | IsolatedStorageScope.Domain
                | IsolatedStorageScope.Assembly, null, null))
            {
                isolatedStorage.DeleteFile("DatabaseMaintenance/" + fileName);

                return true;
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
                    using (IsolatedStorageFileStream isolatedStorageFileStream = new IsolatedStorageFileStream("DatabaseMaintenance/" + fileName, FileMode.Open, isolatedStorage))
                    {
                        using (StreamReader reader = new StreamReader(isolatedStorageFileStream))
                        {
                            text = reader.ReadToEnd();
                        }
                    }
                }
                return text;
            }
        }
    }
}
