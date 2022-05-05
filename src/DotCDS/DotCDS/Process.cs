using DotCDS.Common.Enum;
using DotCDS.Database;
using DotCDS.DatabaseClient;
using DotCDS.Query;
using DotCDS.Services;

namespace DotCDS
{
    /// <summary>
    /// Represents the root CDS process
    /// </summary>
    public class Process
    {
        #region Private Fields
        private Settings _settings;
        private Configurator _configurator;
        private string _rootPath;
        private bool _overrideDefaultDb = false;
        private DatabaseClientType _clientType;
        private string _connectionString;
        private SqliteCDSStore _cooperativeStore;
        private NetworkManager _networkManager;
        private RemoteNetworkManager _remoteNetworkManager;
        private SqliteUserDatabaseManager _userDatabaseManager;
        private QueryParser _queryParser;
        #endregion

        #region Public Properties
        public Settings Settings => _settings;
        #endregion

        #region Constructors
        public Process()
        {
            _networkManager = new NetworkManager();
        }

        public Process(string rootPath) : this()
        {
            _rootPath = rootPath;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Loads the appsettings.json file and attempts to startup the services
        /// </summary>
        public void Start()
        {
            LoadConfiguration();
            ConfigureBackingStore();
            _networkManager.SetRootFolder(_rootPath);
        }

        /// <summary>
        /// Starts the process with the supplied settings object
        /// </summary>
        /// <param name="settings"></param>
        public void Start(Settings settings)
        {
            _settings = settings;
            ConfigureBackingStore();
            _networkManager.SetRootFolder(_rootPath);
        }

        /// <summary>
        /// Use for testing purposes only. Configures a default admin username and pw for the CDS process.
        /// </summary>
        /// <param name="userName">The user name</param>
        /// <param name="pw">The pw</param>
        public void Test_SetupAdmin(string userName, string pw)
        {
            if (!_cooperativeStore.HasLogin(userName))
            {
                _cooperativeStore.CreateLogin(userName, pw);
            }

            if (!_cooperativeStore.UserIsInRole(userName, InternalSQLStatements.RoleNames.SYS_ADMIN))
            {
                _cooperativeStore.AddUserToRole(userName, InternalSQLStatements.RoleNames.SYS_ADMIN);
            }
        }

        /// <summary>
        /// Used in xunit tests to make sure we can access the setting
        /// </summary>
        /// <returns></returns>
        public string TestConnectionString()
        {
            return _configurator.TestDefaultConnection();
        }

        public void StartDatabaseService()
        {
            PortSettings settings = new PortSettings();
            settings.IPAddress = _settings.DefaultIP4;
            settings.PortNumber = _settings.DatabaseServicePort;

            _networkManager.StartServerForDatabaseService(settings, true);
        }

        public void StartDatabaseService(int overrideSettingsPortNumber, bool overrideSettingsUseHttps)
        {
            PortSettings settings = new PortSettings();
            settings.IPAddress = _settings.DefaultIP4;
            settings.PortNumber = overrideSettingsPortNumber;

            _networkManager.StartServerForDatabaseService(settings, overrideSettingsUseHttps);
        }

        public void StartSQLService()
        {
            PortSettings settings = new PortSettings();
            settings.IPAddress = _settings.DefaultIP4;
            settings.PortNumber = _settings.SQLServicePort;

            _networkManager.StartServerForSqlService(settings, true);
        }

        public void StartSQLService(int overrideSettingsPortNumber, bool overrideSettingsUseHttps)
        {
            PortSettings settings = new PortSettings();
            settings.IPAddress = _settings.DefaultIP4;
            settings.PortNumber = overrideSettingsPortNumber;

            _networkManager.StartServerForSqlService(settings, overrideSettingsUseHttps);
        }

        public void StartAdminService()
        {
            PortSettings settings = new PortSettings();
            settings.IPAddress = _settings.DefaultIP4;
            settings.PortNumber = _settings.AdminServicePort;

            _networkManager.StartServerForAdminService(settings, true);
        }

        public void StartAdminService(int overrideSettingsPortNumber, bool overrideSettingsUseHttps)
        {
            PortSettings settings = new PortSettings();
            settings.IPAddress = _settings.DefaultIP4;
            settings.PortNumber = overrideSettingsPortNumber;

            _networkManager.StartServerForAdminService(settings, overrideSettingsUseHttps);
        }

        public bool HasLogin(string userName)
        {
            return _cooperativeStore.HasLogin(userName);
        }

        public bool IsValidLogin(string username, string pw)
        {
            return _cooperativeStore.IsValidLogin(username, pw);    
        }

        public bool IsAdminLogin(string userName)
        {
            return _cooperativeStore.UserIsInRole(userName, InternalSQLStatements.RoleNames.SYS_ADMIN);
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Loads the appsettings file
        /// </summary>
        private void LoadConfiguration()
        {
            _settings = new Settings();
            _configurator = new Configurator();
            _settings = _configurator.Load();
        }

        /// <summary>
        /// Checks the backing database to see if it needs to be setup
        /// </summary>
        private void ConfigureBackingStore()
        {
            const string unknownDbType = "Unknown client databasetype";

            if (_overrideDefaultDb)
            {
                if (string.IsNullOrEmpty(_rootPath))
                {
                    _rootPath = Settings.RootFolder;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(_rootPath))
                {
                    _rootPath = Settings.RootFolder;
                }
                _clientType = (DatabaseClientType)Settings.DatabaseClientType;
                _connectionString = _configurator.DefaultConnection();
            }

            if (!Directory.Exists(_rootPath))
            {
                Directory.CreateDirectory(_rootPath);
            }

            _networkManager.SetRootFolder(_rootPath);

            switch (_clientType)
            {
                case DatabaseClientType.Unknown:
                    throw new InvalidOperationException(unknownDbType);
                case DatabaseClientType.SQLServer:
                    //_cooperativeStore = new SQLServerClient();
                    throw new NotImplementedException();
                    break;
                case DatabaseClientType.Postgres:
                    //_cooperativeStore = new PostgresClient();
                    throw new NotImplementedException();
                    break;
                case DatabaseClientType.Sqlite:
                    _cooperativeStore = new SqliteCDSStore(Settings.BackingDatabaseName, _rootPath);
                    _userDatabaseManager = new SqliteUserDatabaseManager(_rootPath);

                    _queryParser = new QueryParser();
                    _queryParser.SetSqliteDatabaseManager(_userDatabaseManager);

                    _networkManager.SetQueryParser(_queryParser);
                    _networkManager.SetSqliteUserDatabaseManager(_userDatabaseManager);
                    _networkManager.SetCooperativeStore(_cooperativeStore);
                    _networkManager.SetClientType(_clientType);
                    _networkManager.SetRemoteNetworkManager(new RemoteNetworkManager());
                  
                    break;
                default:
                    throw new InvalidOperationException(unknownDbType);

            }
        }

        #endregion
    }
}