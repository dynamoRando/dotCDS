using DotCDS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS
{
    /// <summary>
    /// Manages all the available gRPC services exposed by a CDS process
    /// </summary>
    internal class NetworkManager
    {
        #region Private Fields
        private DatabaseServiceServer _databaseServiceServer;
        private DatabaseServiceHandler _databaseServiceHandler;
        private PortSettings _databaseServicePort;
        #endregion

        #region Public Properties
        #endregion

        #region Constructors
        #endregion

        #region Public Methods
        /// <summary>
        /// Starts the Database Service with the supplied parameters
        /// </summary>
        /// <param name="useHttps">If the connection should use HTTPS or not</param>
        public void StartServerForDatabaseService(bool useHttps)
        {
            string clientUrl;

            if (_databaseServiceHandler is null)
            {
                _databaseServiceHandler = new DatabaseServiceHandler();
            }

            if (useHttps)
            {
                clientUrl = $"https://{_databaseServicePort.IPAddress}:{_databaseServicePort.PortNumber.ToString()}";
            }
            else
            {
                clientUrl = $"http://{_databaseServicePort.IPAddress}:{_databaseServicePort.PortNumber.ToString()}";
            }

            string[] urls = new string[1];
            urls[0] = clientUrl;

            if (_databaseServiceServer is null)
            {
                _databaseServiceServer = new DatabaseServiceServer();
            }

            _databaseServiceServer.RunAsync(null, urls, _databaseServiceHandler, _databaseServicePort);
        }

        public void StopServerForDatabaseService()
        {
            if (_databaseServiceServer is not null)
            {
                _databaseServiceServer.StopAsync();
            }
        }
        #endregion

        #region Private Methods
        #endregion
    }
}
