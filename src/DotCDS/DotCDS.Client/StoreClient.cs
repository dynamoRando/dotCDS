using DotCDS.Common;
using Grpc.Net.Client;

namespace DotCDS.Client
{
    public sealed class StoreClient
    {
        #region Private Fields
        private SQLService.SQLServiceClient _client;
        private GrpcChannel _channel;
        private string _url;
        #endregion

        #region Public Properties
        #endregion

        #region Constructors
        #endregion

        #region Public Methods
        public SQLQueryReply ExecuteSQL(string sqlStatement, string databaseName, string userName, string pw)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Private Methods
        private void InitConnection(string url, int portNumber)
        {
            string completeUrl = url + ":" + portNumber.ToString();
            _url = completeUrl;

            _channel = GrpcChannel.ForAddress(completeUrl);
            _client = new SQLService.SQLServiceClient(_channel);
        }
        #endregion

    }
}