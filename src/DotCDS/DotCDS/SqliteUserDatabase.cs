using DotCDS.Common;
using DotCDS.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS
{
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
        }
        #endregion

        #region Public Methods
        public bool IsCooperative()
        {
            throw new NotImplementedException();
        }

        public bool HasParticipant(string participantAlias)
        {
            throw new NotImplementedException();
        }

        public bool HasParticipant(Guid participantId)
        {
            throw new NotImplementedException();
        }

        public Participant GetParticipant(string participantAlias)
        {
            throw new NotImplementedException();
        }

        public Participant GetParticipant(Guid participantId)
        {
            throw new NotImplementedException();
        }

        public Contract[] GetContracts()
        {
            throw new NotImplementedException();
        }

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
        #endregion

        #region Private Methods

        #endregion

    }
}
