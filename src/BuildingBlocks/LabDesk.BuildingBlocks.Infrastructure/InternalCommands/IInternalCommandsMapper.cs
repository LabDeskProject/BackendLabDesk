using System;
using System.Collections.Generic;
using System.Text;

namespace LabDesk.BuildingBlocks.Infrastructure.InternalCommands
{
    public interface IInternalCommandsMapper
    {
        string GetName(Type type);

        Type GetType(string name);
    }
}
