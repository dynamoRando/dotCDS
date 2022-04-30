using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS
{
    internal static class InternalSQLStatements
    {
        internal static class SQLKeywords
        {
            internal static class CRUD
            {
                internal const string SELECT = "SELECT";
                internal const string INSERT = "INSERT";
                internal const string UPDATE = "UPDATE";
                internal const string DELETE = "DELETE";
            }
        }

        internal static class TableNames
        {
            /// <summary>
            /// anything in CDS is in the cooperative data store
            /// </summary>
            internal static class CDS
            {
                /// <summary>
                /// users in this CDS 
                /// </summary>
                internal const string USER = "CDS_USER";

                /// <summary>
                /// roles in this CDS
                /// </summary>
                internal const string ROLE = "CDS_ROLE";

                /// <summary>
                /// xref users to roles
                /// </summary>
                internal const string USER_ROLE = "CDS_USER_ROLE";

                // ----------
                // the tables below are for holding our identifier information to other participants
                // ----------

                /// <summary>
                /// holds our unique identifiers to participants
                /// </summary>
                internal const string HOST_INFO = "CDS_HOST_INFO";

                // ----------
                // the tables below are for partial databases and their contracts
                // ----------

                /// <summary>
                /// hosts that this CDS is cooperating with
                /// </summary>
                internal const string HOSTS = "CDS_HOSTS";

                /// <summary>
                /// holds schema information for partial databases participating with a remote host
                /// </summary>
                internal const string CONTRACTS = "CDS_CONTRACTS";

                /// <summary>
                /// holds the tables information for the partial databases 
                /// </summary>
                internal const string CONTRACTS_TABLES = "CDS_CONTRACTS_TABLES";

                /// <summary>
                /// holds the schema information for the tables in partial databases
                /// </summary>
                internal const string CONTRACTS_TABLE_SCHEMAS = "CDS_CONTRACTS_TABLES_SCHEMAS";
            }

            /// <summary>
            /// anything in COOP are tables stored in the user database used to enable cooperative functions with participants
            /// </summary>
            internal static class COOP
            {
                // ----------
                // the tables below are for data hosts
                // ----------

                // a list of participants for this user database
                internal const string PARTICIPANT = "COOP_PARTICIPANT";

                // a list of all the contracts that we have generated for this database
                internal const string DATABASE_CONTRACT = "COOP_DATABASE_CONTRACT";

                // the naming convention prefix for any table that is holding remote row references
                // e.g. if the table is named 'EMPLOYEE_ADDRESS' and is remotable, the actual table holding the participants references would be named
                // 'COOP_SHADOWS_EMPLOYEE_ADDRESS' and would just contain all the participants that would needed to be queried
                // and the 'EMPLOYEE_ADDRESS' would just be an empty table
                internal const string SHADOWS = "COOP_SHADOWS";

                // contains a list of all the tables that have remote data turned on for them
                // this tells CDS that rather than actually sending the request to Sqlite to query that table, 
                // to instead look at the corresponding shadow table and request data from the participant(s)
                internal const string REMOTES = "COOP_REMOTES";

                // ----------
                // the tables below are for partial databases (data participants)
                // ----------

                // the naming convention prefix for any table that is in a partial database
                // this table holds references back to the host for the rows
                // e.g. a table in a partial database named "EMPLOYEE_ADDRESS" would also have a table 
                // named "COOP_PARENT_EMPLOYEE_ADDRESS" which would point back to the host that had a reference to this table
                internal const string PARENT = "COOP_PARENT";
            }
        }

        internal static class RoleNames
        {
            public const string SYS_ADMIN = "SysAdmin";
        }

        internal static class SQLLite
        {
            internal const string CREATE_USER_TABLE = $@"
        CREATE TABLE IF NOT EXISTS {TableNames.CDS.USER} 
        (
            USERNAME VARCHAR(25) UNIQUE,
            BYTELENGTH INT NOT NULL,
            SALT BLOB NOT NULL,
            HASH BLOB NOT NULL,
            WORKFACTOR INT NOT NULL
        );";

            internal const string CREATE_ROLE_TABLE = $@"
        CREATE TABLE IF NOT EXISTS {TableNames.CDS.ROLE} 
        (
            ROLENAME VARCHAR(25) UNIQUE
        );";

            internal const string CREATE_USER_ROLE_TABLE = $@"
        CREATE TABLE IF NOT EXISTS {TableNames.CDS.USER_ROLE} 
        (
            USERNAME VARCHAR(25) NOT NULL,
            ROLENAME VARCHAR(25) NOT NULL   
        );";

            internal const string CREATE_PARTICIPANT_TABLE = $@"
        CREATE TABLE IF NOT EXISTS {TableNames.COOP.PARTICIPANT} 
        (
            INTERNAL_PARTICIPANT_ID CHAR(36) NOT NULL,
            ALIAS VARCHAR(50) NOT NULL,
            IP4ADDRESS VARCHAR(25),
            IP6ADDRESS VARCHAR(25),
            PORT INT,
            CONTRACT_STATUS INT,
            ACCEPTED_CONTRACT_VERSION_ID CHAR(36),
            TOKEN BLOB NOT NULL,
            PARTICIPANT_ID CHAR(36)
        );";

            internal const string CREATE_DATABASE_CONTRACT_TABLE = $@"
        CREATE TABLE IF NOT EXISTS {TableNames.COOP.DATABASE_CONTRACT}
        (
            CONTRACT_ID CHAR(36) NOT NULL,
            GENERATED_DATE_UTC DATETIME NOT NULL,
            DESCRIPTION VARCHAR(255),
            RETIRED_DATE_UTC DATETIME,
            VERSION_ID CHAR(36) NOT NULL,
            REMOTE_DELETE_BEHAVIOR INT
        );";

            internal const string CREATE_SHADOW_TABLE = $@"
        CREATE TABLE IF NOT EXISTS {TableNames.COOP.SHADOWS} 
        (
            PARTICIPANT_ID CHAR(36) NOT NULL,
            IS_PARTICIPANT_DELETED INT,
            PARTICIPANT_DELETE_DATE_UTC DATETIME,
            DATA_HASH_LENGTH INT,
            DATA_HASH BLOB
        );
        ";

            internal const string CREATE_REMOTE_TABLE = $@"
        CREATE TABLE IF NOT EXISTS {TableNames.COOP.REMOTES}
        (
            TABLENAME VARCHAR(255) NOT NULL,
            LOGICAL_STORAGE_POLICY INT NOT NULL
        );
        ";

            internal const string CREATE_PARENT_TABLE = $@"
        CREATE TABLE IF NOT EXISTS {TableNames.COOP.PARENT} 
        (
            PARENT_ID CHAR(36) NOT NULL,
            IS_PARENT_DELETED INT,
            PARENT_DELETE_DATE_UTC DATETIME,
            DATA_HASH_LENGTH INT,
            DATA_HASH BLOB
        );
        ";

            internal const string CREATE_HOST_INTO_TABLE = $@"
        CREATE TABLE IF NOT EXISTS {TableNames.CDS.HOST_INFO}
        (
            HOST_ID CHAR(36) NOT NULL,
            HOST_NAME VARCHAR(50) NOT NULL,
            TOKEN BLOB NOT NULL
        );
        ";
            internal const string CREATE_HOSTS_TABLE = $@"
        CREATE TABLE IF NOT EXISTS {TableNames.CDS.HOSTS}
        (
            HOST_ID CHAR(36) NOT NULL,
            HOST_NAME VARCHAR(50),
            TOKEN BLOB,
            IP4ADDRESS VARCHAR(25),
            IP6ADDRESS VARCHAR(25),
            PORT INT,
            LAST_COMMUNICATION_UTC DATETIME
        );
        ";

            internal const string CREATE_CDS_CONTRACTS_TABLE = $@"
        CREATE TABLE IF NOT EXISTS {TableNames.CDS.CONTRACTS}
        (
            HOST_ID CHAR(36) NOT NULL,
            CONTRACT_ID CHAR(36) NOT NULL,
            CONTRACT_VERSION_ID CHAR(36) NOT NULL,
            DATABASE_NAME VARCHAR(50) NOT NULL,
            DATABASE_ID CHAR(36) NOT NULL,
            DESCRIPTION VARCHAR(255),
            GENERATED_DATE_UTC DATETIME,
            CONTRACT_STATUS INT
        );
        ";

            internal const string CREATE_CDS_CONTRACTS_TABLE_TABLE = $@"
        CREATE TABLE IF NOT EXISTS {TableNames.CDS.CONTRACTS_TABLES}
        (
            DATABASE_ID CHAR(36) NOT NULL,
            DATABASE_NAME VARCHAR(50) NOT NULL,
            TABLE_ID CHAR(36) NOT NULL,
            TABLE_NAME VARCHAR(50) NOT NULL
        );
        ";

            internal const string CREATE_CDS_CONTRACTS_TABLE_SCHEMA_TABLE = $@"
        CREATE TABLE IF NOT EXISTS {TableNames.CDS.CONTRACTS_TABLE_SCHEMAS}
        (
            TABLE_ID CHAR(36) NOT NULL,
            COLUMN_ID CHAR(36) NOT NULL,
            COLUMN_NAME VARCHAR(50) NOT NULL,
            COLUMN_TYPE INT NOT NULL,
            COLUMN_LENGTH INT NOT NULL,
            COLUMN_ORDINAL INT NOT NULL,
            IS_NULLABLE INT
        );
        ";

            internal const string GET_REMOTE_STATUS = $@"
            SELECT
                TABLENAME,
                LOGICAL_STORAGE_POLICY  
            FROM
                {TableNames.COOP.REMOTES}
            ;
            ";

            internal const string GET_REMOTE_STATUS_TABLE = $@"
            SELECT
                TABLENAME,
                LOGICAL_STORAGE_POLICY  
            FROM
                {TableNames.COOP.REMOTES} 
            WHERE
                TABLENAME = 'table_name'
            ;
            ";

            internal const string HAS_REMOTE_STATUS_TABLE = $@"
            SELECT
                COUNT(*) TOTALCOUNT
            FROM
                {TableNames.COOP.REMOTES}
            WHERE
                TABLENAME = 'table_name'
            ;
            ";

            internal const string INSERT_REMOTE_STATUS_TABLE = $@"
            INSERT INTO {TableNames.COOP.REMOTES}
            (
                TABLENAME,
                LOGICAL_STORAGE_POLICY  
            )
            VALUES
            (
                @tableName,
                @policy
            );
            ";

            internal const string UPDATE_REMOTE_STATUS_TABLE = $@"
            UPDATE {TableNames.COOP.REMOTES}
            SET LOGICAL_STORAGE_POLICY = @policy
            WHERE TABLENAME = @tableName
            ";

            internal const string GET_HOST_INFO = $@"
            SELECT
                HOST_ID,
                HOST_NAME,
                TOKEN
            FROM
                {TableNames.CDS.HOST_INFO}
            ";

            internal const string ADD_ADMIN_ROLE = $"INSERT INTO {TableNames.CDS.ROLE} (ROLENAME) VALUES ('{RoleNames.SYS_ADMIN}');";
            internal const string ADD_USER_TO_ROLE = $"INSERT INTO {TableNames.CDS.USER_ROLE} (USERNAME, ROLENAME) VALUES (@username, @rolename);";

            internal const string COUNT_OF_TABLES_WITH_NAME = @"
            SELECT count(*) AS TABLECOUNT FROM sqlite_master WHERE type='table' AND name='table_name';
            ";

            internal const string COUNT_OF_USERS_WITH_NAME = $"SELECT count(*) AS USERCOUNT FROM {TableNames.CDS.USER} WHERE USERNAME = 'user_name';";
            internal const string COUNT_OF_ROLES_WITH_NAME = $"SELECT count(*) AS ROLECOUNT FROM {TableNames.CDS.ROLE} WHERE ROLENAME = 'role_name';";
            internal const string COUNT_OF_USER_WITH_ROLE = $"SELECT count(*) AS TOTALCOUNT FROM {TableNames.CDS.USER_ROLE} WHERE USERNAME = @username AND ROLENAME = @rolename;";

            internal const string ADD_LOGIN = $"INSERT INTO {TableNames.CDS.USER} (USERNAME, BYTELENGTH, SALT, HASH, WORKFACTOR) VALUES (@username, @bytelength, @salt, @hash, @workfactor);";
            internal const string GET_LOGIN = $"SELECT USERNAME, BYTELENGTH, SALT, HASH, WORKFACTOR FROM {TableNames.CDS.USER} WHERE USERNAME = @username;";
        }

    }
}
