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
            Loading_Text_1,
            Downloading_Text_1,
            Downloading_Text_2,
            Downloading_Text_3,
            NoInternet_Text_1,
            NoInternet_Text_2,
            Downloading_Error_Text_1,
            Downloading_Error_Text_2
        }

        public static string GetText(TextId textId)
        {
            switch (textId)
            {
                case TextId.Loading_Text_1:
                    return "Wczytywanie...";

                case TextId.Downloading_Text_1:
                    return "By przeglądąc plan lekcji musisz go najpierw pobrać." +
                        $"{Environment.NewLine}Czy chcesz to zrobić teraz?";
                case TextId.Downloading_Text_2:
                    return "Pobieranie planu lekcji...";
                case TextId.Downloading_Text_3:
                    return "Pobierz plan lekcji";

                case TextId.NoInternet_Text_1:
                    return "Do pobrania rozkładu wymagane jest połączenie z internetem." +
                        $"{Environment.NewLine}Sprawdź je, i spróbuj ponownie.";
                case TextId.NoInternet_Text_2:
                    return "Spróbuj ponownie";

                case TextId.Downloading_Error_Text_1:
                    return "Wystąpił problem podczas pobierania planu lekcji." +
                        $"{Environment.NewLine}Czy chcesz spróbować ponownie?";
                case TextId.Downloading_Error_Text_2:
                    return "Spróbuj ponownie";

                default:
                    return "Error";
            }
        }
    }
}
