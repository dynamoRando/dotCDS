using DotCDS.Common.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS.Model
{
    internal class DatabaseContract
    {
        public Guid Id { get; set; }
        public DateTime GeneratedDateUTC { get; set; }
        public string Description { get; set; }
        public DateTime RetiredDateUTC { get; set; }
        public Guid Version { get; set; }
        public RemoteDeleteBehavior DeleteBehavior { get; set; }

        public static DatabaseContract Parse(DataRow row)
        {
            string id = Convert.ToString(row["CONTRACT_ID"]) ?? string.Empty;
            DateTime generatedDateUtc = Convert.ToDateTime(row["GENERATED_DATE_UTC"]);
            string description = Convert.ToString(row["DESCRIPTION"]) ?? string.Empty;
            DateTime retiredDateUtc = Convert.ToDateTime(row["RETIRED_DATE_UTC"]);
            string version = Convert.ToString(row["VERSION_ID"]) ?? string.Empty;
            uint deleteBehavior = Convert.ToUInt32(row["REMOTE_DELETE_BEHAVIOR"]);

            var contract = new DatabaseContract();
            contract.Id = Guid.Parse(id);
            contract.GeneratedDateUTC = generatedDateUtc;
            contract.Description = description;
            contract.RetiredDateUTC = retiredDateUtc;
            contract.Version = Guid.Parse(version);
            contract.DeleteBehavior = (RemoteDeleteBehavior)deleteBehavior;
            return contract;
        }
    }
}
