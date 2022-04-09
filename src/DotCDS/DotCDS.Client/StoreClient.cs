using DotCDS.Common;
using Grpc.Net.Client;

namespace DotCDS.Client
{
    public sealed class StoreClient
    {
        #region Private Fields
        private CooperativeSQLService.CooperativeSQLServiceClient _client;
        private GrpcChannel _channel;
        private string _url;
        #endregion

        #region Public Properties
        #endregion

        #region Constructors
        #endregion

        #region Public Methods
        public ExecuteReadReply ExecuteSQLRead(uint dbType, string sqlStatement, string databaseName, string userName, string pw)
        {
            var statement = new ExecuteReadRequest();
            statement.Authentication = GetAuthRequest(userName, pw);
            statement.DatabaseName = databaseName;
            statement.SqlStatement = sqlStatement;
            statement.DatabaseType = dbType;

            return _client.ExecuteRead(statement);
        }

        public ExecuteCooperativeReadReply ExecuteSQLCooperativeRead()
        {
            // why did I write this?
            throw new NotImplementedException();
        }

        /// <summary>
        /// Executes a SQL statement for an INSERT, UPDATE, DELETE
        /// </summary>
        /// <param name="dbType">The type of database</param>
        /// <param name="sqlStatement">The SQL statement to execute</param>
        /// <param name="databaseName">The name of the database to execute against</param>
        /// <param name="userName">The username to the CDS instance</param>
        /// <param name="pw"></param>
        /// <returns></returns>
        public ExecuteWriteReply ExecuteSQLWrite(uint dbType, string sqlStatement, string databaseName, string userName, string pw)
        {
            var statement = new ExecuteWriteRequest();
            statement.Authentication = GetAuthRequest(userName, pw);
            statement.DatabaseName = databaseName;
            statement.SqlStatement = sqlStatement;
            statement.DatabaseType = dbType;

            return _client.ExecuteWrite(statement);
        }

        /// <summary>
        /// Executes a SQL statement for a particular participant for an INSERT, UPDATE, DELETE
        /// </summary>
        /// <param name="dbType">The type of database</param>
        /// <param name="participantAlias">The alias of the participant we want to execute this against</param>
        /// <param name="particpantId">The id of the participant. This can be empty if the alias has been provided</param>
        /// <param name="sqlStatement">The SQL statement to execute</param>
        /// <param name="databaseName">The name of the database to execute against</param>
        /// <param name="userName">The username to the CDS instance</param>
        /// <param name="pw"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ExecuteCooperativeWriteReply ExecuteSQLCooperativeWrite(uint dbType, string participantAlias, Guid particpantId, string sqlStatement, string databaseName, string userName, string pw)
        {
            var statement = new ExecuteCooperativeWriteRequest();
            statement.Authentication = GetAuthRequest(userName, pw);
            statement.DatabaseName = databaseName;
            statement.SqlStatement = sqlStatement;
            statement.DatabaseType = dbType;
            statement.Alias = participantAlias;
            statement.ParticipantId = particpantId.ToString();

            return _client.ExecuteCooperativeWrite(statement);
        }

        public HasTableReply HasTable(uint dbType, string tableName, string databaseName, string userName, string pw)
        {
            var statement = new HasTableRequest();
            statement.Authentication = GetAuthRequest(userName, pw);
            statement.DatabaseName = databaseName;
            statement.TableName = tableName;

            return _client.HasTable(statement);
        }

        public void Configure(string url, int sqlPortNumber)
        {
            string completeUrl = url + ":" + sqlPortNumber.ToString();
            _url = completeUrl;

            _channel = GrpcChannel.ForAddress(completeUrl);
            _client = new CooperativeSQLService.CooperativeSQLServiceClient(_channel);
        }

        public bool IsConfigured()
        {
            return _url != null;
        }

        public bool IsOnline()
        {
            var testRequest = new TestRequest();
            var reply = _client.IsOnline(testRequest);
            return reply != null;
        }

        public bool CreateDatabase(string databaseName, string userName, string pw)
        {
            var request = new CreateUserDatabaseRequest();
            request.Authentication = GetAuthRequest(userName, pw);
            request.DatabaseName = databaseName;

            var reply = _client.CreateUserDatabase(request);
            return reply.IsCreated;
        }

        /// <summary>
        /// Configures the target database to have the needed tables for cooperation
        /// </summary>
        /// <param name="databaseName">The database to add cooperative tables to</param>
        /// <param name="userName">The username to the CDS instance</param>
        /// <param name="pw"></param>
        /// <returns>A result describing if the action was sucessful</returns>
        /// <exception cref="NotImplementedException"></exception>
        public EnableCoooperativeFeaturesReply EnableCooperativeFeatures(string databaseName, string userName, string pw)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Configures the logical data storage policy for table in a database that has cooperative features enabled
        /// </summary>
        /// <param name="databaseName">The name of the database</param>
        /// <param name="tableName">The name of the table</param>
        /// <param name="policyMode">The mode to configure the table for cooperative data</param>
        /// <param name="userName">The username to the CDS instance</param>
        /// <param name="pw"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public SetLogicalStoragePolicyReply SetLogicalStoragePolicy(string databaseName, string tableName, uint policyMode, string userName, string pw)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the logical storage policy of the table in the database specified
        /// </summary>
        /// <param name="databaseName">The name of the database</param>
        /// <param name="tableName">The name of the table</param>
        /// <param name="userName">The username to the CDS instance</param>
        /// <param name="pw"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public GetLogicalStoragePolicyReply GetLogicalStoragePolicy(string databaseName, string tableName, string userName, string pw)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generates a cooperative contract for the specified database. 
        /// </summary>
        /// <param name="hostName">The host name that will be used to identify this CDS instane to others</param>
        /// <param name="description">A description of the contract</param>
        /// <param name="databaseName">The name of the database this contract is for</param>
        /// <param name="userName">The username to the CDS instance</param>
        /// <param name="pw"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// <remarks>Before a contract can be generated for a database, logical storage policies must be set on all tables 
        /// and cooperative features enabled on the database</remarks>
        public GenerateContractReply GenerateContract(string hostName, string description, string databaseName, string userName, string pw)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a list of pending contracts that the CDS has not yet accepted or rejected
        /// </summary>
        /// <param name="userName">The username to the CDS instance</param>
        /// <param name="pw"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ViewPendingContractsReply ViewPendingContracts(string userName, string pw)
        {
            throw new NotImplementedException();
        }
       
        /// <summary>
        /// Adds a participant to a host database at this CDS
        /// </summary>
        /// <param name="name">The name or alias of the participant to add</param>
        /// <param name="ip4address">The IP4 address of the participant</param>
        /// <param name="ip6address">The IP6 address of the participant</param>
        /// <param name="portNumber">The port number of the participant - this is the data port</param>
        /// <param name="databaseName">The database that this participant wiil be cooperating with</param>
        /// <param name="userName">The username to the CDS instance</param>
        /// <param name="pw"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public AddParticipantReply AddParticipant(string name, string ip4address, string ip6address, uint portNumber, string databaseName, string userName, string pw)
        {
            throw new NotImplementedException();
        }

      
        #endregion

        #region Private Methods
        private AuthRequest GetAuthRequest(string userName, string pw)
        {
            var authRequest = new AuthRequest();
            authRequest.UserName = userName;
            authRequest.Pw = pw;

            return authRequest;
        }
        #endregion

    }
}