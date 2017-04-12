using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZS1Planv2.Model.Application
{
    public class Text
    {
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
            TimetablePage_Title_Text_1,
            TimetablePage_Title_Text_2,
            TimetablePage_Grid_Text_1,
            TimetablePage_Grid_Text_2
        }

        private static string[] _DayNames = new string[]
        {
            "Poniedziałek",
            "Wtorek",
            "Środa",
            "Czwartek",
            "Piątek"
        };

        public static string GetText(int dayId)
            => _DayNames[dayId];

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
                case TextId.TimetablePage_Title_Text_1:
                    return "Plan lekcji";
                case TextId.TimetablePage_Title_Text_2:
                    return "Brak";

                case TextId.TimetablePage_Grid_Text_1:
                    return "Nr.";
                case TextId.TimetablePage_Grid_Text_2:
                    return "Godz.";

                default:
                    return "Error";
            }
        }
    }
}
