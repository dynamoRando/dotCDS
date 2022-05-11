using DotCDS.Common;
using DotCDS.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using DotCDS.Common.Enum;
using DotCDS.Enum;
using DotCDS.Model;

namespace DotCDS
{
    /// <summary>
    /// Represents a Sqlite database created by the user and provides 
    /// CDS related functions/activities 
    /// </summary>
    internal class SqliteUserDatabase
    {
        #region Private Fields
        private string _rootFolder;
        private string _databaseName;
        private SqliteClient _client;
        private CooperativeDatabaseClientCollection _remoteClients;
        #endregion

        #region Public Properties
        public string DatabaseName => _databaseName;
        #endregion

        #region Constructors
        public SqliteUserDatabase(string rootFolder, string name)
        {
            _rootFolder = rootFolder;
            _databaseName = name;

            _client = new SqliteClient(_rootFolder);
            _remoteClients = new CooperativeDatabaseClientCollection();
            CreateDbIfNotExists();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Checks to see if the database has any tables configured for cooperation
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool IsCooperative()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks to see if the database has a participant with the specified alias
        /// </summary>
        /// <param name="participantAlias"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool HasParticipant(string participantAlias)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks to see if the database has a participant with the specified id
        /// </summary>
        /// <param name="participantId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool HasParticipant(Guid participantId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the participant with the specified alias
        /// </summary>
        /// <param name="participantAlias"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Participant GetParticipant(string participantAlias)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the participant with the specified id
        /// </summary>
        /// <param name="participantId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Participant GetParticipant(Guid participantId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the list of all database contracts
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Contract[] GetContracts()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks to see if the specified table has a logical storage policy
        /// that is configured for cooperation with participants
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool IsTableCooperative(string tableName)
        {
            bool result = false;

            if (_client.HasTable(_databaseName, InternalSQLStatements.TableNames.COOP.REMOTES))
            {
                string COUNT_OF_TABLES_WITH_NAME = @$"
            SELECT count(*) AS TABLECOUNT FROM {InternalSQLStatements.TableNames.COOP.REMOTES} WHERE name='{tableName}';
            ";

                var dt = _client.ExecuteRead(_databaseName, COUNT_OF_TABLES_WITH_NAME);
                int totalRows = Convert.ToInt32(dt.Rows[0]["TABLECOUNT"]);
                return totalRows > 0;
            }

            return result;
        }

        public DataTable ExecuteRead(string query)
        {
            return _client.ExecuteRead(_databaseName, query);
        }

        public DataTable ExecuteRead(string query, Dictionary<string, object> values)
        {
            return _client.ExecuteRead(_databaseName, query, values);
        }

        public int ExecuteWrite(string query)
        {
            return _client.ExecuteWrite(_databaseName, query);
        }

        public int ExecuteWrite(string query, Dictionary<string, object> values)
        {
            return _client.ExecuteWrite(_databaseName, query, values);
        }

        public bool HasTable(string tableName)
        {
            return _client.HasTable(_databaseName, tableName);
        }

        public bool AddParticipant(string alias, string ipAddress, uint portNumber)
        {
            bool isSuccessful = false;
            if (!HasDatabaseParticipant(alias))
            {
                var participant = new DatabaseParticipant();
                participant.InternalId = Guid.NewGuid();
                participant.Alias = alias;
                participant.Ip4Address = ipAddress;
                participant.Ip6Address = String.Empty;
                participant.Port = portNumber;
                participant.Token = new byte[0];
                participant.ParticipantId = Guid.Empty;
                participant.AcceptedContractVersion = Guid.Empty;
                participant.ContractStatus = Common.Enum.ContractStatus.NotSent;

                SaveParticipant(participant);
                isSuccessful = true;
            }

            return isSuccessful;
        }

        public ActionOptionalResult EnableCooperativeFeatures()
        {
            ActionOptionalResult actionResult = new ActionOptionalResult();

            ActionOptionalResult resultRemote = CreateRemotesTableIfNotExists();
            if (resultRemote.Status == ActionOptionalResultStatus.Information ||
                resultRemote.Status == ActionOptionalResultStatus.Success)
            {
                ActionOptionalResult resultParticipant = CreateParticipantTableIfNotExists();
                if (resultParticipant.Status == ActionOptionalResultStatus.Success ||
                    resultParticipant.Status == ActionOptionalResultStatus.Information)
                {
                    actionResult.Status = ActionOptionalResultStatus.Success;
                }
                else
                {
                    actionResult = resultParticipant;
                }
            }
            else
            {
                actionResult = resultRemote;
            }

            CreateContractTableIfNotExists();
            CreateAndPopulateDataHostTables();

            return actionResult;
        }

        public bool SetLogicalStoragePolicy(string tableName, LogicalStoragePolicy policy)
        {
            bool isSuccessful = false;

            if (HasTable(tableName))
            {
                if (policy == GetLogicalStoragePolicy(tableName))
                {
                    isSuccessful = true;
                }
                else
                {
                    var values = new Dictionary<string, object>();
                    values.Add("@tableName", tableName);
                    values.Add("@policy", (int)policy);

                    // insert or update
                    string query = InternalSQLStatements.SQLLite.HAS_REMOTE_STATUS_TABLE.Replace("table_name", tableName);
                    DataTable dt = ExecuteRead(query);
                    if (dt.Rows.Count > 0)
                    {
                        int totalRows = Convert.ToInt32(dt.Rows[0]["TOTALCOUNT"]);

                        // is insert
                        if (totalRows == 0)
                        {
                            var result = ExecuteWrite(InternalSQLStatements.SQLLite.INSERT_REMOTE_STATUS_TABLE, values);
                            if (result > 0)
                            {
                                isSuccessful = true;
                            }
                        }

                        // is update
                        if (totalRows > 0)
                        {
                            var result = ExecuteWrite(InternalSQLStatements.SQLLite.UPDATE_REMOTE_STATUS_TABLE, values);
                            if (result > 0)
                            {
                                isSuccessful = true;
                            }
                        }
                    }
                }
            }

            return isSuccessful;
        }

        public DatabaseRemoteStatusTable[] GetStatusTables()
        {
            DataTable dt = ExecuteRead(InternalSQLStatements.SQLLite.GET_REMOTE_STATUS);
            var statuses = new DatabaseRemoteStatusTable[dt.Rows.Count];
            int i = 0;

            foreach (DataRow row in dt.Rows)
            {
                var status = new DatabaseRemoteStatusTable();
                status.TableName = Convert.ToString(row["TABLENAME"]) ?? string.Empty;
                uint logicalStatus = Convert.ToUInt32(row["LOGICAL_STORAGE_POLICY"]);
                status.StoragePolicy = (LogicalStoragePolicy)logicalStatus;
                statuses[i] = status;
                i++;
            }

            return statuses;

        }

        public LogicalStoragePolicy GetLogicalStoragePolicy(string tableName)
        {
            LogicalStoragePolicy policy = LogicalStoragePolicy.None;

            string query = InternalSQLStatements.SQLLite.GET_REMOTE_STATUS_TABLE;
            query = query.Replace("table_name", tableName);

            DataTable dt = ExecuteRead(query);

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string? dtName = Convert.ToString(row["TABLENAME"]);

                    if (dtName is null)
                    {
                        dtName = string.Empty;
                    }

                    if (string.Equals(dtName, tableName, StringComparison.OrdinalIgnoreCase))
                    {
                        int iPolicy = Convert.ToInt32(row["LOGICAL_STORAGE_POLICY"]);
                        policy = (LogicalStoragePolicy)iPolicy;
                        break;
                    }
                }
            }

