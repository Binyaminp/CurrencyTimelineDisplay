using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using HtmlAgilityPack;

namespace CsExercise
{
    public class CurrencyFileMgr
    {
        public static List<DailyCurrency> GetCurrenciesCsv(string csvFilePath)
        {
            return File.ReadAllLines(csvFilePath).Skip(1).Select(line => DailyCurrency.FromStr(line)).ToList();
        }

        public static List<DailyCurrency> GetCurrenciesWeb(int curNum, DateTime startDate, DateTime endDate)
        {
            List<DailyCurrency> dailyCurrencies = new List<DailyCurrency>();
            try
            {
                string html = string.Empty;
                string url = @"https://www.boi.org.il/he/_layouts/boi/handlers/WebPartHandler.aspx?wp=ExchangeRates&lang=he-IL&Currency=" +
                    curNum + "&DateStart=" + startDate.Day + "%2F" + startDate.Month + "%2F" + startDate.Year +
                    "&DateEnd=" + endDate.Day + "%2F" + endDate.Month + "%2F" + endDate.Year;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    html = reader.ReadToEnd();
                }


                HtmlDocument doc = new HtmlDocument();

                doc.LoadHtml(html);


                var rows = doc.DocumentNode.SelectNodes("//div[@id='BoiExchangeRateSearceResults']/table/tr");


                for (int i = 1; i < rows.Count; i++)
                {
                    string curStr = rows[i].ChildNodes[1].InnerText + "," + rows[i].ChildNodes[3].InnerText;

                    dailyCurrencies.Add(DailyCurrency.FromStr(curStr));
                }

            }
            catch (Exception) { }
            return dailyCurrencies;
        }

        public static Dictionary<int, string> GetCurrencies()
        {
            string html = string.Empty;
            string url = @"https://www.boi.org.il/he/_layouts/boi/handlers/WebPartHandler.aspx?wp=ExchangeRates&lang=he-IL&Currency=3&DateStart=25%2F09%2F2017&DateEnd=25%2F09%2F2019";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }


            HtmlDocument doc = new HtmlDocument();

            doc.LoadHtml(html);


            var rows = doc.DocumentNode.SelectNodes("//select[@id='selCurrency']/option");

            Dictionary<int, string> currencies = new Dictionary<int, string>();

            for (int i = 0; i < rows.Count; i++)
            {
                if (rows[i].Attributes.Count == 3)
                {
                    currencies.Add(int.Parse(rows[i].Attributes[2].Value), rows[i].Attributes[0].DeEntitizeValue);
                }
                else
                {
                    currencies.Add(int.Parse(rows[i].Attributes[1].Value), rows[i].Attributes[0].DeEntitizeValue);
                }
            }

            return currencies;
        }
    }
}
