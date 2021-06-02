using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prometheus;

namespace InvoiceHandlingService.Prometheus
{
    public class MetricsFactory
    {
        private readonly string _applicationName;
        public MetricsFactory(string applicationName)
        {
            _applicationName = applicationName;
        }

        public Summary CreateSummary(string name, string help)
        {
            return Metrics.CreateSummary(_applicationName + "_" + name, help,
                new SummaryConfiguration
                {
                    LabelNames = new[] { "method" },
                    Objectives = new[]
                    {
                        new QuantileEpsilonPair(0.25, 0.05),
                        new QuantileEpsilonPair(0.5, 0.05),
                        new QuantileEpsilonPair(0.75, 0.005),
                        new QuantileEpsilonPair(0.99, 0.005),
                    }
                });
        }
        public Counter CreateBusinessCounter(string name, string help)
        {
            return Metrics.CreateCounter(_applicationName + "_" + name, help, new CounterConfiguration
            {
                LabelNames = new[] { "kpi" }
            });
        }

        public Counter CreateErrorCounter(string name, string help)
        {
            return Metrics.CreateCounter(_applicationName + "_" + name, help, new CounterConfiguration
            {
                LabelNames = new[] {"method", "exceptionType"}
            });
        }

        public Gauge CreateGauge(string name, string help)
        {
            return Metrics.CreateGauge(_applicationName + "_" + name, help, new GaugeConfiguration
            {
                LabelNames = new[] {"method"}
            });
        }
        
    }
}
