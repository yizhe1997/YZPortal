using Application.Interfaces.Services.Mailing;
using Application.Requests;
using Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.Services.Mailing
{
    // TODO: Tos, Bccs, and CCs consider the display name
    public class SendGridMailService : IMailService
    {
        private readonly MailConfig _mailConfig;
        private readonly ISendGridClient _sendGridClient;

        public SendGridMailService(IOptions<MailConfig> mailConfig, ISendGridClient sendGridClient)
        {
            _mailConfig = mailConfig.Value;
            _sendGridClient = sendGridClient;
        }

        public async Task SendAsync(CreateMailCommand request)
        {
            //    var message = new SendGridMessage();

            //    message.Personalizations = new List<Personalization>(){
            //    new Personalization(){
            //        Tos = new List<EmailAddress>(){
            //            new EmailAddress(){
            //                Email = "john_doe@example.com",
            //                Name = "John Doe"
            //            },
            //            new EmailAddress(){
            //                Email = "julia_doe@example.com",
            //                Name = "Julia Doe"
            //            }
            //        },
            //        Ccs = new List<EmailAddress>(){
            //            new EmailAddress(){
            //                Email = "jane_doe@example.com",
            //                Name = "Jane Doe"
            //            }
            //        },
            //        Bccs = new List<EmailAddress>(){
            //            new EmailAddress(){
            //                Email = "james_doe@example.com",
            //                Name = "Jim Doe"
            //            }
            //        }
            //    },
            //    new Personalization(){
            //        From = new EmailAddress(){
            //            Email = "sales@example.com",
            //            Name = "Example Sales Team"
            //        },
            //        Tos = new List<EmailAddress>(){
            //            new EmailAddress(){
            //                Email = "janice_doe@example.com",
            //                Name = "Janice Doe"
            //            }
            //        },
            //        Bccs = new List<EmailAddress>(){
            //            new EmailAddress(){
            //                Email = "jordan_doe@example.com",
            //                Name = "Jordan Doe"
            //            }
            //        }
            //    }
            //};

            //    message.From = new EmailAddress()
            //    {
            //        Email = "orders@example.com",
            //        Name = "Example Order Confirmation"
            //    };

            //    message.ReplyTo = new EmailAddress()
            //    {
            //        Email = "customer_service@example.com",
            //        Name = "Example Customer Service Team"
            //    };

            //    message.Subject = "Your Example Order Confirmation";

            //    message.Contents = new List<Content>(){
            //    new Content(){
            //        Type = "text/html",
            //        Value = "<p>Hello from Twilio SendGrid!</p><p>Sending with the email service trusted by developers and marketers for <strong>time-savings</strong>, <strong>scalability</strong>, and <strong>delivery expertise</strong>.</p><p>%open-track%</p>"
            //    }
            //};

            //    message.Attachments = new List<Attachment>(){
            //    new Attachment(){
            //        Content = "PCFET0NUWVBFIGh0bWw+CjxodG1sIGxhbmc9ImVuIj4KCiAgICA8aGVhZD4KICAgICAgICA8bWV0YSBjaGFyc2V0PSJVVEYtOCI+CiAgICAgICAgPG1ldGEgaHR0cC1lcXVpdj0iWC1VQS1Db21wYXRpYmxlIiBjb250ZW50PSJJRT1lZGdlIj4KICAgICAgICA8bWV0YSBuYW1lPSJ2aWV3cG9ydCIgY29udGVudD0id2lkdGg9ZGV2aWNlLXdpZHRoLCBpbml0aWFsLXNjYWxlPTEuMCI+CiAgICAgICAgPHRpdGxlPkRvY3VtZW50PC90aXRsZT4KICAgIDwvaGVhZD4KCiAgICA8Ym9keT4KCiAgICA8L2JvZHk+Cgo8L2h0bWw+Cg==",
            //        Filename = "index.html",
            //        Type = "text/html",
            //        Disposition = "attachment"
            //    }
            //};

            //    message.Categories = new List<string>(){
            //    "cake",
            //    "pie",
            //    "baking"
            //};

            //    message.SendAt = 1617260400;

            //    message.BatchId = "AsdFgHjklQweRTYuIopzXcVBNm0aSDfGHjklmZcVbNMqWert1znmOP2asDFjkl";

            //    message.Asm = new ASM()
            //    {
            //        GroupId = 12345,
            //        GroupsToDisplay = new List<int>(){
            //        12345
            //    }
            //    };

            //    message.IpPoolName = "transactional email";

            //    message.MailSettings = new MailSettings()
            //    {
            //        BypassListManagement = new BypassListManagement()
            //        {
            //            Enable = false
            //        },
            //        FooterSettings = new FooterSettings()
            //        {
            //            Enable = false
            //        },
            //        SandboxMode = new SandboxMode()
            //        {
            //            Enable = false
            //        }
            //    };

            //    message.TrackingSettings = new TrackingSettings()
            //    {
            //        ClickTracking = new ClickTracking()
            //        {
            //            Enable = true,
            //            EnableText = false
            //        },
            //        OpenTracking = new OpenTracking()
            //        {
            //            Enable = true,
            //            SubstitutionTag = "%open-track%"
            //        },
            //        SubscriptionTracking = new SubscriptionTracking()
            //        {
            //            Enable = false
            //        }
            //    };

            //    string apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            //    var client = new SendGridClient(apiKey);
            //    var response = await client.SendEmailAsync(message).ConfigureAwait(false);

            //var msg = new SendGridMessage
            //{
            //    From = new EmailAddress(request.FromEmail ?? _mailConfig.SendGridSMTP.DefaultFromEmail, request.FromDisplayName ?? _mailConfig.SendGridSMTP.DefaultFromName),
            //    Subject = request.Subject,
            //    HtmlContent = request.HtmlContent,
            //    Personalizations = new List<Personalization>()
            //    {
            //        new Personalization(){
            //            Tos = new List<EmailAddress>(){
            //                new EmailAddress(){
            //                    Email = "conmansia@gmail.com",
            //                    Name = "Janice Doe"
            //                }
            //            }
            //        }
            //    }
            //};

            //// Reply To
            //if (!string.IsNullOrEmpty(request.ReplyToEmail))
            //    msg.ReplyTo = new EmailAddress(request.ReplyToEmail, request.ReplyToName);

            // Tos
            //foreach (var header in request.Tos)
            //{
            //    if (!string.IsNullOrEmpty(header.Key))
            //        msg.AddTo(header.Key, header.Value);
            //}

            //// Bccs
            //foreach (var header in request.Bccs)
            //{
            //    if (!string.IsNullOrEmpty(header.Key))
            //        msg.AddTo(header.Key, header.Value);
            //}

            // Cc
            //foreach (var header in request.Ccs)
            //{
            //    if (!string.IsNullOrEmpty(header.Key))
            //        msg.AddTo(header.Key, header.Value);
            //}

            //// Headers
            //foreach (var header in request.Headers)
            //    msg.Headers.Add(header.Key, header.Value);

            //// TODO: add type
            //// Create the file attachments for this e-mail message
            //if (request.Attachments.Any())
            //{
            //    foreach (var attachmentInfo in request.Attachments)
            //        msg.Attachments.Add(new SendGrid.Helpers.Mail.Attachment()
            //        {
            //            Filename = attachmentInfo.Key,
            //            Content = attachmentInfo.Value.ToString()
            //        });
            //}

            //var response = await _sendGridClient.SendEmailAsync(msg).ConfigureAwait(false);

            //var stringsss = response.DeserializeResponseBodyAsync().Result;
        }
    }
}