            return policy;
        }

        public bool SaveParticipant(DatabaseParticipant participant)
        {
            bool isSuccessful = false;

            if (HasDatabaseParticipant(participant.Alias))
            {
                // update
                string query = InternalSQLStatements.SQLLite.UPDATE_DB_PARTICIPANT_BY_ALIAS;
                var values = new Dictionary<string, object>();
                values.Add("@ip4", participant.Ip4Address);
                values.Add("@ip6", participant.Ip6Address);
                values.Add("@port", participant.Port);
                values.Add("@status", participant.ContractStatus);
                values.Add("@contract_version", participant.AcceptedContractVersion.ToString());
                values.Add("@token", participant.Token);
                values.Add("@alias", participant.Alias);

                ExecuteWrite(query, values);
                isSuccessful = true;
            }
            else
            {
                // insert
                string query = InternalSQLStatements.SQLLite.INSERT_DB_PARTICIPANT;
                var values = new Dictionary<string, object>();
                values.Add("@ip4", participant.Ip4Address);
                values.Add("@ip6", participant.Ip6Address);
                values.Add("@port", participant.Port);
                values.Add("@status", (uint)participant.ContractStatus);
                values.Add("@contract_version", participant.AcceptedContractVersion.ToString());
                values.Add("@token", participant.Token);
                values.Add("@alias", participant.Alias);
                values.Add("@internalId", participant.InternalId.ToString());
                values.Add("@participantId", participant.ParticipantId.ToString());

                ExecuteWrite(query, values);
                isSuccessful = true;
            }

            return isSuccessful;
        }

