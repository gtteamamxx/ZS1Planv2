using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ZS1Planv2.Model.Application
{
    public class SettingsManager
    {
        public enum SettingsType
        {
            Start_Plan,
            Hightlight_Lessons
        }

        private static string GetEnumName(SettingsType type)
            => Enum.GetName(typeof(SettingsType), type);

        public static bool KeyExist(SettingsType type)
            => ApplicationData.Current.LocalSettings.Values.ContainsKey(GetEnumName(type));

        public static void SetSetting(SettingsType type, object value)
        {
            string key = GetEnumName(type);
            if (KeyExist(type))
                ApplicationData.Current.LocalSettings.Values.Remove(key);
            
            ApplicationData.Current.LocalSettings.Values.Add(new KeyValuePair<string, object>(key, value));
        }
        public static T GetSetting<T>(SettingsType type)
        {
            if (!KeyExist(type))
                return default(T);
            string key = GetEnumName(type);
            return ((T)ApplicationData.Current.LocalSettings.Values[key]);
        }
    }
}
