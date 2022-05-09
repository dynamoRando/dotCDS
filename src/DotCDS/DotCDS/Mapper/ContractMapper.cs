using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotCDS.Model;
using DotCDS.Common;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using DotCDS.Common.Enum;

namespace DotCDS.Mapper
{
    internal class ContractMapper
    {
        public static Contract Map(DatabaseContract contract)
        {
            var messageContract = new Contract();
            messageContract.ContractGUID = contract.Id.ToString();
            messageContract.ContractVersion = contract.Version.ToString();
            messageContract.Description = contract.Description;
            messageContract.GeneratedDate = contract.GeneratedDateUTC.ToUniversalTime().ToTimestamp();
            messageContract.Schema = contract.Schema;

            return messageContract;
        }

        public static DatabaseContract Map(Contract contract)
        {
            var dbContract = new DatabaseContract();
            dbContract.Schema = contract.Schema;

            dbContract.Id = Guid.Parse(contract.ContractGUID);
            dbContract.GeneratedDateUTC = DateTime.Parse(contract.GeneratedDate.ToString());
            dbContract.Description = contract.Description;
            dbContract.Version = Guid.Parse(contract.ContractVersion);
            dbContract.Status = (ContractStatus)contract.Status;
            dbContract.HostId = Guid.Parse(contract.HostInfo.HostGUID);
            
            return dbContract;
        }
    }
}
