using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ZS1Planv2.Model.Serializer
{
    public class JSONSerializerService
    {
        private const string _FILE_NAME = "timetable.json";
        private readonly string _FILE_PATH = Path.Combine(ApplicationData.Current.LocalFolder.Path, _FILE_NAME);

        private void PrepareFile()
        {
            if (File.Exists(_FILE_PATH))
                File.Delete(_FILE_PATH);
        }

        public async Task<bool> SaveData<T>(T item)
        {
            try
            {
                PrepareFile();
                await Task.Run(() =>
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(getSerializedString(item));
                    File.Create(_FILE_PATH).Write(bytes, 0, bytes.Count())
                });
                return true;
            }
            catch
            {
                return false;
            }

            string getSerializedString(object obj)
                => JsonConvert.SerializeObject(obj);
        }

        public async Task<T> LoadData<T>()
        {
            try
            {
                if (!File.Exists(_FILE_PATH))
                    return default(T);

                return await Task.Run(() => JsonConvert.DeserializeObject<T>(getSerializedString()));
            }
            catch
            {
                return default(T);
            }

            string getSerializedString()
                => File.ReadAllText(_FILE_PATH, Encoding.UTF8);
        }
    }
}
