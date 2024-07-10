using Application.Interfaces.Services;
using Application.Interfaces.Services.Events;
using Application.Interfaces.Services.Mailing;
using Application.Models.MailTemplateModels;
using Domain.Events.Users;
using Microsoft.Extensions.Logging;

namespace Application.EventHandlers;

public class UserCreatedEventHandler(IJobService jobService, ILogger<UserCreatedEventHandler> logger, IEmailTemplateService emailTemplateService, IMailService mailService) : IEventNotificationHandler<UserCreatedEvent>
{
    public async Task Handle(EventNotification<UserCreatedEvent> notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event: {DomainEvent}", notification.Event.GetType().Name);

        var userName = notification.Event.User.FirstName + notification.Event.User.LastName;

        var html = await emailTemplateService.GenerateEmailTemplate("new-user-greeting", new NewUserGreetingModel()
        {
            UserName = userName,
            ReadMoreHereUrl = "https://github.com/yizhe1997/YZPortal",
            GetStartedUrl = "https://yzportalclient.azurewebsites.net",
        });

        var createMailCommand = new Requests.CreateMailCommand()
        {
            Tos =
            [
                new() { Name = userName, Email = notification.Event.User.Email}
            ],
            HtmlContent = html,
            Subject = $"YZPortal Admin Dashboard: Hi {userName}! Your Account Has Been Created!"
        };

        jobService.Enqueue(() => mailService.SendAsync(createMailCommand, cancellationToken));
    }
}
