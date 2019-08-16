using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.Utils
{
    public static class DateHelper
    {
        public static string GetLongDate(string input)//Date Helper Method
        {
            if (input is "Today")
            {
                DateTime dateNow = DateTime.Now;
                string date = dateNow.ToString("dd/MM/yyyy");

                return date;
            }
            else if (input.Contains("Today+"))
            {
                string days = input.Substring(input.LastIndexOf("+"));
                int newInput = Convert.ToInt16(days);
                DateTime dateNow = DateTime.Now;
                DateTime newDate = dateNow.AddDays(newInput);
                string date = newDate.ToString("dd/MM/yyyy");

                return date;
            }
            else if (input.Contains("Today-"))
            {
                string days = input.Substring(input.LastIndexOf("-"));
                int newInput = Convert.ToInt16(days);
                DateTime dateNow = DateTime.Now;
                int negDays = -System.Math.Abs(newInput);
                DateTime newDate = dateNow.AddDays(negDays);
                string date = newDate.ToString("dd/MM/yyyy");

                return date;
            }
            else if (input == "NULL")
            {
                return "NULL";
            }
            else
            {
                return input;
            }
        }
    }
}
