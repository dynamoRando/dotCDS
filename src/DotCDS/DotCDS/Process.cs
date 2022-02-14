namespace DotCDS
{
    public class Process
    {
        #region Private Fields
        private Settings _settings;
        private string _rootPath;
        #endregion

        #region Public Properties
        public Settings Settings => _settings;
        #endregion

        #region Constructors
        public Process()
        {
        }

        public Process(string rootPath)
        {

        }
        #endregion

        #region Public Methods
        public void Start()
        {
            LoadConfiguration();
        }

        #endregion

        #region Private Methods
        private void LoadConfiguration()
        {
            _settings = new Settings();
            _settings = new Configurator().Load();
        }
        #endregion

    }
}