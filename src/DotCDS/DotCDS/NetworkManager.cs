using DotCDS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS
{
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
        /// <param name="authenticationManager">An instance of an auth manager</param>
        /// <param name="dbManager">An instance of a db manager</param>
        /// <param name="cache">An instance of a cache manager</param>
        /// <param name="crypt">An instance of a crypt manager</param>
        /// <remarks>The managers passed in are normally used in the creation of a new database</remarks>
        public void StartServerForDatabaseService(bool useHttps)
        {
            string clientUrl;

            if (_databaseServiceHandler is null)
            {
                // this is a hack to try and avoid registering every single type with dependency injection
                // will need to research this again later. This is the result of the call from DrummerDB.Core.Service
                //_databaseServiceHandler = new DatabaseServiceHandler(authenticationManager, dbManager);

                /*
                     Message=Some services are not able to be constructed (Error while validating the service descriptor 'ServiceType: Drummersoft.DrummerDB.Core.Communication.DatabaseServiceHandler Lifetime: 
                     Singleton ImplementationType: Drummersoft.DrummerDB.Core.Communication.DatabaseServiceHandler': 
                     Unable to resolve service for type 'Drummersoft.DrummerDB.Core.IdentityAccess.Interface.IAuthenticationManager' while 
                     attempting to activate 'Drummersoft.DrummerDB.Core.Communication.DatabaseServiceHandler'.)
                     Source=Microsoft.Extensions.DependencyInjection

                     Inner Exception 1:
                     InvalidOperationException: Error while validating the service descriptor 
                     'ServiceType: Drummersoft.DrummerDB.Core.Communication.DatabaseServiceHandler Lifetime: Singleton ImplementationType: 
                     Drummersoft.DrummerDB.Core.Communication.DatabaseServiceHandler': Unable to resolve service for 
                     type 'Drummersoft.DrummerDB.Core.IdentityAccess.Interface.IAuthenticationManager' while attempting to activate 
                     'Drummersoft.DrummerDB.Core.Communication.DatabaseServiceHandler'.

                     Inner Exception 2:
                     InvalidOperationException: Unable to resolve service for type 'Drummersoft.DrummerDB.Core.IdentityAccess.Interface.IAuthenticationManager' 
                     while attempting to activate 'Drummersoft.DrummerDB.Core.Communication.DatabaseServiceHandler'.

                 */

                _databaseServiceHandler = new DatabaseServiceHandler();
                //_databaseServiceHandler.SetAuth(authenticationManager);
                //_databaseServiceHandler.SetDatabase(dbManager);
                //_databaseServiceHandler.SetStorage(storage);
                //_databaseServiceHandler.SetHostInfo(_hostInfo, _overrideDbPort);
                //_databaseServiceHandler.SetQueryManager(_queryManager);
                //_databaseServiceHandler.SetLogger(_logService);
                //_databaseServiceHandler.SetNotifications(_notifications);
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
