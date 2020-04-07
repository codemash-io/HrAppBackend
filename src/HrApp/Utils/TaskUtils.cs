using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HrApp
{
    public static class TaskUtils
    {
        /// <summary>
        /// Makes input word only first letter uppercase and others lowercase (meant for Enums)
        /// </summary>
        /// <param name="input"> Input word </param>
        /// <returns> Formatted word </returns>
        public static string OnlyFirstUppercase(string input)
        {
            if(String.IsNullOrEmpty(input))
                throw new ArgumentNullException();

            input = input.ToLower();
            input = input.First().ToString().ToUpper() + input.Substring(1);

            return input;
        }

        /*public static void AddEmployeesToTimeLines(TimeLine timeLine, Employee[] employees)
        {
            foreach (Employee employee in employees)
            {
                timeLine.Employees.Add(employee);
            }
        }*/
    }
}
