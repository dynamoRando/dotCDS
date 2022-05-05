using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS
{
    internal class CooperativeDataClient
    {

        #region Private Fields
        private Common.CooperativeDataService.CooperativeDataServiceClient _client;
        private string _alias;
        private Guid _id;
        private string _url;
        private uint _port;
        private GrpcChannel _channel;
        #endregion

        #region Public Properties
        public string Alias => _alias;
        public Guid Id => _id;
        public string Url => _url;
        public uint Port => _port;
        #endregion

        #region Constructors
        public CooperativeDataClient(string alias, Guid id, string url, uint port)
        {
            _alias = alias;
            _id = id;
            _url = url;
            _port = port;
        }
        #endregion

        #region Public Methods
        public Common.CooperativeDataService.CooperativeDataServiceClient GetClient()
        {
            if (_client is null)
            {
                string completeUrl = _url + ":" + _port.ToString();
                _url = completeUrl;

                _channel = GrpcChannel.ForAddress(completeUrl);
                _client = new Common.CooperativeDataService.CooperativeDataServiceClient(_channel);
            }

            return _client;
        }
        #endregion

        #region Private Methods
        #endregion

    }
}
