﻿using DotCDS.Common;
using DotCDS.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using DotCDS.Common.Enum;

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
            CreateIfNotExists();
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

        public bool HasTable(string tableName)
        {
            return _client.HasTable(_databaseName, tableName);
        }

        public bool EnableCooperativeFeatures()
        {
            throw new NotImplementedException();
        }

        public bool SetLogicalStoragePolicy(string tableName, LogicalStoragePolicy policy)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Private Methods
        private void CreateIfNotExists()
        {
            if (!_client.HasDatabase(_databaseName))
            {
                _client.CreateDatabase(_databaseName);
            }
        }
        #endregion

    }
}
