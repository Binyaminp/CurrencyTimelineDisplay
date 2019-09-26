using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsExercise
{
    public class DailyCurrency
    {
        public DateTime day { get; set; }
        public double currency { get; set; }

        public static DailyCurrency FromStr(string strLine)
        {
            var splitLine = strLine.Split(',');

            return new DailyCurrency()
            {
                day = DateTime.ParseExact(splitLine[0], "dd/MM/yyyy", CultureInfo.InvariantCulture),
                currency = Double.Parse(splitLine[1])
            };
        }
    }
}
