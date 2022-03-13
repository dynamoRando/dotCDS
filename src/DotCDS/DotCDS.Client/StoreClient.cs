﻿using DotCDS.Common;
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
        public StatementReply ExecuteSQL(string sqlStatement, string databaseName, string userName, string pw)
        {
            throw new NotImplementedException();
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
        #endregion

        #region Private Methods

        #endregion

    }
}