using System;
using System.Net.Mail;

namespace Library.Reporting
{
    public class ReportMailer
    {
        private readonly MailDestination[] destinations;
        const string From = "reports@librarySystem.com";

        public ReportMailer(MailDestination[] destinations)
        {
            this.destinations = destinations;
            if (destinations.Length == 0)
                throw new Exception("dests required");
            foreach (var destination in destinations)
                if (MailDestination.RetrieveEndpoint(destination) == null)
                    throw new Exception("invalid endpoint");
        }

        public void MailReport(IReport report)
        {
            foreach (var destination in destinations)
            {
                var toAddress = destination.Address;
                var message = ConstructMailMessage(toAddress, report);
                destination.Send(message);
            }
        }

        MailMessage ConstructMailMessage(String toAddress, IReport report)
        {
            var content = report.Text();
            var subject = report.Name();
            var message = new MailMessage();
            message.To.Add(toAddress);
            message.Body = content;
            message.From = new MailAddress(From);
            message.Subject = subject;
            return message;
        }
    }
}