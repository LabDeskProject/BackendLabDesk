using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.BuildingBlocks.Application.Emails
{
    public interface IEmailSender
    {
        Task SendEmail(EmailMessage message);
    }
}
