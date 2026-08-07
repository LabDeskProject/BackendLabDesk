using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.BuildingBlocks.Application.Queries
{
    public struct PageData
    {
        public int Offset { get; }

        public int Next { get; }

        public PageData(int offset, int next)
        {
            this.Offset = offset;
            this.Next = next;
        }
    }
}
