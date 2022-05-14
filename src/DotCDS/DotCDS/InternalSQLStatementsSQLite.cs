using System;
namespace DotCDS
{
    internal static partial class InternalSQLStatements
    {
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

            internal const string CREATE_DATA_HOST = $@"
        CREATE TABLE IF NOT EXISTS {TableNames.COOP.DATA_HOST} 
        (
            DATABASE_ID CHAR(36) NOT NULL,
            DATABASE_NAME VARCHAR(500) NOT NULL
        );
        ";

            internal const string CREATE_HOST_TABLE = $@"
        CREATE TABLE IF NOT EXISTS {TableNames.COOP.DATA_HOST_TABLES} 
        (
            TABLE_ID CHAR(36) NOT NULL,
            TABLE_NAME VARCHAR(500) NOT NULL
        );
        ";

            internal const string CREATE_HOST_TABLE_COLUMNS = $@"
        CREATE TABLE IF NOT EXISTS {TableNames.COOP.DATA_HOST_TABLE_COLUMNS}
        (
            TABLE_ID CHAR(36) NOT NULL,
            COLUMN_ID CHAR(36) NOT NULL,
            COLUMN_NAME VARCHAR(500) NOT NULL
        )
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

            internal const string ADD_HOST_INFO = $@"
            INSERT INTO {TableNames.CDS.HOST_INFO}
            (
                HOST_ID,
                HOST_NAME,
                TOKEN
            )
            VALUES
            (
                @hostId,
                @hostName,
                @token
            )
            ;
            ";

            internal const string GET_DB_CONTRACTS = $@"
            SELECT
                CONTRACT_ID,
                GENERATED_DATE_UTC,
                DESCRIPTION,
                RETIRED_DATE_UTC,
                VERSION_ID,
                REMOTE_DELETE_BEHAVIOR  
            FROM
                {TableNames.COOP.DATABASE_CONTRACT}
            ;
            ";

            internal const string UPDATE_DB_CONTRACT_WITH_ID = $@"
            UPDATE {TableNames.COOP.DATABASE_CONTRACT}
            SET 
                GENERATED_DATE_UTC = @generatedDateUtc,
                DESCRIPTION = @description,
                RETIRED_DATE_UTC = @retiredDateUtc,
                REMOTE_DELETE_BEHAVIOR = @remoteDeleteBehavior
            WHERE
                VERSION_ID = @versionId
            ;";

            internal const string ADD_DB_CONTRACT = $@"
            INSERT INTO {TableNames.COOP.DATABASE_CONTRACT}
            (
                CONTRACT_ID,
                GENERATED_DATE_UTC,
                DESCRIPTION,
                RETIRED_DATE_UTC,
                VERSION_ID,
                REMOTE_DELETE_BEHAVIOR
            )
            VALUES
            (
                @id,
                @generatedDateUtc,
                @description,
                @retiredDateUtc,
                @versionId,
                @remoteDeleteBehavior
            )
            ;
            ";

            internal const string GET_ACTIVE_DB_CONTRACT = $@"
            SELECT
                CONTRACT_ID,
                GENERATED_DATE_UTC,
                DESCRIPTION,
                RETIRED_DATE_UTC,
                VERSION_ID,
                REMOTE_DELETE_BEHAVIOR  
            FROM
                {TableNames.COOP.DATABASE_CONTRACT}
            WHERE
                RETIRED_DATE_UTC = @date
            ;
            ";

            internal const string GET_DB_PARTICIPANTS = $@"
            SELECT
                INTERNAL_PARTICIPANT_ID,
                ALIAS,
                IP4ADDRESS,
                IP6ADDRESS,
                PORT,
                CONTRACT_STATUS,
                ACCEPTED_CONTRACT_VERSION_ID,
                TOKEN,
                PARTICIPANT_ID
            FROM
                {TableNames.COOP.PARTICIPANT}
            ;
            ";

            internal const string GET_DB_PARTICIPANT_BY_ALIAS = $@"
            SELECT
                INTERNAL_PARTICIPANT_ID,
                ALIAS,
                IP4ADDRESS,
                IP6ADDRESS,
                PORT,
                CONTRACT_STATUS,
                ACCEPTED_CONTRACT_VERSION_ID,
                TOKEN,
                PARTICIPANT_ID
            FROM
                {TableNames.COOP.PARTICIPANT}
            WHERE
                ALIAS = 'p_alias'
            ;
            ";

            internal const string UPDATE_DB_PARTICIPANT_BY_ALIAS = $@"
            UPDATE {TableNames.COOP.PARTICIPANT}
            SET 
                IP4ADDRESS = @ip4,
                IP6ADDRESS = @ip6,
                PORT = @port,
                CONTRACT_STATUS = @status,
                ACCEPTED_CONTRACT_VERSION_ID = @contract_version,
                TOKEN = @token  
            WHERE
                ALIAS = @alias
            ";

            internal const string INSERT_DB_PARTICIPANT = $@"
            INSERT INTO {TableNames.COOP.PARTICIPANT}
            (
                INTERNAL_PARTICIPANT_ID,
                ALIAS,
                IP4ADDRESS,
                IP6ADDRESS,
                PORT,
                CONTRACT_STATUS,
                ACCEPTED_CONTRACT_VERSION_ID,
                TOKEN,
                PARTICIPANT_ID
            )
            VALUES
            (
                @internalId,
                @alias,
                @ip4,
                @ip6,
                @port,
                @status,
                @contract_version,
                @token,
                @participantId
            )
            ;
            ";

