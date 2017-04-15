using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZS1Planv2.Model.Application
{
    public class Text
    {
        public enum DataType
        {
            DayName,
            Hour
        }

        public enum TextId
        {
            MainPage_Loading_Text_1,
            MainPage_Downloading_Text_1,
            MainPage_Downloading_Text_2,
            MainPage_Downloading_Text_3,
            MainPage_NoInternet_Text_1,
            MainPage_NoInternet_Text_2,
            MainPage_Downloading_Error_Text_1,
            MainPage_Downloading_Error_Text_2,
            TimetablePage_MainMenuButton_Text_1,
            TimetablePage_SettingsButton_Text_1,
            TimetablePage_RefreshButton_Text_1,
            TimetablePage_Title_Text_1,
            TimetablePage_Title_Text_2,
            TimetablePage_Grid_Text_1,
            TimetablePage_Grid_Text_2,
            SettingsUserControl_Text_1,
            SettingsUserControl_Text_2,
            SettingsUserControl_Text_3,
            SettingsUserControl_Text_4,
            SettingsUserControl_Text_5,
            SettingsUserControl_Text_6
        }

        private static string[] _DayNames = new string[]
        {
            "Poniedziałek",
            "Wtorek",
            "Środa",
            "Czwartek",
            "Piątek"
        };

        private static string[] _LessonHours =
        {
            "7:10 - 7:55",
            "8:00 - 8:45",
            "8:50 - 9:35",
            "9:45 - 10:30",
            "10:45 - 11:30",
            "11:35 - 12:20",
            "12:30 - 13:15",
            "13:20 - 14:05",
            "14:10 - 14:55",
            "15:00 - 15:45",
            "15:50 - 16:35"
        };

        public static string GetText(DataType dataType, int index)
            => dataType == DataType.DayName ? _DayNames[index] : _LessonHours[index];

        public static string GetText(TextId textId)
        {
            switch (textId)
            {
                case TextId.MainPage_Loading_Text_1:
                    return "Wczytywanie...";

                case TextId.MainPage_Downloading_Text_1:
                    return "By przeglądąc plan lekcji musisz go najpierw pobrać." +
                        $"{Environment.NewLine}Czy chcesz to zrobić teraz?";
                case TextId.MainPage_Downloading_Text_2:
                    return "Pobieranie planu lekcji...";
                case TextId.MainPage_Downloading_Text_3:
                    return "Pobierz plan lekcji";

                case TextId.MainPage_NoInternet_Text_1:
                    return "Do pobrania rozkładu wymagane jest połączenie z internetem." +
                        $"{Environment.NewLine}Sprawdź je, i spróbuj ponownie.";
                case TextId.MainPage_NoInternet_Text_2:
                    return "Spróbuj ponownie";

                case TextId.MainPage_Downloading_Error_Text_1:
                    return "Wystąpił problem podczas pobierania planu lekcji." +
                        $"{Environment.NewLine}Czy chcesz spróbować ponownie?";
                case TextId.MainPage_Downloading_Error_Text_2:
                    return "Spróbuj ponownie";

                case TextId.TimetablePage_MainMenuButton_Text_1:
                    return "\xE700";
                case TextId.TimetablePage_SettingsButton_Text_1:
                    return "\xE713";
                case TextId.TimetablePage_RefreshButton_Text_1:
                    return "\xE117";

                case TextId.TimetablePage_Title_Text_1:
                    return "Plan lekcji";
                case TextId.TimetablePage_Title_Text_2:
                    return "Brak";

                case TextId.TimetablePage_Grid_Text_1:
                    return "Nr.";
                case TextId.TimetablePage_Grid_Text_2:
                    return "Godz.";

                case TextId.SettingsUserControl_Text_1:
                    return "Startowy plan lekcji:";
                case TextId.SettingsUserControl_Text_2:
                    return "Podświetl aktualne lekcje:";
                case TextId.SettingsUserControl_Text_3:
                    return "Włączone";
                case TextId.SettingsUserControl_Text_4:
                    return "Wyłączone";
                case TextId.SettingsUserControl_Text_5:
                    return "Zapisz";
                case TextId.SettingsUserControl_Text_6:
                    return "Zapisano!";

                default:
                    return "Error";
            }
        }
    }
}
