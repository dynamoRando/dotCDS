using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotCDS.Common;
using DotCDS.Model;
using Grpc.Net.Client;

namespace DotCDS
{
    /// <summary>
    /// Manages all the connections via gRPC to remote participants 
    /// </summary>
    internal class RemoteNetworkManager
    {
        #region Private Fields
        public List<RemoteParticipant> _participants;
        #endregion

        #region Public Properties
        #endregion

        #region Constructors
        public RemoteNetworkManager()
        {
            _participants = new List<RemoteParticipant>();
        }
        #endregion

        #region Public Methods
        public bool SendContractToParticipant(DatabaseParticipant participant, DatabaseContract contract)
        {
            RemoteParticipant remote = GetOrAddParticipant(participant);
            return remote.SaveContract(contract);
        }
        #endregion

        #region Private Methods
        private RemoteParticipant GetOrAddParticipant(DatabaseParticipant participant)
        {
            if (!HasParticipant(participant.Alias))
            {
                var remoteParticipant = new RemoteParticipant(participant);
                _participants.Add(remoteParticipant);
            }

            return GetParticipant(participant.Alias) ?? new RemoteParticipant(new DatabaseParticipant());
        }

        private RemoteParticipant? GetParticipant(string alias)
        {
            foreach (var participant in _participants)
            {
                if (string.Equals(alias, participant.Alias, StringComparison.OrdinalIgnoreCase))
                {
                    return participant;
                }
            }

            return null;
        }

        private bool HasParticipant(string alias)
        {
            foreach (var participant in _participants)
            {
                if (string.Equals(participant.Alias, alias, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
        #endregion

    }
}
