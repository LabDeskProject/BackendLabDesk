using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.SeedWork.Application.Results
{
    public record Error(string Code, string Description)
    {
        public static readonly Error None = new(string.Empty, string .Empty);
        public static readonly Error NullValue = new("Error.NullValue", "Value is null.");
    }
}
