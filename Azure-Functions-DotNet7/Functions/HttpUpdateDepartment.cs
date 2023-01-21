using System.Net;
using Azure_Functions_DotNet7.Data;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Azure_Functions_DotNet7.Functions
{
    public class HttpUpdateDepartment
    {
        private readonly ILogger _logger;
        private readonly IDepartmentsRepository _departmentsRepository;
        JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public HttpUpdateDepartment(ILoggerFactory loggerFactory, IDepartmentsRepository departmentsRepository)
        {
            _logger = loggerFactory.CreateLogger<HttpUpdateDepartment>();
            _departmentsRepository = departmentsRepository;
        }

        [Function("UpdateDepartment")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "departments/{id:int}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Update");
            HttpResponseData response;
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Department? department = JsonSerializer.Deserialize<Department>(requestBody, _options);

            _logger.LogInformation("Id: " + id);
            if (department != null)
            {
                try
                {
                    _departmentsRepository.UpdateDepartment(id, department);
                    response = req.CreateResponse(HttpStatusCode.OK);
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

