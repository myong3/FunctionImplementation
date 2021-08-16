using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Polly;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleApp5
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var get = getStream();

            var lists = new List<EmailAttachmentModel>();
            var list = new EmailAttachmentModel()
            {
                AttachmentName = "中文.pdf",
                AttachmentContent = get
            };

            var list2 = new EmailAttachmentModel()
            {
                AttachmentName = "2.pdf",
                AttachmentContent = get
            };

            lists.Add(list);
            lists.Add(list2);

            var a = await FailResponse3(lists);
        }

        private static async Task<bool> test2()
        {
            var result = false;
            try
            {
                var retryPolicy2 = Policy.Handle<Exception>()
                                        .WaitAndRetryAsync(new[]
                                              {
                                              TimeSpan.FromSeconds(2),
                                              TimeSpan.FromSeconds(2),
                                              TimeSpan.FromSeconds(2)
                                              },
                                              (response, retryTime, retryCount, context) =>
                                              {
                                                  if (retryCount == 1)
                                                  {
                                                      Console.WriteLine($"EmailService-SendMailThread: (收件人:)，信件寄送失敗。");
                                                      Console.WriteLine($"EmailService-SendMailThread: (收件人:)於2秒後重新寄信");
                                                  }
                                                  else
                                                  {
                                                      Console.WriteLine($"EmailService-SendMailThread: (收件人:)，第{retryCount - 1}次重新寄送失敗。");
                                                      Console.WriteLine($"EmailService-SendMailThread: (收件人:)於2秒後重新寄信");
                                                  }
                                              });


                result = await retryPolicy2.ExecuteAsync(async () => await FailResponse2());


                Console.WriteLine("延遲重試，完成");

                return result;
            }
            catch (Exception ex)
            {

                return result;
            }
        }

        /// <summary>
        /// 寄email - 含附件(本地本機)
        /// </summary>
        /// <returns></returns>
        private static async Task<bool> FailResponse2()
        {
            try
            {
                var a = new List<string>();
                a.Add("myong333@icloud.com");

                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress("123444", "veve0331@gmail.com"));
                foreach (var item in a)
                {
                    mimeMessage.To.Add(MailboxAddress.Parse(item));
                }
                mimeMessage.Subject = "EMAIL_Subject";

                var builder = new BodyBuilder();
                builder.HtmlBody = "<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /></head><body leftmargin='0' topmargin='0' marginwidth='0' marginheight='0' width='700' ><table width='650px' border='0' cellpadding='0' cellspacing='0'><tr></tr></table><table width='650px' border='0' cellpadding='0' cellspacing='0' class='Context_tb' ><tr><td class='Context_td01' align='left'>親愛的顧客，您好：<br><br>您於網路郵局終止約定帳戶之申請已完成，該約定帳戶已失效。請詳閱附檔，檢視終止帳戶之資料。<br><br></td></tr><tr><td class='Context_td01' align='left'>提醒您，附檔已設定為加密文件，<font color='red'>開啟密碼為您的身分證字號(大寫英文字母+9位數字；外籍人士為居留證號碼：兩個大寫英文字母+8位數字)。</font><br><br></td></tr><tr><td class='Context_td01' align='left'>若您無法看到電子郵件通知單內容，請至Adobe Acrobat官網下載Adobe Reader軟體並安裝。<br><br></td></tr><tr><td class='Context_td01' align='left'>如需任何協助，請隨時致電本公司24小時客戶服務中心0800-700-365，手機請改撥付費電話(04)23542030。<br><br>本信件由系統發出，請勿直接回覆此信。</td></tr></td></tr></table></body></html>";
                var attachmentPath2 = @"D:\Chih-Yun_Gina_CV.pdf";

                // We may also want to attach a calendar event for Monica's party...
                if (!System.IO.File.Exists(attachmentPath2))
                {
                    Console.WriteLine($"attachmentPath");
                }

                builder.Attachments.Add(attachmentPath2);

                // Now we just need to set the message body and we're done
                mimeMessage.Body = builder.ToMessageBody();

                var attachment = mimeMessage.Attachments.First();

                var fileName = attachment.ContentDisposition?.FileName;
                //var rfc822 = (MessagePart)attachment;
                mimeMessage.Attachments.First().ContentDisposition.FileName = "12345.pdf";
                //if (string.IsNullOrEmpty(fileName))
                //    fileName = "attached-message.pdf";

                //using (var stream = File.Create(fileName))
                //    rfc822.Message.WriteTo(stream);


                var hh = 1;
                using (SmtpClient smtpClient = new SmtpClient())
                {
                    await smtpClient.ConnectAsync("smtp.gmail.com", 465, true);
                    await smtpClient.AuthenticateAsync("veve0331@gmail.com", "rqnaqrtjjyygujyg");
                    await smtpClient.SendAsync(mimeMessage);
                    await smtpClient.DisconnectAsync(true);
                }

                return true;
            }
            catch (AuthenticationException ex)
            {
                Console.WriteLine($"Invalid user name or password. Message: {ex.Message}");
                throw;
            }
            catch (SmtpCommandException ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
                Console.WriteLine($"\tStatusCode: {ex.StatusCode}");

                switch (ex.ErrorCode)
                {
                    case SmtpErrorCode.RecipientNotAccepted:
                        Console.WriteLine($"\tRecipient not accepted: {ex.Mailbox}");
                        break;
                    case SmtpErrorCode.SenderNotAccepted:
                        Console.WriteLine($"\tSender not accepted: {ex.Mailbox}");
                        break;
                    case SmtpErrorCode.MessageNotAccepted:
                        Console.WriteLine($"\tMessage not accepted.");
                        break;
                }
                throw;

            }
            catch (SmtpProtocolException ex)
            {
                Console.WriteLine($"Protocol error while sending message: {ex.Message}");
                throw;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private static Stream getStream()
        {
            string pdfFilePath = @"D:\Chih-Yun_Gina_CV.pdf";

            var pdfContent = new MemoryStream(System.IO.File.ReadAllBytes(pdfFilePath));
            return pdfContent;
        }
        /// <summary>
        /// 寄email - 含附件(byte[])
        /// </summary>
        /// <returns></returns>
        private static async Task<bool> FailResponse3(List<EmailAttachmentModel> pdfContent)
        {
            Console.WriteLine($"Execute EmailService-SendEmailWithAttachmentPathAsync {new { attachmentPath = JsonSerializer.Serialize(pdfContent.Select(x => x.AttachmentName)) }}");
            try
            {
                var a = new List<string>();
                a.Add("myong333@icloud.com");

                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress("123444", "veve0331@gmail.com"));
                foreach (var item in a)
                {
                    mimeMessage.To.Add(MailboxAddress.Parse(item));
                }
                mimeMessage.Subject = "EMAIL_Subject";

                var builder = new BodyBuilder();
                builder.HtmlBody = "<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /></head><body leftmargin='0' topmargin='0' marginwidth='0' marginheight='0' width='700' ><table width='650px' border='0' cellpadding='0' cellspacing='0'><tr></tr></table><table width='650px' border='0' cellpadding='0' cellspacing='0' class='Context_tb' ><tr><td class='Context_td01' align='left'>親愛的顧客，您好：<br><br>您於網路郵局終止約定帳戶之申請已完成，該約定帳戶已失效。請詳閱附檔，檢視終止帳戶之資料。<br><br></td></tr><tr><td class='Context_td01' align='left'>提醒您，附檔已設定為加密文件，<font color='red'>開啟密碼為您的身分證字號(大寫英文字母+9位數字；外籍人士為居留證號碼：兩個大寫英文字母+8位數字)。</font><br><br></td></tr><tr><td class='Context_td01' align='left'>若您無法看到電子郵件通知單內容，請至Adobe Acrobat官網下載Adobe Reader軟體並安裝。<br><br></td></tr><tr><td class='Context_td01' align='left'>如需任何協助，請隨時致電本公司24小時客戶服務中心0800-700-365，手機請改撥付費電話(04)23542030。<br><br>本信件由系統發出，請勿直接回覆此信。</td></tr></td></tr></table></body></html>";

                foreach (var item in pdfContent)
                {
                    builder.Attachments.Add(item.AttachmentName, item.AttachmentContent);
                }

                // Now we just need to set the message body and we're done
                mimeMessage.Body = builder.ToMessageBody();

                using (SmtpClient smtpClient = new SmtpClient())
                {
                    await smtpClient.ConnectAsync("smtp.gmail.com", 465, true);
                    await smtpClient.AuthenticateAsync("veve0331@gmail.com", "rqnaqrtjjyygujyg");
                    await smtpClient.SendAsync(mimeMessage);
                    await smtpClient.DisconnectAsync(true);
                }

                return true;
            }
            catch (AuthenticationException ex)
            {
                Console.WriteLine($"Invalid user name or password. Message: {ex.Message}");
                throw;
            }
            catch (SmtpCommandException ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
                Console.WriteLine($"\tStatusCode: {ex.StatusCode}");

                switch (ex.ErrorCode)
                {
                    case SmtpErrorCode.RecipientNotAccepted:
                        Console.WriteLine($"\tRecipient not accepted: {ex.Mailbox}");
                        break;
                    case SmtpErrorCode.SenderNotAccepted:
                        Console.WriteLine($"\tSender not accepted: {ex.Mailbox}");
                        break;
                    case SmtpErrorCode.MessageNotAccepted:
                        Console.WriteLine($"\tMessage not accepted.");
                        break;
                }
                throw;

            }
            catch (SmtpProtocolException ex)
            {
                Console.WriteLine($"Protocol error while sending message: {ex.Message}");
                throw;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public class EmailAttachmentModel
        {
            /// <summary>
            /// 附件名稱
            /// </summary>
            public string AttachmentName { get; set; }

            /// <summary>
            /// 附件內容
            /// </summary>
            public Stream AttachmentContent { get; set; }
        }
    }
}
