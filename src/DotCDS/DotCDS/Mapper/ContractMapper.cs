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

            throw new NotImplementedException();
        }
    }
}
