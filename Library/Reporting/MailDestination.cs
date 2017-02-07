using System;
using System.Net.Mail;
using System.Net;

namespace Library.Reporting
{
    public class MailDestination
    {
        public const string SmtpServer = "smtpServer";
        public const string SmtpPassword = "smtpPassword";
        public const string SmtpUser = "smtpUser";

        public static Endpoint RetrieveEndpoint(MailDestination origin)
        {
            return CreateEndpoint(origin);
        }

        private static Endpoint CreateEndpoint(MailDestination mailDestination)
        {
            throw new Exception("unable to connect to LDAP server");
        }

        public MailDestination(string address)
        {
            Address = address;
        }

        public string Address { get; private set; }

        public void Send(MailMessage message)
        {
            var smtp = new SmtpClient(Get(SmtpServer))
            {
                Credentials = new NetworkCredential(Get(SmtpUser), Get(SmtpPassword))
            };
            smtp.Send(message);
        }

        private string Get(string variableName)
        {
            return Environment.GetEnvironmentVariable(variableName);
        }

    }
}