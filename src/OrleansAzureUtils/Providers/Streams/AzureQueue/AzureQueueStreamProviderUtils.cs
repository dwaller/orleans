using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Orleans.AzureUtils;
using Orleans.Providers.Streams.Common;
using Orleans.Runtime;
using Orleans.Streams;

namespace Orleans.Providers.Streams.AzureQueue
{
    /// <summary>
    /// Utility functions for azure queue Persistent stream provider.
    /// </summary>
    public class AzureQueueStreamProviderUtils
    {
        /// <summary>
        /// Helper method for testing.
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="deploymentId"></param>
        /// <param name="storageConnectionString"></param>
        /// <returns></returns>
        public static async Task DeleteAllUsedAzureQueues(string providerName, string deploymentId, string storageConnectionString)
        {
            if (deploymentId != null)
            {
                var queueMapper = new HashRingBasedStreamQueueMapper(AzureQueueAdapterFactory.DEFAULT_NUM_QUEUES, providerName);
                List<QueueId> allQueues = queueMapper.GetAllQueues().ToList();

                var deleteTasks = new List<Task>();
                foreach (var queueId in allQueues)
                {
                    var manager = new AzureQueueDataManager(queueId.ToString(), deploymentId, storageConnectionString);
                    deleteTasks.Add(manager.DeleteQueue());
                }

                await Task.WhenAll(deleteTasks);
            }
        }
    }
}
