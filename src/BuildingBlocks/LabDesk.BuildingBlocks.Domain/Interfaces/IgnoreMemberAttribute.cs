using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.SeedWork.Domain.Interfaces
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    class IgnoreMemberAttribute : Attribute
    {

    }
}
