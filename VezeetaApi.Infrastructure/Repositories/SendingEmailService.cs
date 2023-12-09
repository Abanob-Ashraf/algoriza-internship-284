using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using VezeetaApi.Domain.Dtos;
using VezeetaApi.Domain.Helpers;
using VezeetaApi.Domain.Services;

namespace VezeetaApi.Infrastructure.Repositories
{
    public class SendingEmailService : ISendingEmailService
    {
        private readonly Mail Mail;
        public SendingEmailService(IOptions<Mail> mail)
        {
            Mail = mail.Value;
        }

        public async Task SendEmailAsync(MailMessageDTO mailMessageDTO)
        {
            var emailMessage = CreateEmailMessage(mailMessageDTO);

            SendAsync(emailMessage);
        }


        private MimeMessage CreateEmailMessage(MailMessageDTO mailMessageDTO)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(Mail.Email),
                Subject = mailMessageDTO.Subject
            };

            email.To.AddRange(mailMessageDTO.To);
            email.From.Add(new MailboxAddress(Mail.DisplayName, Mail.Email));
            string[] parts = mailMessageDTO.To[0].ToString().Split('@');

            var builder = new BodyBuilder();


            if (mailMessageDTO.Content != null)
            {
                builder.HtmlBody = string.Format(
                    "<h1>Hello, Our dear Doctor \n{1}</h1>\r\n " +
                    "<br/> &nbsp;&nbsp; {0}      <br /><br /><br/>     " +
                    "<table style=\"border-collapse: collapse;\">\r\n " +
                    "<tr> \r\n <td rowspan=\"2\"><img style=\"width: 150px;padding: 10px;text-align: left;\"\r\n" +
                    "src='https://gist.github.com/Abanob-Ashraf/dd294306ccb3853939d43b27e09c4ce5/raw/253ea6dd2890287402afad40caf74a540fd6ad42/mage-from-rawpixel-id-2301904-png.png' />\r\n" +
                    " </td>\r\n                <td style=\"font-weight: bold;padding: 10px;text-align: left;\"> Phone: 01142601607</td>\r\n" +
                    " </tr>\r\n            <tr>\r\n                <td style=\"font-weight: bold;padding: 10px;text-align: left;\">" +
                    "Email: abanobashraf74@gmail.com </td>\r\n </tr>\r\n        </table>",
                    mailMessageDTO.Content, parts[0].Substring(1));

                var attachments = mailMessageDTO.Attachments;

                if (mailMessageDTO.Attachments is not null)
                {
                    byte[] fileBytes;

                    foreach (var file in attachments)
                    {
                        if (file.Length > 0)
                        {
                            using var ms = new MemoryStream();
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();

                            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                        }
                    }
                }

                email.Body = builder.ToMessageBody();
            }
            else
            {
                builder.HtmlBody = string.Format(
                    " <h1>Welcome To Vezeeta {0}</h1>\r\n" +
                    "<br /><br /><br/>  <img style=\"width: 150px;padding: 10px;text-align: left;\"\r\n" +
                    "src='https://gist.github.com/Abanob-Ashraf/dd294306ccb3853939d43b27e09c4ce5/raw/253ea6dd2890287402afad40caf74a540fd6ad42/05136568360535.5b59f63c4594f.png' />\r\n" +
                    "</td>\r\n                <td style=\"font-weight: bold;padding: 10px;text-align: left;\"> Phone: 01142601607</td>\r\n" +
                    "</tr>\r\n            <tr>\r\n                <td style=\"font-weight: bold;padding: 10px;text-align: left;\">" +
                    "Email: abanobashraf74@gmail.com </td>\r\n            </tr>\r\n        </table>", 
                    parts[0].Substring(1));
                email.Body = builder.ToMessageBody();
            }
            return email;
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            using var smtp = new SmtpClient();
            try
            {
                smtp.Connect(Mail.SmtpServer, Mail.Port, true);
                smtp.AuthenticationMechanisms.Remove("XOAUTH2");
                smtp.Authenticate(Mail.Email, Mail.Password);
                await smtp.SendAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                await smtp.DisconnectAsync(true);
                smtp.Dispose();
            }
        }
    }
}
