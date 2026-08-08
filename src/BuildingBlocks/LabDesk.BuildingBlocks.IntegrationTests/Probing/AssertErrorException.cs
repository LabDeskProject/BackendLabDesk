using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.BuildingBlocks.IntegrationTests.Probing
{
    public class AssertErrorException : Exception
    {
        public AssertErrorException(string message)
            : base(message)
        {
        }
    }
}