        public bool HasDatabaseParticipant(string alias)
        {
            var query = InternalSQLStatements.SQLLite.HAS_DB_PARTICIPANT_BY_ALIAS;
            query = query.Replace("p_alias", alias);
            DataTable dt = ExecuteRead(query);
            int totalParticipants = Convert.ToInt32(dt.Rows[0]["PARTICIPANTCOUNT"]);

            return totalParticipants > 0;
        }

        public bool HasDatabaseParticipant(Guid participantId)
        {
            var query = InternalSQLStatements.SQLLite.HAS_DB_PARTICIPANT_BY_ID;
            query = query.Replace("p_id", participantId.ToString());
            DataTable dt = ExecuteRead(query);
            int totalParticipants = Convert.ToInt32(dt.Rows[0]["PARTICIPANTCOUNT"]);

            return totalParticipants > 0;
        }

        public DatabaseParticipant? GetDatabaseParticipant(string alias)
        {
            DatabaseParticipant result = null;

            string query = InternalSQLStatements.SQLLite.GET_DB_PARTICIPANT_BY_ALIAS;
            query = query.Replace("p_alias", alias);
            DataTable dt = ExecuteRead(query);
            foreach (DataRow row in dt.Rows)
            {
                result = DatabaseParticipant.Parse(row);
                break;
            }

            return result;
        }

        public DatabaseParticipant[] GetDatabaseParticipants()
        {
            var query = InternalSQLStatements.SQLLite.GET_DB_PARTICIPANT_COUNT;
            DataTable dt = ExecuteRead(query);
            int totalParticipants = Convert.ToInt32(dt.Rows[0]["PARTICIPANTCOUNT"]);
            var participants = new DatabaseParticipant[totalParticipants];
            int i = 0;

            query = InternalSQLStatements.SQLLite.GET_DB_PARTICIPANTS;
            dt = ExecuteRead(query);
            foreach (DataRow row in dt.Rows)
            {
                participants[i] = DatabaseParticipant.Parse(row);
                i++;
            }

            return participants;
        }

        public DatabaseContract[] GetDatabaseContracts()
        {
            var query = InternalSQLStatements.SQLLite.GET_DB_CONTRACT_COUNT;
            DataTable dt = ExecuteRead(query);
            int totalContracts = Convert.ToInt32(dt.Rows[0]["CONTRACTCOUNT"]);
            var contracts = new DatabaseContract[totalContracts];
            int i = 0;

            query = InternalSQLStatements.SQLLite.GET_DB_CONTRACTS;
            dt = ExecuteRead(query);
            foreach (DataRow row in dt.Rows)
            {
                contracts[i] = DatabaseContract.Parse(row);
                i++;
            }

            return contracts;
        }

        public void SaveContract(DatabaseContract contract)
        {
            var query = InternalSQLStatements.SQLLite.GET_DB_CONTRACT_COUNT_FOR_VERSION_ID;
            query = query.Replace("version_id", contract.Version.ToString());
            DataTable dt = ExecuteRead(query);
            int totalContracts = Convert.ToInt32(dt.Rows[0]["CONTRACTCOUNT"]);

            if (totalContracts > 0)
            {
                // update
                query = InternalSQLStatements.SQLLite.UPDATE_DB_CONTRACT_WITH_ID;
                var values = new Dictionary<string, object>();
                values.Add("@generatedDateUtc", contract.GeneratedDateUTC);
                values.Add("@description", contract.Description);
                values.Add("@retiredDateUtc", contract.RetiredDateUTC);
                values.Add("@remoteDeleteBehavior", contract.DeleteBehavior);
                values.Add("@versionId", contract.Version.ToString());

                ExecuteWrite(query, values);
            }

            if (totalContracts == 0)
            {
                // insert
                query = InternalSQLStatements.SQLLite.ADD_DB_CONTRACT;
                var values = new Dictionary<string, object>();
                values.Add("@generatedDateUtc", contract.GeneratedDateUTC);
                values.Add("@description", contract.Description);
                values.Add("@retiredDateUtc", contract.RetiredDateUTC);
                values.Add("@remoteDeleteBehavior", contract.DeleteBehavior);
                values.Add("@versionId", contract.Version.ToString());
                values.Add("@id", contract.Id.ToString());

                ExecuteWrite(query, values);
            }
        }

