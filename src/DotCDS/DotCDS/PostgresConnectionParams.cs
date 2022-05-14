using System;
namespace DotCDS
{
	public record PostgresConnectionParams
	{
		public string Host { get; set; }
		public string Un { get; set; }
		public string Pw { get; set; }
		public string DatabaseName { get; set; }

		public static PostgresConnectionParams FromConnectionString(string cs)
        {
			//"Host=prod.home;Username=cds;Password=$ecret;Database=cds"
			var items = cs.Split(";");

			string host = string.Empty;
			string un = string.Empty;
			string pw = string.Empty;
			string dbName = string.Empty;

			foreach(var item in items)
            {
				if (item.Contains("Host"))
                {
					var sHost = item.Replace("Host=", string.Empty);
					sHost = sHost.Replace(";", string.Empty);
					host = sHost.Trim();
                }

				if (item.Contains("Username"))
				{
					var sUn = item.Replace("Username=", string.Empty);
					sUn = sUn.Replace(";", string.Empty);
					un = sUn.Trim();
				}

				if (item.Contains("Password"))
				{
					var sPw = item.Replace("Password=", string.Empty);
					sPw = sPw.Replace(";", string.Empty);
					pw = sPw.Trim();
				}

				if (item.Contains("Database"))
				{
					var sDb = item.Replace("Database=", string.Empty);
					sDb = sDb.Replace(";", string.Empty);
					dbName = sDb.Trim();
				}
			}

			return new PostgresConnectionParams { DatabaseName = dbName, Host = host, Un = un, Pw = pw };
        }
	}
}

