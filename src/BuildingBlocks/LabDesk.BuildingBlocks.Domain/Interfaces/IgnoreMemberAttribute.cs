using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.BuildingBlocks.Domain.Interfaces
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    class IgnoreMemberAttribute : Attribute
    {

    }
}