        public Guid GetContractId()
        {
            Guid contractId = Guid.Empty;

            var query = InternalSQLStatements.SQLLite.GET_DB_CONTRACT_ID;
            DataTable dt = ExecuteRead(query);
            foreach (DataRow row in dt.Rows)
            {
                string id = Convert.ToString(row["CONTRACT_ID"]) ?? string.Empty;
                contractId = Guid.Parse(id);
            }

            return contractId;
        }

        public DatabaseContract? GetActiveContract(bool fillSchema = false)
        {
            DatabaseContract contract = null;
            var query = InternalSQLStatements.SQLLite.GET_ACTIVE_DB_CONTRACT;
            var values = new Dictionary<string, object>();
            values.Add("@date", DateTime.MinValue);
            DataTable dt = ExecuteRead(query, values);
            foreach (DataRow row in dt.Rows)
            {
                contract = DatabaseContract.Parse(row);
                break;
            }

            if (fillSchema)
            {
                var schema = new DatabaseSchema();
                schema.DatabaseName = _databaseName;

                // need to fill out tables
                DatabaseRemoteStatusTable[] tables = GetStatusTables();
                foreach (var table in tables)
                {
                    // need to get the table schema
                    var tableSchema = new TableSchema();
                    tableSchema.DatabaseName = _databaseName;
                    tableSchema.TableName = table.TableName;

                    Guid tableId = Guid.Empty;

                    string sql = @$"
                    SELECT 
                        TABLE_ID 
                    FROM 
                        {InternalSQLStatements.TableNames.COOP.DATA_HOST_TABLES}
                    WHERE
                        TABLE_NAME = '{table.TableName}';
                    ";

                    DataTable dttid = ExecuteRead(sql);
                    if (dttid.Rows.Count > 0)
                    {
                        tableId = Guid.Parse(Convert.ToString(dttid.Rows[0][0]));
                    }

                    if (tableId != Guid.Empty)
                    {
                        tableSchema.TableId = tableId.ToString();
                    }

                    // how do we look up table schema in SQLite?
                    // https://www.sqlitetutorial.net/sqlite-describe-table/
                    // https://stackoverflow.com/questions/3268986/getting-table-schema-doesnt-seem-to-work-with-system-data-sqlite
                    DataTable dtSchema = _client.GetSchemaForTable(_databaseName, table.TableName);
                    foreach (DataRow dr in dtSchema.Rows)
                    {
                        var colSchema = new ColumnSchema();
                        colSchema.ColumnName = Convert.ToString(dr[1]) ?? string.Empty;
                        string colType = Convert.ToString(dr[2]) ?? string.Empty;
                        colSchema.ColumnType = (uint)SQLColumnTypeHelper.GetType(colType);

                        bool isNullable = Convert.ToBoolean(dr[3]);
                        colSchema.IsNullable = isNullable;

                        bool isPrimaryKey = Convert.ToBoolean(dr[5]);
                        colSchema.IsPrimaryKey = isPrimaryKey;

                        sql = $@"SELECT
                                    COLUMN_ID
                                 FROM 
                                    {InternalSQLStatements.TableNames.COOP.DATA_HOST_TABLE_COLUMNS}
                                WHERE
                                    TABLE_ID = '{tableId}'
                                AND
                                    COLUMN_NAME = '{colSchema.ColumnName}'
                                ";

                        DataTable dtColId = ExecuteRead(sql);
                        if (dtColId.Rows.Count > 0)
                        {
                            colSchema.ColumnId = Convert.ToString(dtColId.Rows[0][0]);
                        }

                        tableSchema.Columns.Add(colSchema);
                    }

                    schema.Tables.Add(tableSchema);
                }

                contract.Schema = schema;
            }

            return contract;
        }

