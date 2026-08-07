using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LabDesk.BuildingBlocks.Application.Data
{
    public interface ISqlConnectionFactory
    {
        IDbConnection GetOpenConnection();

        IDbConnection CreateNewConnection();

        string GetConnectionString();
    }
}
