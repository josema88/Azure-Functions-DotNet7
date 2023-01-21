using System.Net;
using Azure_Functions_DotNet7.Data;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Azure_Functions_DotNet7.Functions
{
    public class HttpInsertDepartment
    {
        private readonly ILogger _logger;
        private readonly IDepartmentsRepository _departmentsRepository;
        JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public HttpInsertDepartment(ILoggerFactory loggerFactory, IDepartmentsRepository departmentsRepository)
        {
            _logger = loggerFactory.CreateLogger<HttpInsertDepartment>();
            _departmentsRepository = departmentsRepository;
        }

        [Function("InsertDepartment")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "departments")] HttpRequestData req)
        {
            _logger.LogInformation("Insert");
            HttpResponseData response;
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Department? department = JsonSerializer.Deserialize<Department>(requestBody, _options);

            _logger.LogInformation("Name: " + department?.Name ?? "");
            if (department != null && !string.IsNullOrEmpty(department.Name))
            {
                try
                {
                    _departmentsRepository.InsertDepartment(department);
                    response = req.CreateResponse(HttpStatusCode.Created);
                }
                catch (Exception x)
                {
                    _logger.LogError("Exception: " + x.StackTrace);
                    response = req.CreateResponse(HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                response = req.CreateResponse(HttpStatusCode.BadRequest);
            }
            return response;
        }
    }
}

