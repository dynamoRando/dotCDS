using DotCDS.Common.Enum;
using DotCDS.Database;
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

        private AdminServiceServer _adminServiceServer;
        private AdminServiceHandler _adminServiceHandler;
        private PortSettings _adminServicePort;

        private SQLServiceServer _sqlServiceServer;
        private SQLServiceHandler _sqlServiceHandler;
        private PortSettings _sqlServicePort;

        private string _rootFolder = string.Empty;
        private DatabaseClientType _clientType;
        private SqliteCDSStore _store;
        private SqliteUserDatabaseManager _userDatabaseManager;
        #endregion

        #region Public Properties
        #endregion

        #region Constructors
        public NetworkManager()
        {

        }
        #endregion

        #region Public Methods
        public void SetSqliteUserDatabaseManager(SqliteUserDatabaseManager manager)
        {
            _userDatabaseManager = manager;
        }

        public void SetRootFolder(string rootFolder)
        {
            _rootFolder = rootFolder;
        }

        public void SetClientType(DatabaseClientType type)
        {
            _clientType = type;
        }

        public void SetCooperativeStore(SqliteCDSStore store)
        {
            _store = store;
        }

        /// <summary>
        /// Starts the Database Service with the supplied parameters
        /// </summary>
        /// <param name="portsettings">The settings to use for ip address and port number</param>
        /// <param name="useHttps">If the connection should use HTTPS or not</param>
        public void StartServerForDatabaseService(PortSettings portsettings, bool useHttps)
        {
            if (_databaseServicePort.PortNumber == 0)
            {
                _databaseServicePort = portsettings;
            }

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

        public void StartServerForSqlService(PortSettings portsettings, bool useHttps)
        {
            if (_sqlServicePort.PortNumber == 0)
            {
                _sqlServicePort = portsettings;
            }

            string clientUrl;

            if (_sqlServiceHandler is null)
            {
                _sqlServiceHandler = new SQLServiceHandler();
                _sqlServiceHandler.SetCooperativeStore(_store);

                switch (_clientType)
                {
                    case DatabaseClientType.Unknown:
                        throw new InvalidOperationException();
                    case DatabaseClientType.SQLServer:
                        throw new NotImplementedException();
                        break;
                    case DatabaseClientType.Postgres:
                        throw new NotImplementedException();
                        break;
                    case DatabaseClientType.Sqlite:
                        _sqlServiceHandler.SetSqliteClient(new SqliteClient(_rootFolder));
                        _sqlServiceHandler.SetSqliteUserDatabaseManager(_userDatabaseManager);
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }

            if (useHttps)
            {
                clientUrl = $"https://{_sqlServicePort.IPAddress}:{_sqlServicePort.PortNumber.ToString()}";
            }
            else
            {
                clientUrl = $"http://{_sqlServicePort.IPAddress}:{_sqlServicePort.PortNumber.ToString()}";
            }

            string[] urls = new string[1];
            urls[0] = clientUrl;

            if (_sqlServiceServer is null)
            {
                _sqlServiceServer = new SQLServiceServer();
            }

            _sqlServiceServer.RunAsync(null, urls, _sqlServiceHandler, _sqlServicePort);
        }

        public void StopServerForSqlService()
        {
            if (_sqlServiceServer is not null)
            {
                _sqlServiceServer.StopAsync();
            }
        }

        public void StartServerForAdminService(PortSettings portsettings, bool useHttps)
        {
            if (_adminServicePort.PortNumber == 0)
            {
                _adminServicePort = portsettings;
            }

            string clientUrl;

            if (_adminServiceHandler is null)
            {
                _adminServiceHandler = new AdminServiceHandler();
            }

            if (useHttps)
            {
                clientUrl = $"https://{_adminServicePort.IPAddress}:{_adminServicePort.PortNumber.ToString()}";
            }
            else
            {
                clientUrl = $"http://{_adminServicePort.IPAddress}:{_adminServicePort.PortNumber.ToString()}";
            }

            string[] urls = new string[1];
            urls[0] = clientUrl;

            if (_adminServiceServer is null)
            {
                _adminServiceServer = new AdminServiceServer();
            }

            _adminServiceServer.RunAsync(null, urls, _adminServiceHandler, _adminServicePort);
        }

        public void StopServerForAdminService()
        {
            if (_adminServiceServer is not null)
            {
                _adminServiceServer.StopAsync();
            }
        }
        #endregion

        #region Private Methods
        #endregion
    }
}
