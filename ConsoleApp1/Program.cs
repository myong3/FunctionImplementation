using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    /// <summary>
    /// 驗證xml formate
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            string xmlStr = null;

            if (IsValidXML(xmlStr))
            {
                Console.WriteLine("XML is well formed");
            }
            else
            {
                Console.WriteLine("XML is not well formed");
            }
        }

        private static bool IsValidXML(string xmlStr)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(xmlStr))
                {
                    return false;
                }
                var elementList = new List<string>();
                var less = 0;
                var greater = 0;
                var isFirst = true;
                while (true)
                {
                    if (isFirst)
                    {
                        less = xmlStr.IndexOf("<") + 1;
                        greater = xmlStr.IndexOf(">") - 1;
                        if (less == 0)
                        {
                            break;
                        }
                        var element = xmlStr.Substring(less, greater - less + 1);
                        elementList.Add(element);
                        isFirst = false;
                    }
                    else
                    {
                        less = xmlStr.IndexOf("<", less) + 1;
                        greater = xmlStr.IndexOf(">", greater + 2) - 1;
                        if (less == 0)
                        {
                            break;
                        }
                        var element = xmlStr.Substring(less, greater - less + 1);

                        if (element.IndexOf("/") != -1)
                        {
                            if (element.Substring(1, element.Length - 1) == elementList[elementList.Count - 1])
                            {
                                elementList.RemoveAt(elementList.Count - 1);
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            elementList.Add(element);
                        }
                    }
                }

                return elementList.Count == 0 ? true : false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
