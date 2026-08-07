using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.BuildingBlocks.Application.Outbox
{
    public interface IOutbox
    {
        void Add(OutboxMessage message);

        Task Save();
    }
}
