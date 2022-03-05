using DotCDS.Common;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS.Client
{
    public static class Store
    {
        #region Private Fields
        private static SQLService.SQLServiceClient _client;
        private static GrpcChannel _channel;
        private static string _url;
        #endregion

        #region Public Properties
        #endregion

        #region Constructors
        #endregion

        #region Public Methods
        public static void Configure(string url, int portNumber)
        {
            string completeUrl = url + ":" + portNumber.ToString();
            _url = completeUrl;
        }

        public static bool IsConfigured()
        {
            return _url != null;
        }

        public static SQLQueryReply ExecuteSQL(string sqlStatement, string databaseName, string userName, string pw)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Private Methods
        #endregion

    }
}
