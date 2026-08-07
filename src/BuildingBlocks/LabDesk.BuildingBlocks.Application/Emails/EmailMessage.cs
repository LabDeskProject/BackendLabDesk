using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.BuildingBlocks.Application.Emails
{
    public struct EmailMessage
    {
        public string To { get; }

        public string Subject { get; }

        public string Content { get; }

        public EmailMessage(
            string to,
            string subject,
            string content)
        {
            this.To = to;
            this.Subject = subject;
            this.Content = content;
        }
    }
}
