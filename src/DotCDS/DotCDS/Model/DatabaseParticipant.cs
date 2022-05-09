using DotCDS.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DotCDS.Common.Enum;

namespace DotCDS.Model
{
    internal class DatabaseParticipant
    {
        public Guid InternalId { get; set; }
        public string Alias { get; set; }
        public string Ip4Address { get; set; }
        public string Ip6Address { get; set; }
        public uint Port { get; set; }
        public ContractStatus ContractStatus { get; set; }
        public Guid AcceptedContractVersion { get; set; }
        public byte[] Token { get; set; }
        public Guid ParticipantId { get; set; }

        public static DatabaseParticipant Parse(DataRow row)
        {
            string internalId = Convert.ToString(row["INTERNAL_PARTICIPANT_ID"]) ?? string.Empty;
            string alias = Convert.ToString(row["ALIAS"]) ?? string.Empty;
            string ip4Address = Convert.ToString(row["IP4ADDRESS"]) ?? string.Empty;
            string ip6Address = Convert.ToString(row["IP6ADDRESS"]) ?? string.Empty;
            uint port = Convert.ToUInt32(row["PORT"]);
            uint contractStatus = Convert.ToUInt32(row["CONTRACT_STATUS"]);
            string acceptedContractVersion = Convert.ToString(row["ACCEPTED_CONTRACT_VERSION_ID"]) ?? string.Empty;
            byte[] token = (byte[])row["TOKEN"];
            string id = Convert.ToString(row["PARTICIPANT_ID"]) ?? string.Empty;

            var particpant = new DatabaseParticipant();
            particpant.InternalId = internalId == string.Empty ? Guid.Empty : Guid.Parse(internalId);
            particpant.Alias = alias;
            particpant.Ip4Address = ip4Address;
            particpant.Ip6Address = ip6Address;
            particpant.Port = port;
            particpant.ContractStatus = (ContractStatus)contractStatus;
            particpant.AcceptedContractVersion = acceptedContractVersion == string.Empty ? Guid.Empty : Guid.Parse(acceptedContractVersion);
            particpant.Token = token;
            particpant.ParticipantId = id == string.Empty ? Guid.Empty : Guid.Parse(id);

            return particpant;
        }
    }
}