            internal const string INSERT_DB_CONTRACT_FROM_HOST = $@"INSERT INTO {TableNames.CDS.CONTRACTS}
            (
                HOST_ID,
                CONTRACT_ID,
                CONTRACT_VERSION_ID,
                DATABASE_NAME,
                DATABASE_ID,
                DESCRIPTION,
                GENERATED_DATE_UTC,
                CONTRACT_STATUS
            )
            VALUES
            (
                @hostId,
                @contractId,
                @contractVersion,
                @databaseName,
                @databaseId,
                @description,
                @gendate,
                @status
            )
            ";

            internal const string INSERT_DB_CONTRACT_TABLE_FROM_HOST = $@"
            INSERT INTO {TableNames.CDS.CONTRACTS_TABLES}
            (
                DATABASE_ID,
                DATABASE_NAME,
                TABLE_ID,
                TABLE_NAME
            )
            VALUES
            (   
                @dbId,
                @dbName,
                @tbId,
                @tbName
            )
            ";

            internal const string INSERT_DB_CONTRACT_TABLE_COLUMN_FROM_HOST = $@"
            INSERT INTO {TableNames.CDS.CONTRACTS_TABLE_SCHEMAS}            
            (
                TABLE_ID,
                COLUMN_ID,
                COLUMN_NAME,
                COLUMN_TYPE,
                COLUMN_LENGTH,
                COLUMN_ORDINAL,
                IS_NULLABLE
            )
            VALUES
            (
                @tbId,
                @colId,
                @colName,
                @colType,
                @colLength,
                @colOrdinal,
                @isNullable
            )
            ;";

            internal const string GET_PENDING_CONTRACTS_FROM_HOST = $@"
            SELECT
                HOST_ID,
                CONTRACT_ID,
                CONTRACT_VERSION_ID,
                DATABASE_NAME,
                DATABASE_ID,
                DESCRIPTION,
                GENERATED_DATE_UTC,
                CONTRACT_STATUS
            FROM
                {TableNames.CDS.CONTRACTS}
            WHERE
                CONTRACT_STATUS = 2
            ";

            internal const string GET_DB_CONTRACT_ID = $@"SELECT MAX(CONTRACT_ID) CONTRACT_ID FROM {TableNames.COOP.DATABASE_CONTRACT}";

            internal const string GET_DB_CONTRACT_COUNT_FOR_VERSION_ID = $"SELECT COUNT(*) CONTRACTCOUNT FROM {TableNames.COOP.DATABASE_CONTRACT} WHERE VERSION_ID = 'version_id';";

            internal const string GET_DB_CONTRACT_COUNT = $"SELECT COUNT(*) CONTRACTCOUNT FROM {TableNames.COOP.DATABASE_CONTRACT}";

            internal const string GET_DB_PARTICIPANT_COUNT = $"SELECT COUNT(*) PARTICIPANTCOUNT FROM {TableNames.COOP.PARTICIPANT}";

            internal const string HAS_DB_PARTICIPANT_BY_ALIAS = $"SELECT COUNT(*) PARTICIPANTCOUNT FROM {TableNames.COOP.PARTICIPANT} WHERE ALIAS = 'p_alias';";
            internal const string HAS_DB_PARTICIPANT_BY_ID = $"SELECT COUNT(*) PARTICIPANTCOUNT FROM {TableNames.COOP.PARTICIPANT} WHERE PARTICIPANT_ID = 'p_id';";

            internal const string ADD_ADMIN_ROLE = $"INSERT INTO {TableNames.CDS.ROLE} (ROLENAME) VALUES ('{RoleNames.SYS_ADMIN}');";
            internal const string ADD_USER_TO_ROLE = $"INSERT INTO {TableNames.CDS.USER_ROLE} (USERNAME, ROLENAME) VALUES (@username, @rolename);";

            internal const string COUNT_OF_TABLES_WITH_NAME = @"
            SELECT count(*) AS TABLECOUNT FROM sqlite_master WHERE type='table' AND name='table_name';
            ";

            internal const string GET_TABLE_SCHEMA = @"
            SELECT sql FROM sqlite_master WHERE tbl_name= 't_name' and type = 'table';
            ";

            internal const string COUNT_OF_USERS_WITH_NAME = $"SELECT count(*) AS USERCOUNT FROM {TableNames.CDS.USER} WHERE USERNAME = 'user_name';";
            internal const string COUNT_OF_ROLES_WITH_NAME = $"SELECT count(*) AS ROLECOUNT FROM {TableNames.CDS.ROLE} WHERE ROLENAME = 'role_name';";
            internal const string COUNT_OF_USER_WITH_ROLE = $"SELECT count(*) AS TOTALCOUNT FROM {TableNames.CDS.USER_ROLE} WHERE USERNAME = @username AND ROLENAME = @rolename;";

            internal const string ADD_LOGIN = $"INSERT INTO {TableNames.CDS.USER} (USERNAME, BYTELENGTH, SALT, HASH, WORKFACTOR) VALUES (@username, @bytelength, @salt, @hash, @workfactor);";
            internal const string GET_LOGIN = $"SELECT USERNAME, BYTELENGTH, SALT, HASH, WORKFACTOR FROM {TableNames.CDS.USER} WHERE USERNAME = @username;";
        }
    }
}
