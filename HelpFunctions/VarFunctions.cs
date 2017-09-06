using System;

namespace HelpFunctions
{
    public static class VarFunctions
    {

        public static int ToClarion(DateTime time)
        {
            DateTime dt = new DateTime(1800, 12, 28, 0, 0, 0);
            return (time - dt).Days;
        }

        //data w formacie yyyy-mm-dd
        public static int ToClarion(string date)
        {
            DateTime inputDate = DateTime.Parse(date);
            DateTime dt = new DateTime(1800, 12, 28, 0, 0, 0);
            return (inputDate - dt).Days;
        }

        public static int FromXLFormatToInt(string liczba)
        {
            liczba = liczba.Replace('.', ',');
            return Convert.ToInt32(Convert.ToDouble(liczba));
        }

        public static double FromXLFormatToDouble(string liczba)
        {
            liczba = liczba.Replace('.', ',');
            return Convert.ToDouble(liczba);
        }
        
    }
}
