using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prometheus;
using Serilog;
using Serilog.Events;

namespace InvoiceHandlingService.Prometheus
{
    public class TimedOperation : IDisposable
    {
        readonly string _description;
        readonly LogEventLevel _level;
        readonly bool _isLoggged;
        private readonly ITimer _timer;

        private TimedOperation(ITimer timer, string description, LogEventLevel level, bool isLoggged)
        {
            Log.ForContext<TimedOperation>()
                .Write(level, $"{description} - started", _description);

            _description = description;
            _level = level;
            _isLoggged = isLoggged;

            _timer = timer;

        }

        public void Dispose()
        {
            var duration = _timer.ObserveDuration();

            var messageTemplate = "{description:l} took {time} ms to complete";
            if (_isLoggged)
            {
                Log.ForContext<TimedOperation>()
                    .Write(_level,
                        messageTemplate, _description,
                        duration.TotalMilliseconds);
            }
        }

        public static TimedOperation Create(ITimer timer, string description, LogEventLevel level = LogEventLevel.Information, bool isLogged = true) =>
            new TimedOperation(timer, description, level, isLogged);
    }
}
