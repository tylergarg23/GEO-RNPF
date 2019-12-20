namespace SERFOR.Component.Tools.DateManager
{
    public static class Month
    {
        public static string GetMonthName(int month)
        {
            string[] months = { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };

            return months[month - 1];
        }

        //public static List<Month> GetMonthName(int month)
        //{


        //    return new List<MesDTe>()
        //    {
        //        new Month{NumberMonth = 1, MonthName = "ENERO"},
        //        new Month{NumberMonth = 2, MonthName = "FEBRERO"},
        //        new Month{NumberMonth = 3, MonthName = "MARZO"},
        //        new Month{NumberMonth = 4, MonthName = "ABRIL"},
        //        new Month{NumberMonth = 5, MonthName = "MAYO"},
        //        new Month{NumberMonth = 6, MonthName = "JUNIO"},
        //        new Month{NumberMonth = 7, MonthName = "JULIO"},
        //        new Month{NumberMonth = 8, MonthName = "AGOSTO"},
        //        new Month{NumberMonth = 9, MonthName = "SEPTIEMBRE"},
        //        new Month{NumberMonth = 10, MonthName = "OCTUBRE"},
        //        new Month{NumberMonth = 11, MonthName = "NOVIEMBRE"},
        //        new Month{NumberMonth = 12, MonthName = "DICIEMBRE"}
        //    };
        //}
    }

}
