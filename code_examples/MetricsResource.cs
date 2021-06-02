using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Prometheus;

namespace InvoiceHandlingService.Prometheus
{
    public class MetricsResource : IMetricsResource
    {
        private readonly IAsyncWorker _asyncWorker;

        public MetricsResource(IAsyncWorker asyncWorker)
        {
            _asyncWorker = asyncWorker;

        }

        private static readonly Gauge AsyncWorkerItemsInQueueGauge =
            Metrics.CreateGauge("ihs_AsyncWorker_ItemsInQueue", "Counts the amount of total items in the current queue");
        private static readonly Gauge AsyncWorkerStatusSummary =
            Metrics.CreateGauge("ihs_AsyncWorker_Status", "Current status of the AsyncWorker. 1 = Running, 2 = Stopping, 3 = Stopped, 4 = StoppedError");

        public void CallbackRegister()
        {
            AsyncWorkerGetAllWorkItemsCallback();
            AsyncWorkerGetStatusCallback();

        }

        private void AsyncWorkerGetAllWorkItemsCallback()
        {
            Metrics.DefaultRegistry.AddBeforeCollectCallback(() =>
            {
                List<WorkItem> response = _asyncWorker.GetAllWorkItems();
                AsyncWorkerItemsInQueueGauge.Set(response.Count);
            });
        }

        private void AsyncWorkerGetStatusCallback()
        {
            Metrics.DefaultRegistry.AddBeforeCollectCallback(() =>
            {
                AsyncWorkerStatus response = _asyncWorker.GetStatus();
                AsyncWorkerStatusSummary.Set((double)response);
            });
        }
    }
}
