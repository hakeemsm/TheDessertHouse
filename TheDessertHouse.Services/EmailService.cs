using System;
using System.Net.Mail;
using log4net;
using TheDessertHouse.Domain;
using TheDessertHouse.Infrastructure;
using TheDessertHouse.Services.ControllerTests;

namespace TheDessertHouse.Services
{
    /// <summary>
    /// Gateway to the email service. Technically this class could have been in the Web assembly. Moved out so as to
    /// have all external services in a separate project
    /// </summary>
    public class EmailService : IEmailService
    {
        private IRepositoryProvider _repositoryProvider;
        private ILog _logger;

        public EmailService(IRepositoryProvider repositoryProvider)
        {
            _repositoryProvider = repositoryProvider;
            _logger = LogManager.GetLogger("DessertHouse");
        }

        public void SendNewsLetter(object msgObj)
        {
            Newsletter newsletter = null;
            try
            {
                var newsletterMessage = msgObj as NewsletterMessage;
                if (newsletterMessage == null)
                    throw new Exception("NewsletterMessage is null");
                newsletter = newsletterMessage.Newsletter;
                var mailMessage = new MailMessage
                                      {
                                          Body = newsletter.HtmlBody,
                                          From = new MailAddress(newsletterMessage.FromEmail, newsletterMessage.FromEmailDisplayName),
                                          Subject = newsletter.Subject
                                      };
                var emailList = newsletterMessage.EmailList;
                emailList.ForEach(e => mailMessage.Bcc.Add(e));

                new SmtpClient().Send(mailMessage);
                using (var repository = _repositoryProvider.GetRepository())
                {
                    newsletter.Status = "Sent";
                    newsletter.DateSent = DateTime.Now;
                    repository.SaveOrUpdate(newsletter);
                }
            }
            catch (Exception ex)
            {
                if (newsletter != null)
                {
                    newsletter.Status = string.Format("Send Failed. Exception: {0}", ex.Message);
                    using (var repository = _repositoryProvider.GetRepository())
                        repository.SaveOrUpdate(newsletter);
                }
                _logger.ErrorFormat("Exception thrown sending Email\n Message: {0}",ex.Message);
            }
        }
    }
}