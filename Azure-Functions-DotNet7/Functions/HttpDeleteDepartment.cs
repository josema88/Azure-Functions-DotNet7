using System.Net;
using Azure_Functions_DotNet7.Data;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Azure_Functions_DotNet7.Functions
{
    public class HttpDeleteDepartment
    {
        private readonly ILogger _logger;
        private readonly IDepartmentsRepository _departmentsRepository;

        public HttpDeleteDepartment(ILoggerFactory loggerFactory, IDepartmentsRepository departmentsRepository)
        {
            _logger = loggerFactory.CreateLogger<HttpDeleteDepartment>();
            _departmentsRepository = departmentsRepository;
        }

        [Function("DeleteDepartment")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "departments/{id:int}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Delete");
            HttpResponseData response;

            _logger.LogInformation("Id: " + id);
            try
            {
                _departmentsRepository.DeleteDepartment(id);
                response = req.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception x)
            {
                _logger.LogError("Exception: " + x.StackTrace);
                response = req.CreateResponse(HttpStatusCode.InternalServerError);
            }


            return response;
        }
    }
}

