using DotCDS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotCDS.Common;
using Grpc.Net.Client;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using DotCDS.Mapper;
using System.Net;

namespace DotCDS
{
    /// <summary>
    /// Represents a participant and it's connection
    /// </summary>
    internal class RemoteParticipant
    {
        #region Private Fields
        private CooperativeDataService.CooperativeDataServiceClient _client;
        private GrpcChannel _channel;
        private string _url;
        #endregion

        #region Public Properties
        public DatabaseParticipant Participant { get; init; }
        public string Alias => Participant.Alias;
        #endregion

        #region Constructors
        public RemoteParticipant(DatabaseParticipant participant)
        {
            Participant = participant;

            string completeUrl = participant.Ip4Address + ":" + participant.Port.ToString();
            _url = completeUrl;

            _channel = GrpcChannel.ForAddress(completeUrl);
            _client = new CooperativeDataService.CooperativeDataServiceClient(_channel);
        }
        #endregion

        #region Public Methods
        public bool SaveContract(DatabaseContract contract, DatabaseHostInfo hostInfo)
        {
            var request = new SaveContractRequest();
            var messageContract = ContractMapper.Map(contract);
            var messageHost = HostMapper.Map(hostInfo);

            messageContract.HostInfo = messageHost;

            request.Contract = messageContract;
            request.MessageInfo = GetMessageInfo();
            
            var result = _client.SaveContract(request);

            return result.IsSaved;
        }
        #endregion

        #region Private Methods
        private MessageInfo GetMessageInfo()
        {
            var info = new MessageInfo();
            info.IsLittleEndian = BitConverter.IsLittleEndian;

            var addresses = Dns.GetHostAddresses(Dns.GetHostName());

            foreach (var address in addresses)
            {
                info.MessageAddresses.Add(address.ToString());
            }

            info.MessageGeneratedTimeUTC = DateTime.UtcNow.ToString();
            info.MessageGUID = Guid.NewGuid().ToString();

            return info;
        }
        #endregion



    }
}
