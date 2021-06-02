private static Summary _asyncWorkerProcessWorkItemDuration;
private static Counter _asyncWorkerErrorsCounter;

//AsyncWorker metrics example

public AsyncWorker(..., MetricsFactory metricsFactory)
{
    ...
    _asyncWorkerProcessWorkItemDuration = metricsFactory.CreateSummary("async_worker_process_work_item_duration_seconds",
        "Summary of AsyncWorker operation durations");
    _asyncWorkerErrorsCounter = metricsFactory.CreateErrorCounter("async_worker_errors_total",
        "Number of errors thrown in the AsyncWorker");
}

//Error metrics

_asyncWorkerErrorsCounter.WithLabels("Enqueue", "Exception").Inc();
_asyncWorkerErrorsCounter.WithLabels("GetWorkItem", "NotFoundException").Inc();

//Timer metric

using (TimedOperation.Create(_asyncWorkerProcessWorkItemDuration.WithLabels("process_work_item").NewTimer(),
            "Processes a work item from the queue in the AsyncWorker", LogEventLevel.Debug))
{
    ...
}

//Creating metrics in class constructors

_billRunLogicErrorsCounter = metricsFactory.CreateErrorCounter("bill_run_logic_errors_total",
    "Number of errors thrown in the BillRunLogic");
_billRunInvoicesPerBatchCount = metricsFactory.CreateBusinessCounter("bill_run_invoices_per_batch_total",
    "Number of invoices per batch by billRunId");
_billRunOperationDurationSummary = metricsFactory.CreateSummary("bill_run_operation_durations",
    "Summary of operation timers for BillRunLogic operations");

_invoiceLogicErrorsCounter = metricsFactory.CreateErrorCounter("invoice_logic_errors_total",
        "Number of errors thrown in the InvoiceLogic");
_invoiceLogicPositiveBalanceInvoicesHandledCounter = metricsFactory.CreateBusinessCounter("invoice_logic_positive_balance_invoices_handled_total",
    "Number of positive balance invoices handled in the InvoiceLogic");
_invoiceLogicNegativeBalanceInvoicesHandledCounter = metricsFactory.CreateBusinessCounter("invoice_logic_negative_balance_invoices_handled_total",
    "Number of negative balance invoices handled in the InvoiceLogic");
_invoiceLogicZeroBalanceInvoicesHandledCounter = metricsFactory.CreateBusinessCounter("invoice_logic_zero_balance_invoices_handled_total",
    "Number of zero balance invoices handled in the InvoiceLogic");

_mailLogicPaymentMethodCounter = metricsFactory.CreateBusinessCounter("mail_logic_payment_method_total",
"Number of payment methods used.");
_mailLogicErrorsCounter = metricsFactory.CreateErrorCounter("mail_logic_errors_total",
    "Number of errors thrown in the MailLogic");

    