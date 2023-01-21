using System.Net;
using Azure_Functions_DotNet7.Data;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Azure_Functions_DotNet7.Functions
{
    public class HttpGetDepartments
    {
        private readonly ILogger _logger;
        private readonly IDepartmentsRepository _departmentsRepository;

        public HttpGetDepartments(ILoggerFactory loggerFactory, IDepartmentsRepository departmentsRepository)
        {
            _logger = loggerFactory.CreateLogger<HttpGetDepartments>();
            _departmentsRepository = departmentsRepository;
        }

        [Function("GetDepartments")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "departments/{id:int?}")] HttpRequestData req, int? id)
        {
            _logger.LogInformation("Get Departments");
            HttpResponseData response;
            _logger.LogInformation("Parameter: " + id);
            try
            {
                response = req.CreateResponse(HttpStatusCode.OK);
                if (id != null )
                {
                    Department department = _departmentsRepository.GetDepartment((int) id);
                    await response.WriteAsJsonAsync(department);
                    if (department == null)
                        response = req.CreateResponse(HttpStatusCode.NotFound);
                }
                else
                {
                    List<Department> departments = _departmentsRepository.GetDepartments();
                    await response.WriteAsJsonAsync(departments);
                }
                
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