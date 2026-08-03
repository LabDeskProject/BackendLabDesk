using LabDesk.SeedWork.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.SeedWork.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
