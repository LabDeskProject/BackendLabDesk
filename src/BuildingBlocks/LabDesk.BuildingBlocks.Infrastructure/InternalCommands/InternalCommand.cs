using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.BuildingBlocks.Infrastructure.InternalCommands
{
    public class InternalCommand
    {
        public Guid Id { get; set; }

        public string Type { get; set; }

        public string Data { get; set; }

        public DateTime? ProcessedDate { get; set; }
    }
}
