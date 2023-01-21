using System;
using Azure_Functions_DotNet7.Data;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Azure_Functions_DotNet7.Functions
{
    public class QueueTriggerInsertDepartment
    {
        private readonly ILogger _logger;
        private readonly IDepartmentsRepository _departmentsRepository;
        JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true
        };


        public QueueTriggerInsertDepartment(ILoggerFactory loggerFactory, IDepartmentsRepository departmentsRepository)
        {
            _logger = loggerFactory.CreateLogger<QueueTriggerInsertDepartment>();
            _departmentsRepository = departmentsRepository;
        }

        [Function("QueueTriggerInsertDepartment")]
        public void Run([QueueTrigger("queue-departments", Connection = "MyStorageConnection")] string myQueueItem)
        {
            _logger.LogInformation($"Message from queue: {myQueueItem}");
            try
            {
                Department? department = JsonSerializer.Deserialize<Department>(myQueueItem, _options);
                var id = _departmentsRepository.InsertDepartment(department);
                _logger.LogInformation($"Department inserted, Id: {id.ToString()}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Queue Message could not be processed, Exception: {ex.Message}");
            }
            
        }
    }
}

