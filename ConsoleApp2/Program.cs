using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace ConsoleApp2
{
    /// <summary>
    /// 測試string相加 各方法 效能
    /// </summary>
    public class Program
    {

        private static StringBuilder globalSb = new StringBuilder();
        static async Task Main()
        {
            var a = new List<string>();
            for (int i = 0; i < 25000; i++)
            {
                a.Add("123");
            }


            Stopwatch sw = new Stopwatch();
            int times = 10000000;
            //sw.Start();
            //for (int i = 0; i < times; i++)
            //    GetSqlString_1();
            //sw.Stop();
            //Console.WriteLine(string.Format("Test1={0}ms", sw.ElapsedMilliseconds));
            //sw.Reset();
            //sw.Start();
            //for (int i = 0; i < times; i++)
            //    GetSqlString_2();
            //sw.Stop();
            //Console.WriteLine(string.Format("Test2={0}ms", sw.ElapsedMilliseconds));
            //sw.Reset();
            //sw.Start();
            //for (int i = 0; i < times; i++)
            //    GetSqlString_3();
            //sw.Stop();
            //Console.WriteLine(string.Format("Test3={0}ms", sw.ElapsedMilliseconds));

            //sw.Reset();
            var text = string.Empty;
            //sw.Start();
            //for (int i = 0; i < times; i++)
            //    text = GetSqlString_1();
            //sw.Stop();
            //Console.WriteLine(string.Format("Test1={0}ms", sw.ElapsedMilliseconds));
            //Console.WriteLine("Test1=" + text);

            //sw.Reset();
            sw.Start();
            for (int i = 0; i < times; i++)
                text = GetSqlString_2();
            sw.Stop();
            Console.WriteLine(string.Format("Test2={0}ms", sw.ElapsedMilliseconds));
            //Console.WriteLine("Test2=" + text);

            sw.Reset();
            sw.Start();
            for (int i = 0; i < times; i++)
                text = GetSqlString_3();
            sw.Stop();
            Console.WriteLine(string.Format("Test3={0}ms", sw.ElapsedMilliseconds));
            ////Console.WriteLine("Test3=" + text);

            //sw.Reset();
            //sw.Start();
            //for (int i = 0; i < times; i++)
            //    text = GetSqlString_4(a);
            //sw.Stop();
            //Console.WriteLine(string.Format("Test4={0}ms", sw.ElapsedMilliseconds));
            //Console.WriteLine("Test4=" + text);

            //sw.Reset();
            //sw.Start();
            //for (int i = 0; i < times; i++)
            //    text = GetCombinedData_3(a);
            //sw.Stop();
            //Console.WriteLine(string.Format("Test5={0}ms", sw.ElapsedMilliseconds));
            ////Console.WriteLine("Test5=" + text);

            //sw.Reset();
            //sw.Start();
            //for (int i = 0; i < times; i++)
            //    text = GetSqlString_6(a);
            //sw.Stop();
            //Console.WriteLine(string.Format("Test6={0}ms", sw.ElapsedMilliseconds));
            ////Console.WriteLine("Test6=" + text);

            //sw.Reset();
            //sw.Start();
            //for (int i = 0; i < times; i++)
            //    text = GetSqlString_7(a);
            //sw.Stop();
            //Console.WriteLine(string.Format("Test7={0}ms", sw.ElapsedMilliseconds));
            ////Console.WriteLine("Test7=" + text);

            //sw.Reset();
            //sw.Start();
            //for (int i = 0; i < times; i++)
            //    text = GetSqlString_8(a);
            //sw.Stop();
            //Console.WriteLine(string.Format("Test8={0}ms", sw.ElapsedMilliseconds));
            //Console.WriteLine("Test8=" + text);
            return;
        }

        static string GetSqlString_1(IReadOnlyList<string> data)
        {
            bool isFirst = true;
            var sqlCmd = new StringBuilder();

            foreach (var item in data)
            {
                if (!isFirst)
                {
                    sqlCmd.Append(", ");
                }
                isFirst = false;
                sqlCmd.Append("\"");
                sqlCmd.Append(item);
                sqlCmd.Append("\"");
            }
            return sqlCmd.ToString();
        }

        static string GetCombinedData_2(IReadOnlyList<string> data)
        {
            var isFirst = true;
            var sb = new StringBuilder();

            foreach (var item in data)
            {
                if (!isFirst)
                {
                    sb.Append(", ");
                    sb.Append("\"");
                    sb.Append(item);
                    sb.Append("\"");
                }
                else
                {
                    sb.Append("\"");
                    sb.Append(item);
                    sb.Append("\"");
                    isFirst = false;
                }
            }
            return sb.ToString();
        }

        static string GetSqlString_3(IReadOnlyList<string> data)
        {
            var isFirstItem = true;
            var sqlCmd = new StringBuilder();

            foreach (var item in data)
            {
                if (!isFirstItem)
                {
                    sqlCmd.Append(", \"");
                    sqlCmd.Append(item);
                    sqlCmd.Append("\"");
                }
                else
                {
                    sqlCmd.Append("\"");
                    sqlCmd.Append(item);
                    sqlCmd.Append("\"");
                    isFirstItem = false;
                }
            }
            return sqlCmd.ToString();
        }

        static string GetSqlString_4(IReadOnlyList<string> data)
        {
            bool isFirst = true;
            var sqlCmd = new StringBuilder();

            foreach (var item in data)
            {
                if (!isFirst)
                {
                    sqlCmd.Append(", \"" + item + "\"");
                }
                else
                {
                    sqlCmd.Append("\"" + item + "\"");
                    isFirst = false;
                }
            }
            return sqlCmd.ToString();
        }

        static string GetCombinedData_3(IReadOnlyList<string> data)
        {
            bool isFirstItem = true;
            globalSb.Clear();

            foreach (var item in data)
            {
                if (!isFirstItem)
                {
                    globalSb.Append(", \"");
                    globalSb.Append(item);
                    globalSb.Append("\"");
                }
                else
                {
                    globalSb.Append("\"");
                    globalSb.Append(item);
                    globalSb.Append("\"");
                    isFirstItem = false;
                }
            }
            return globalSb.ToString();
        }

        static string GetSqlString_6(IReadOnlyList<string> data)
        {
            var a = @"""" + string.Join(@""", """, data) + @"""";
            return a;
        }

        static string GetSqlString_7(IReadOnlyList<string> data)
        {
            var b = string.Format(@"""{0}""", string.Join(@""", """, data));
            return b;
        }

        static string GetSqlString_8(IReadOnlyList<string> data)
        {
            var combinedData = new StringBuilder();
            foreach (var columnName in data) combinedData.Append($@"""{columnName}"",");
            return combinedData.ToString().TrimEnd(',');

        }


        static string GetSqlString_1()
        {
            StringBuilder sqlCmd;
            sqlCmd = new StringBuilder("");
            sqlCmd.Append(
        "select customer.customername || ' ' as cstname,vendor.vbename || ' ' as vbename,");
            sqlCmd.Append(" tradepara.cparavalue || ' ' as Bank,");
            sqlCmd.Append(" vendor.brkcap ||' '|| vendor.brkcapvalue as CCASS,");
            sqlCmd.Append(" contract.tradername ||'      '|| contract.traderphone as Person,");
            sqlCmd.Append(" contract.bankno || ' ' as BIC,contract.traderemail || ' ' as email");
            sqlCmd.Append(" from sbtrade");
            sqlCmd.Append(" left join customer on sbtrade.corpid = customer.corpid");
            sqlCmd.Append(" and sbtrade.customerid = customer.customerid");
            sqlCmd.Append(" left join vendor on sbtrade.corpid = vendor.corpid");
            sqlCmd.Append(" and sbtrade.secbrkid = vendor.secbrkid");
            sqlCmd.Append(" left join tradepara on sbtrade.corpid = tradepara.corpid");
            sqlCmd.Append(" and tradepara.tradeid=:tradeid");
            sqlCmd.Append(" and sbtrade.customerid = tradepara.customerid");
            sqlCmd.Append(" and tradepara.cparaid='CUSTBANKNAME'");
            sqlCmd.Append(" left join contract on sbtrade.corpid = contract.corpid");
            sqlCmd.Append(" and sbtrade.mktcodeid = contract.mktcodeid");
            sqlCmd.Append(" and sbtrade.secbrkid = contract.secbrkid");
            sqlCmd.Append(" and contract.sbktype='CSI'");
            sqlCmd.Append(" and sbtrade.secaccount = contract.secaccount");
            sqlCmd.Append(" where sbtrade.corpid=:gCorpId and sbtrade.tradetype='BS'");
            sqlCmd.Append(" and to_char(sbtrade.tradedate,'yyyy/MM/dd') = :tradedate");
            sqlCmd.Append(" and sbtrade.mktcodeid=:mktcodeid");
            sqlCmd.Append(" and sbtrade.secbrkid = :secbrkid");
            sqlCmd.Append(" and sbtrade.secaccount = :secaccount");
            sqlCmd.Append(" and sbtrade.customerid = :customerid");

            return sqlCmd.ToString();
        }

        static string GetSqlString_2()
        {
            string sql =
        "select customer.customername || ' ' as cstname,vendor.vbename || ' ' as vbename," +
            " tradepara.cparavalue || ' ' as Bank," +
            " vendor.brkcap ||' '|| vendor.brkcapvalue as CCASS," +
            " contract.tradername ||'      '|| contract.traderphone as Person," +
            " contract.bankno || ' ' as BIC,contract.traderemail || ' ' as email" +
            " from sbtrade" +
            " left join customer on sbtrade.corpid = customer.corpid" +
            " and sbtrade.customerid = customer.customerid" +
            " left join vendor on sbtrade.corpid = vendor.corpid" +
            " and sbtrade.secbrkid = vendor.secbrkid" +
            " left join tradepara on sbtrade.corpid = tradepara.corpid" +
            " and tradepara.tradeid=:tradeid" +
            " and sbtrade.customerid = tradepara.customerid" +
            " and tradepara.cparaid='CUSTBANKNAME'" +
            " left join contract on sbtrade.corpid = contract.corpid" +
            " and sbtrade.mktcodeid = contract.mktcodeid" +
            " and sbtrade.secbrkid = contract.secbrkid" +
            " and contract.sbktype='CSI'" +
            " and sbtrade.secaccount = contract.secaccount" +
            " where sbtrade.corpid=:gCorpId and sbtrade.tradetype='BS'" +
            " and to_char(sbtrade.tradedate,'yyyy/MM/dd') = :tradedate" +
            " and sbtrade.mktcodeid=:mktcodeid" +
            " and sbtrade.secbrkid = :secbrkid" +
            " and sbtrade.secaccount = :secaccount" +
            " and sbtrade.customerid = :customerid";
            return sql;
        }

        static string GetSqlString_3()
        {
            string sql = @"
select customer.cstname || ' ' as customername,vendor.vbename || ' ' as vbename, 
tradepara.cparavalue || ' ' as Bank, 
vendor.brkcap ||' '|| vendor.brkcapvalue as CCASS, 
contract.tradername ||'      '|| contract.traderphone as Person, 
contract.bankno || ' ' as BIC,contract.traderemail || ' ' as email 
from sbtrade 
left join customer on sbtrade.corpid = customer.corpid 
and sbtrade.customerid = customer.customerid 
left join vendor on sbtrade.corpid = vendor.corpid 
and sbtrade.secbrkid = vendor.secbrkid 
left join tradepara on sbtrade.corpid = tradepara.corpid 
and tradepara.tradeid=:tradeid 
and sbtrade.customerid = tradepara.customerid 
and tradepara.cparaid='CUSTBANKNAME' 
left join contract on sbtrade.corpid = contract.corpid 
and sbtrade.mktcodeid = contract.mktcodeid 
and sbtrade.secbrkid = contract.secbrkid 
and contract.sbktype='CSI' 
and sbtrade.secaccount = contract.secaccount 
where sbtrade.corpid=:gCorpId and sbtrade.tradetype='BS' 
and to_char(sbtrade.tradedate,'yyyy/MM/dd') = :tradedate 
and sbtrade.mktcodeid=:mktcodeid 
and sbtrade.secbrkid = :secbrkid 
and sbtrade.secaccount = :secaccount 
and sbtrade.customerid = :customerid ";
            return sql;
        }
    }
}