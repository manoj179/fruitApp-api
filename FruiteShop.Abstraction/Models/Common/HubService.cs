using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FruiteShop.Abstraction.Models.Common
{
    public class HubService
    {
        private readonly ConcurrentDictionary<int, string> _connectDetails = new ConcurrentDictionary<int, string>();

        public ConcurrentDictionary<int, string> Connection => _connectDetails;

        public void addConnectDetailsToDict(int userId, string connectionId)
        {
            Connection.AddOrUpdate(userId, connectionId, (_, _) => connectionId);
        }

        public string getConnectionIdByUserId(int userId)
        {
            string connectionId = null;
            return Connection.TryGetValue(userId, out connectionId) ? connectionId : null;
        }

        public void removeConnectDetailsToDict(int userId)
        {
            Connection.TryRemove(userId, out _);
        }
    }
}