        public bool GenerateContract(string description, RemoteDeleteBehavior deleteBehavior)
        {
            bool isSuccessful = false;
            var contracts = GetDatabaseContracts();

            if (contracts.Length == 0)
            {
                // this is the first contract ever
                var firstContract = new DatabaseContract();
                firstContract.Id = Guid.NewGuid();
                firstContract.GeneratedDateUTC = DateTime.UtcNow;
                firstContract.Description = description;
                firstContract.RetiredDateUTC = DateTime.MinValue;
                firstContract.Version = Guid.NewGuid();
                firstContract.DeleteBehavior = deleteBehavior;
                SaveContract(firstContract);
                isSuccessful = true;
            }
            else
            {
                // we need to find the contract that is not retired and retire it
                foreach (var contract in contracts)
                {
                    if (contract.RetiredDateUTC == DateTime.MinValue)
                    {
                        contract.RetiredDateUTC = DateTime.UtcNow;
                        SaveContract(contract);
                        break;
                    }
                }

                // now generate a new contract
                var newContract = new DatabaseContract();
                newContract.Id = contracts.FirstOrDefault().Id;
                newContract.GeneratedDateUTC = DateTime.UtcNow;
                newContract.Description = description;
                newContract.RetiredDateUTC = DateTime.MinValue;
                newContract.Version = Guid.NewGuid();
                newContract.DeleteBehavior = deleteBehavior;

                CreateAndPopulateDataHostTables();

                SaveContract(newContract);
                isSuccessful = true;
            }

            return isSuccessful;
        }
        #endregion

        #region Private Methods
        private void CreateAndPopulateDataHostTables()
        {
            if (!HasTable(InternalSQLStatements.TableNames.COOP.DATA_HOST))
            {
                _client.ExecuteWrite(_databaseName, InternalSQLStatements.SQLLite.CREATE_DATA_HOST);
            }

            if (!HasTable(InternalSQLStatements.TableNames.COOP.DATA_HOST_TABLES))
            {
                _client.ExecuteWrite(_databaseName, InternalSQLStatements.SQLLite.CREATE_HOST_TABLE);
            }

            if (!HasTable(InternalSQLStatements.TableNames.COOP.DATA_HOST_TABLE_COLUMNS))
            {
                _client.ExecuteWrite(_databaseName, InternalSQLStatements.SQLLite.CREATE_HOST_TABLE_COLUMNS);
            }

            string sql = $"SELECT COUNT(*) COUNT FROM {InternalSQLStatements.TableNames.COOP.DATA_HOST}";

            DataTable dt = ExecuteRead(sql);
            int totalRecords = Convert.ToInt32(dt.Rows[0][0]);

            if (totalRecords == 0)
            {
                // we need to generate the db id
                var dbId = Guid.NewGuid();
                sql = $@"INSERT INTO {InternalSQLStatements.TableNames.COOP.DATA_HOST}
                (DATABASE_ID, DATABASE_NAME) VALUES ('{dbId.ToString()}', '{_databaseName}');
                ";

                ExecuteWrite(sql);
            }

            PopulateDataTablesandSchemas();

        }

