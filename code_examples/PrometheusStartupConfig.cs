app.Map("/metrics", metricsApp =>
{
    metricsApp.UseMetricServer("");
});

app.UseHttpMetrics();
Metrics.SuppressDefaultMetrics();