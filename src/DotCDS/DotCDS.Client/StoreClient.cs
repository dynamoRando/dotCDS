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
            var auth = new AuthRequest();
            auth.UserName = userName;
            auth.Pw = pw;

            var statement = new ExecuteReadRequest();
            statement.Authentication = auth;
            statement.DatabaseName = databaseName;
            statement.SqlStatement = sqlStatement;
            statement.DatabaseType = dbType;

            return _client.ExecuteRead(statement);
        }

        public ExecuteWriteReply ExecuteSQLWrite(uint dbType, string sqlStatement, string databaseName, string userName, string pw)
        {
            var auth = new AuthRequest();
            auth.UserName = userName;
            auth.Pw = pw;

            var statement = new ExecuteWriteRequest();
            statement.Authentication = auth;
            statement.DatabaseName = databaseName;
            statement.SqlStatement = sqlStatement;
            statement.DatabaseType = dbType;

            return _client.ExecuteWrite(statement);
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
            var authRequest = new AuthRequest();
            authRequest.UserName = userName;
            authRequest.Pw = pw;

            var request = new CreateUserDatabaseRequest();
            request.Authentication = authRequest;
            request.DatabaseName = databaseName;

            var reply = _client.CreateUserDatabase(request);
            return reply.IsCreated;
        }
        #endregion

        #region Private Methods

        #endregion

    }
}