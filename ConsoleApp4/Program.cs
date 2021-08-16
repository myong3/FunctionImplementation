using System;

namespace ConsoleApp4
{
    class Program
    {
        /// <summary>
        /// 測試  C# 7.0 區域函式
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var x = a();
            Console.WriteLine(x);
        }

        private static string a()
        {
            var b = CheckType_TRN1003("1");

            return b;

            string CheckType_TRN1003(string type)
            {
                var result = "0";
                if (type == "1" || type == "2" || type == "3" || type == "4" || type == "5")
                {
                    result = "1";
                }

                return result;
            }
        }
    }
}
