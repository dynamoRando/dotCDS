using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS
{
    internal static class InternalSQLStatements
    {
        internal static class TableNames
        {
            internal const string CDS_USER = "CDS_USER";
            internal const string CDS_ROLE = "CDS_ROLE";
            internal const string CDS_USER_ROLE = "CDS_USER_ROLE";
        }

        internal static class RoleNames
        {
            public const string SYS_ADMIN = "SysAdmin";
        }

        internal static class SQLLite
        {
            internal const string CREATE_USER_TABLE = $@"
        CREATE TABLE IF NOT EXISTS {TableNames.CDS_USER} 
        (
            USERNAME VARCHAR(25) UNIQUE,
            BYTELENGTH INT NOT NULL,
            SALT BLOB NOT NULL,
            HASH BLOB NOT NULL,
            WORKFACTOR INT NOT NULL
        );";

            internal const string CREATE_ROLE_TABLE = $@"
        CREATE TABLE IF NOT EXISTS {TableNames.CDS_ROLE} 
        (
            ROLENAME VARCHAR(25) UNIQUE
        );";

            internal const string CREATE_USER_ROLE_TABLE = $@"
        CREATE TABLE IF NOT EXISTS {TableNames.CDS_USER_ROLE} 
        (
            USERNAME VARCHAR(25) NOT NULL,
            ROLENAME VARCHAR(25) NOT NULL   
        );";

            internal const string ADD_ADMIN_ROLE = $"INSERT INTO {TableNames.CDS_ROLE} (ROLENAME) VALUES ('{RoleNames.SYS_ADMIN}');";
            internal const string ADD_USER_TO_ROLE = $"INSERT INTO {TableNames.CDS_USER_ROLE} (USERNAME, ROLENAME) VALUES (@username, @rolename);";

            internal const string COUNT_OF_TABLES_WITH_NAME = @"
            SELECT count(*) AS TABLECOUNT FROM sqlite_master WHERE type='table' AND name='table_name';
            ";

            internal const string COUNT_OF_USERS_WITH_NAME = $"SELECT count(*) AS USERCOUNT FROM {TableNames.CDS_USER} WHERE USERNAME = 'user_name';";
            internal const string COUNT_OF_ROLES_WITH_NAME = $"SELECT count(*) AS ROLECOUNT FROM {TableNames.CDS_ROLE} WHERE ROLENAME = 'role_name';";
            internal const string COUNT_OF_USER_WITH_ROLE = $"SELECT count(*) AS TOTALCOUNT FROM {TableNames.CDS_USER_ROLE} WHERE USERNAME = @username AND ROLENAME = @rolename;";

            internal const string ADD_LOGIN = $"INSERT INTO {TableNames.CDS_USER} (USERNAME, BYTELENGTH, SALT, HASH, WORKFACTOR) VALUES (@username, @bytelength, @salt, @hash, @workfactor);";
            internal const string GET_LOGIN = $"SELECT USERNAME, BYTELENGTH, SALT, HASH, WORKFACTOR FROM {TableNames.CDS_USER} WHERE USERNAME = '@username'";
        }

    }
}
