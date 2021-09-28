using System;
using System.IO;
using WkHtmlToPdfDotNet;

namespace ConsoleApp7
{
    class Program
    {
        /// <summary>
        /// 製作Html轉PDF
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try
            {
                var fORMATTED_TIME = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                var f_bankFullName = "700 - 中華郵政";
                var f_trfeeActNo = "12345678901234";
                var f_nickName = "我的別名";

                string relativePath = "./Font/";
                string contentRootPath = "C:/Users/myong/source/repos/ConsoleApp2/ConsoleApp7";
                string fullcontentRootPath = Path.GetFullPath(relativePath, contentRootPath);


                string content = @"
                <style>
                @font-face {
                  font-family: 'Noto';
                  font-style: normal;
                  font-weight: 400;
                  src: url(data:" + fullcontentRootPath + @"NotoSansAvestan-Regular.eot);
                  src: url(data:" + fullcontentRootPath + @"NotoSansAvestan-Regular.eot?#iefix) format('embedded-opentype'),
                       url(data:" + fullcontentRootPath + @"NotoSansAvestan-Regular.woff2) format('woff2'),
                       url(data:" + fullcontentRootPath + @"NotoSansAvestan-Regular.woff) format('woff'),
                       url(data:" + fullcontentRootPath + @"NotoSansAvestan-Regular.ttf) format('truetype');
             }
                .notice-block {
                  border: 1px solid #000;
                  padding: 20px;
                  letter-spacing: 1px;
                  font-family: 'Noto', sans-serif;
                }
                .notice-block h3 {
                  margin-top: 0;
                  text-align: center;
                  font-size: 1.5rem;
                }
                .notice-block .form-style {
                  list-style: none;
                  padding-left: 0;
                }
                .notice-block .form-style li {
                  display: flex;
                }
                .notice-block .form-style li .title {
                  display: inline-block;
                  width: 13em;
                  text-align: right;
                  margin-right: 5px;
                }
                .notice-block .form-style li .title:after {
                  content: '：';
                }
                .notice-block .form-style li span:last-child {
                  width: 20em;
                  border-bottom: 1px solid #000;
                  display: inline-block;
                }
                .notice-block .ps-txt {
                  position: relative;
                  padding-left: 1.2rem;
                }
                .notice-block .ps-txt:before {
                  content: '※';
                  position: absolute;
                  left: 0;
                  top: 0;
                }
                .notice-block ul {
                  list-style: decimal;
                }
                .notice-block .foot-txt {
                  text-align: right;
                }
                </style>
                <div class='notice-block'>
                  <h3>線上終止約定轉入帳戶通知</h3>
                  <p>親愛的客戶您好：</p>
                  <p>您已於 " + fORMATTED_TIME + @" 於本公司網路郵局終止約定轉入帳號如下：</p>
                  <ul class='form-style'>
                    <li><span class='title'>轉帳業務別</span><span> 網路郵局及e動郵局  </span></li>
                    <li><span class='title'>終止轉入銀行代號及名稱</span><span>" + f_bankFullName + @"</span></li>
                    <li><span class='title'>终止轉入銀行帳號</span><span>" + f_trfeeActNo + @"</span></li>
                    <li><span class='title'>別名</span><span>" + f_nickName + @"</span></li>
                  </ul>
                  <p class='ps-txt'>終止後之約定帳戶如須回復，除本人親自攜帶國民身分證、儲金簿及原印鑑至任一郵局(非通儲戶親至立帳局)辦理外，如已攜帶前述證件至郵局啟用「線上設定約定轉入帳戶」服務，亦可於網路郵局線上重新設定：</p>
                  <ul>
                    <li>「臨櫃設定約定轉入帳戶」轉帳每次最高限額為新臺幣100萬元，每日最高限額(含跨行、非跨行)為新臺幣100萬元。</li>
                    <li>「線上設定約定轉入帳戶」轉帳每次最高限額為新臺幣5萬元，每日最高限額(含跨行、非跨行)為新臺幣10萬元，每月最高限額(含跨行、非跨行)為新臺幣20萬元。</li>
                  </ul>
                  <p class='foot-txt'>中華郵政股份有限公司 敬上</p>
                </div>";


                var doc = new HtmlToPdfDocument()
                {
                    GlobalSettings =
                    {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings { Top = 20, Left = 10, Right = 10, Bottom= 20 }
                    },
                    Objects =
                    {
                        new ObjectSettings
                            {
                                PagesCount = true,
                                HtmlContent = content,
                                WebSettings = new WebSettings
                                {
                                    PrintMediaType = true,
                                    EnableIntelligentShrinking = true,
                                    DefaultEncoding = "utf-8"
                                }
                            }
                    }
                };

                var converter = new BasicConverter(new PdfTools());

                byte[] bytes = converter.Convert(doc);

                if (!Directory.Exists("D:\\"))
                {
                    Directory.CreateDirectory("D:\\");
                }

                using (FileStream stream = new FileStream(@"D:\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf", FileMode.Create))
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

            }
        }
    }
}