        private void PopulateDataTablesandSchemas()
        {
            string sql = string.Empty;
            DatabaseRemoteStatusTable[] tables = GetStatusTables();
            foreach (var table in tables)
            {
                sql = $@"SELECT 
                            COUNT(*) COUNT 
                        FROM {InternalSQLStatements.TableNames.COOP.DATA_HOST_TABLES}
                        WHERE 
                            TABLE_NAME = '{table.TableName}'
                ;
                ";

                DataTable dt = ExecuteRead(sql);
                int tblCount = Convert.ToInt32(dt.Rows[0][0]);
                Guid tableId = Guid.Empty;

                if (tblCount == 0)
                {
                    tableId = Guid.NewGuid();
                    sql = $@"
                    INSERT INTO {InternalSQLStatements.TableNames.COOP.DATA_HOST_TABLES}
                    (
                        TABLE_ID, 
                        TABLENAME
                    ) 
                    VALUES 
                    (
                        '{table.TableName}', 
                        '{tableId.ToString()}'
                    );
                    ";

                    ExecuteWrite(sql);
                }

                DataTable dtSchema = _client.GetSchemaForTable(_databaseName, table.TableName);
                foreach (DataRow dr in dtSchema.Rows)
                {
                    string columnName = Convert.ToString(dr[1]) ?? string.Empty;
                    sql = @$"SELECT 
                                COUNT(*) COUNT
                            FROM 
                                {InternalSQLStatements.TableNames.COOP.DATA_HOST_TABLE_COLUMNS}
                            WHERE
                                COLUMN_NAME = '{columnName}'
                            ;";

                    DataTable dtCol = ExecuteRead(sql);
                    int totalCol = Convert.ToInt32(dtCol.Rows[0][0]);

                    Guid colId = Guid.Empty;

                    if (totalCol == 0)
                    {
                        colId = Guid.NewGuid();
                        sql = $@"INSERT INTO {InternalSQLStatements.TableNames.COOP.DATA_HOST_TABLE_COLUMNS}
                        (
                            TABLE_ID,
                            COLUMN_ID,
                            COLUMN_NAME
                        )
                        VALUES
                        (
                            '{tableId.ToString()}',
                            '{colId.ToString()},
                            '{columnName}'
                        )
                        ;";

                        ExecuteWrite(sql);
                    }
                }
            }
        }

        private ActionOptionalResult CreateDbIfNotExists()
        {
            ActionOptionalResult actionResult = new ActionOptionalResult();

            if (!_client.HasDatabase(_databaseName))
            {
                _client.CreateDatabase(_databaseName);
                actionResult.Status = ActionOptionalResultStatus.Success;
            }
            else
            {
                actionResult.Status = ActionOptionalResultStatus.Information;
                actionResult.Message = $"Database {_databaseName} already exists";
            }

            return actionResult;
        }

        private ActionOptionalResult CreateParticipantTableIfNotExists()
        {
            ActionOptionalResult actionResult = new ActionOptionalResult();

            if (!HasTable(InternalSQLStatements.TableNames.COOP.PARTICIPANT))
            {
                _client.ExecuteWrite
                    (_databaseName,
                    InternalSQLStatements.SQLLite.CREATE_PARTICIPANT_TABLE
                    );
                actionResult.Status = ActionOptionalResultStatus.Success;
            }
            else
            {
                actionResult.Status = ActionOptionalResultStatus.Information;
                actionResult.Message = $"Table " +
                    $"{InternalSQLStatements.TableNames.COOP.PARTICIPANT} already exists";
            }

            return actionResult;

            throw new NotImplementedException();
        }

        private ActionOptionalResult CreateContractTableIfNotExists()
        {
            ActionOptionalResult actionResult = new ActionOptionalResult();

            if (!HasTable(InternalSQLStatements.TableNames.COOP.DATABASE_CONTRACT))
            {
                _client.ExecuteWrite
                    (_databaseName,
                    InternalSQLStatements.SQLLite.CREATE_DATABASE_CONTRACT_TABLE
                    );
                actionResult.Status = ActionOptionalResultStatus.Success;
            }
            else
            {
                actionResult.Status = ActionOptionalResultStatus.Information;
                actionResult.Message = $"Table " +
                    $"{InternalSQLStatements.TableNames.COOP.DATABASE_CONTRACT} already exists";
            }

            return actionResult;
        }

        private ActionOptionalResult CreateShadowTablesIfNotExists()
        {
            // for each table in the database, we need to create a "shadow"
            // if the logical storage policy defines it
            throw new NotImplementedException();
        }

        private ActionOptionalResult CreateRemotesTableIfNotExists()
        {
            ActionOptionalResult actionResult = new ActionOptionalResult();

            if (!HasTable(InternalSQLStatements.TableNames.COOP.REMOTES))
            {
                _client.ExecuteWrite
                    (_databaseName,
                    InternalSQLStatements.SQLLite.CREATE_REMOTE_TABLE
                    );
                actionResult.Status = ActionOptionalResultStatus.Success;
            }
            else
            {
                actionResult.Status = ActionOptionalResultStatus.Information;
                actionResult.Message = $"Table " +
                    $"{InternalSQLStatements.TableNames.COOP.REMOTES} already exists";
            }

            return actionResult;
        }


        #endregion
    }
}
