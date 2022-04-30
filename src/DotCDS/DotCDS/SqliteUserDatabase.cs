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
        #endregion

        #region Private Methods
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
