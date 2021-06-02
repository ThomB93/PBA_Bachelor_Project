private static Summary _invoiceHandlingServiceSummary;
private static Counter _invoiceHandlingServiceErrorsCount;
private static Counter _invoiceHandlingServiceEmailsSentCount;
private static Gauge _invoiceHandlingServiceQueriesInProgress;


_invoiceHandlingServiceSummary = metricsFactory.CreateSummary("invoice_handling_service_operation_durations", "Summary of InvoiceHandling service operations");
_invoiceHandlingServiceErrorsCount = metricsFactory.CreateErrorCounter("invoice_handling_service_errors_total", "Number of errors thrown in the InvoiceHandling service.");
_invoiceHandlingServiceEmailsSentCount = metricsFactory.CreateBusinessCounter(
    "invoice_handling_service_emails_sent_total", "Number of Zuora Emails sent to Zuora Client");
_invoiceHandlingServiceQueriesInProgress = metricsFactory.CreateGauge("invoice_handling_service_queries_in_progress", "Number of ongoing queries to the ActionApi service");

//Timer metrics

using (TimedOperation.Create(_invoiceHandlingServiceSummary.WithLabels("get_invoice").NewTimer(),
                    "Get Invoice from ZuoraClient", LogEventLevel.Debug))
{
    ...
}

using (TimedOperation.Create(_invoiceHandlingServiceSummary.WithLabels("send_email").NewTimer(),
            "Send Zuora Email to ZuoraClient", LogEventLevel.Debug))
{
    ...
}

using (TimedOperation.Create(_invoiceHandlingServiceSummary.WithLabels("update_invoice").NewTimer(),
                    "Make UpdateInvoice request to ZuoraClient", LogEventLevel.Debug))
{
    ...
}

//Error metrics

if (e.HttpStatusCode == HttpStatusCode.NotFound)
{
    _invoiceHandlingServiceErrorsCount.WithLabels("UpdateInvoice", "NotFoundException").Inc();
    throw new NotFoundException($"Invoice with id '{invoiceId}' not found.", e);
}

_invoiceHandlingServiceErrorsCount.WithLabels("UpdateInvoice", "ExternalDependencyException").Inc();
    throw new ExternalDependencyException(e.HttpStatusCode, e.RequestUri, e.Content, mes, e, ExternalDependency.Zuora);