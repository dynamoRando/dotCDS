using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS
{
    internal static class InternalSQLStatements
    {
        internal static class InteralSQLLiteStatements
        {
            internal const string SQL_CREATE_USER_TABLE = @"
        CREATE TABLE IF NOT EXISTS CDS_USER 
        (
            USERNAME VARCHAR(25) UNIQUE,
            BYTELENGTH INT NOT NULL,
            SALT BLOB NOT NULL,
            HASH BLOB NOT NULL,
            WORKFACTOR INT NOT NULL
        );";

            internal const string SQL_COUNT_OF_TABLES_WITH_NAME = @"
            SELECT count(*) AS TABLECOUNT FROM sqlite_master WHERE type='table' AND name='table_name';
            ";
        }

    }
}
