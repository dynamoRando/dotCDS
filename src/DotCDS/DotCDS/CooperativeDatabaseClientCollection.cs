using DotCDS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Net.Client;
using DotCDS.Common;

namespace DotCDS
{
    /// <summary>
    /// Represents a collection of Grpc Data Clients in a database (aka participants)
    /// </summary>
    internal class CooperativeDatabaseClientCollection
    {
        #region Private Fields
        private List<CooperativeDataClient> _clients;
        #endregion

        #region Public Properties
        #endregion

        #region Constructors
        public CooperativeDatabaseClientCollection()
        {
            _clients = new List<CooperativeDataClient>();
        }
        #endregion

        #region Public Methods
        public bool HasClient(string alias)
        {
            return _clients.Any(c => string.Equals(c.Alias, alias, StringComparison.OrdinalIgnoreCase));
        }

        public bool HasClient(Guid id)
        {
            return _clients.Any(c => c.Id == id);
        }

        public void AddClient(string alias, Guid id, string url, uint portNumber)
        {
            var client = new CooperativeDataClient(alias, id, url, portNumber);
            _clients.Add(client);
        }

        public CooperativeDataClient? GetClient(string alias)
        {
            CooperativeDataClient client = null;
            foreach (var c in _clients)
            {
                if (string.Equals(c.Alias, alias, StringComparison.OrdinalIgnoreCase))
                {
                    client = c;
                }
            }

            return client;
        }

        public CooperativeDataClient? GetClient(Guid id)
        {
            CooperativeDataClient client = null;
            foreach (var c in _clients)
            {
                if (c.Id == id)
                {
                    client = c;
                }
            }

            return client;
        }
        #endregion

        #region Private Methods
        #endregion

    }
}
