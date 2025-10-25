using Microsoft.AspNetCore.Mvc;
using MvdBackend.Models;
using MvdBackend.Repositories;
using MvdBackend.Services;
using MvdBackend.DTOs;
using NetTopologySuite.Geometries;
using System.Text.Json;
namespace MvdBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitizenRequestsController : ControllerBase
    {
        private readonly ICitizenRequestRepository _requestRepository;
        private readonly IRepository<Citizen> _citizenRepository;
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<RequestType> _typeRepository;
        private readonly IRepository<RequestStatus> _statusRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IDistrictRepository _districtRepository;
        private readonly IGeminiService _geminiService;
        private readonly INominatimService _nominatimService;
        private readonly ILogger<CitizenRequestsController> _logger;
        private readonly IAuditService _auditService;
        private readonly IServiceProvider _serviceProvider;

        public CitizenRequestsController(
            ICitizenRequestRepository requestRepository,
            IRepository<Citizen> citizenRepository,
            IRepository<Employee> employeeRepository,
            IRepository<RequestType> typeRepository,
            IRepository<RequestStatus> statusRepository,
            IRepository<Category> categoryRepository,
            IDistrictRepository districtRepository,
            IGeminiService geminiService,
            INominatimService nominatimService,
            ILogger<CitizenRequestsController> logger,
            IAuditService auditService,
            IServiceProvider serviceProvider
            )

        {
            _requestRepository = requestRepository;
            _citizenRepository = citizenRepository;
            _employeeRepository = employeeRepository;
            _typeRepository = typeRepository;
            _statusRepository = statusRepository;
            _categoryRepository = categoryRepository;
            _districtRepository = districtRepository;
            _geminiService = geminiService;
            _nominatimService = nominatimService;
            _logger = logger;
            _auditService = auditService;
            _serviceProvider = serviceProvider;
        }

        // GET: api/CitizenRequests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CitizenRequestDto>>> GetCitizenRequests(

            [FromQuery] int? categoryId,
            [FromQuery] int? districtId,
            [FromQuery] int? statusId)
        {
            var requests = await _requestRepository.GetAllWithBasicDetailsAsync();
            // Фильтрация
            if (categoryId.HasValue)
                requests = requests.Where(r => r.CategoryId == categoryId.Value).ToList();

            if (districtId.HasValue)
                requests = requests.Where(r => r.DistrictId == districtId.Value).ToList();

            if (statusId.HasValue)
                requests = requests.Where(r => r.RequestStatusId == statusId.Value).ToList();
            var dtos = requests.Select(cr => new CitizenRequestDto
            {
                Id = cr.Id,
                CitizenId = cr.CitizenId,
                RequestTypeId = cr.RequestTypeId,
                CategoryId = cr.CategoryId,
                Description = cr.Description,
                AcceptedById = cr.AcceptedById,
                AssignedToId = cr.AssignedToId,
                IncidentTime = cr.IncidentTime,
                CreatedAt = cr.CreatedAt,
                IncidentLocation = cr.IncidentLocation,
                CitizenLocation = cr.CitizenLocation,
                RequestStatusId = cr.RequestStatusId,
                DistrictId = cr.DistrictId,
                Latitude = cr.Location is NetTopologySuite.Geometries.Point p ? (double?)p.Y : null,
                Longitude = cr.Location is NetTopologySuite.Geometries.Point p2 ? (double?)p2.X : null,
                RequestNumber = cr.RequestNumber,
                AiCategory = cr.AiCategory,
                AiPriority = cr.AiPriority,
                AiSummary = cr.AiSummary,
                AiSuggestedAction = cr.AiSuggestedAction,
                AiSentiment = cr.AiSentiment,
                AiAnalyzedAt = cr.AiAnalyzedAt,
                IsAiCorrected = cr.IsAiCorrected,
                FinalCategory = cr.FinalCategory
            }).ToList();

            return Ok(dtos);
        }

        // GET: api/CitizenRequests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CitizenRequestDto>> GetCitizenRequest(int id)
        {
            var cr = await _requestRepository.GetWithDetailsAsync(id);
            if (cr == null) return NotFound();

            var dto = new CitizenRequestDto
            {
                Id = cr.Id,
                CitizenId = cr.CitizenId,
                RequestTypeId = cr.RequestTypeId,
                CategoryId = cr.CategoryId,
                Description = cr.Description,
                AcceptedById = cr.AcceptedById,
                AssignedToId = cr.AssignedToId,
                IncidentTime = cr.IncidentTime,
                CreatedAt = cr.CreatedAt,
                IncidentLocation = cr.IncidentLocation,
                CitizenLocation = cr.CitizenLocation,
                RequestStatusId = cr.RequestStatusId,
                DistrictId = cr.DistrictId,
                Latitude = cr.Location is Point p ? (double?)p.Y : null,
                Longitude = cr.Location is Point p2 ? (double?)p2.X : null,
                RequestNumber = cr.RequestNumber,
                AiCategory = cr.AiCategory,
                AiPriority = cr.AiPriority,
                AiSummary = cr.AiSummary,
                AiSuggestedAction = cr.AiSuggestedAction,
                AiSentiment = cr.AiSentiment,
                AiAnalyzedAt = cr.AiAnalyzedAt,
                IsAiCorrected = cr.IsAiCorrected,
                FinalCategory = cr.FinalCategory
            };


            return Ok(dto);
        }
        // GET: api/CitizenRequests/citizen/5
        [HttpGet("citizen/{citizenId}")]
        public async Task<ActionResult<IEnumerable<CitizenRequest>>> GetRequestsByCitizen(int citizenId)
        {
            var requests = await _requestRepository.GetRequestsByCitizenAsync(citizenId);
            return Ok(requests);
        }

        // GET: api/CitizenRequests/status/1
        [HttpGet("status/{statusId}")]
        public async Task<ActionResult<IEnumerable<CitizenRequest>>> GetRequestsByStatus(int statusId)
        {
            var requests = await _requestRepository.GetRequestsByStatusAsync(statusId);
            return Ok(requests);
        }

        // POST: api/CitizenRequests/analyze
        [HttpPost("analyze")]
        public async Task<ActionResult<GeminiAnalysisResponse>> AnalyzeRequest([FromBody] AnalyzeRequestDto request)
        {
            if (string.IsNullOrEmpty(request.Description))
            {
                return BadRequest("Description is required");
            }

            try
            {
                var analysis = await _geminiService.AnalyzeRequestAsync(request.Description);
                return Ok(analysis);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error analyzing request: {ex.Message}");
            }
        }

        // POST: api/CitizenRequests/generate-response
        [HttpPost("generate-response")]
        public async Task<ActionResult<string>> GenerateResponse([FromBody] AnalyzeRequestDto request)
        {
            if (string.IsNullOrEmpty(request.Description))
            {
                return BadRequest("Description is required");
            }

            try
            {
                var response = await _geminiService.GenerateResponseAsync(request.Description);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error generating response: {ex.Message}");
            }
        }
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateRequestStatusDto dto)
        {
            var request = await _requestRepository.GetByIdAsync(id);
            if (request == null) return NotFound("Обращение не найдено");

            request.RequestStatusId = dto.RequestStatusId;
            request.UpdatedAt = DateTime.UtcNow;

            _requestRepository.Update(request);
            await _requestRepository.SaveAsync();

            // Логирование аудита
            await _auditService.LogActionAsync(
                "UPDATE_STATUS",
                "CitizenRequest",
                request.Id,
                newValues: $"StatusId: {dto.RequestStatusId}",
                userId: request.AcceptedById,
                requestId: id
            );

            return Ok(new { message = "Статус обновлён", requestId = id });
        }
        [HttpPatch("{id}/assign")]
        public async Task<IActionResult> AssignExecutor(int id, [FromBody] AssignRequestDto dto)
        {
            var request = await _requestRepository.GetByIdAsync(id);
            if (request == null) return NotFound("Обращение не найдено");

            request.AssignedToId = dto.AssignedToId;
            request.UpdatedAt = DateTime.UtcNow;

            _requestRepository.Update(request);
            await _requestRepository.SaveAsync();

            await _auditService.LogActionAsync(
                "ASSIGN_EXECUTOR",
                "CitizenRequest",
                request.Id,
                newValues: $"AssignedTo: {dto.AssignedToId}",
                userId: request.AcceptedById,
                requestId: id
            );

            return Ok(new { message = "Исполнитель назначен", requestId = id });
        }
        [HttpPatch("{id}/reclassify")]
        public async Task<IActionResult> Reclassify(int id)
        {
            // Забираем обращение с деталями
            var request = await _requestRepository.GetByIdAsync(id);

            if (request == null)
                return NotFound("Обращение не найдено.");

            // Запускаем анализ AI на основе описания
            var analysis = await _geminiService.AnalyzeRequestAsync(request.Description);

            // Сохраняем результаты анализа AI (не изменяя фактическую категорию)
            request.AiCategory = analysis.Category;
            request.AiSummary = analysis.Summary;
            request.AiPriority = analysis.Priority;
            request.AiSentiment = analysis.Sentiment;
            request.AiSuggestedAction = analysis.SuggestedAction;
            request.AiAnalyzedAt = DateTime.UtcNow;

            request.UpdatedAt = DateTime.UtcNow;

            // Обновляем через репозиторий
            await _requestRepository.UpdateAsync(request);

            return Ok(new
            {
                message = "AI-классификация обновлена.",
                requestId = request.Id,
                suggestedCategory = request.AiCategory
            });
        }

        // POST: api/CitizenRequests
        [HttpPost]
        public async Task<ActionResult<CitizenRequest>> PostCitizenRequest([FromBody] CreateCitizenRequestDto dto)
        {
            try
            {
                var request = new CitizenRequest
                {
                    CitizenId = dto.CitizenId,
                    RequestTypeId = dto.RequestTypeId,
                    CategoryId = dto.CategoryId,
                    Description = dto.Description,
                    AcceptedById = dto.AcceptedById,
                    AssignedToId = dto.AssignedToId,
                    IncidentTime = dto.IncidentTime,
                    IncidentLocation = dto.IncidentLocation,
                    CitizenLocation = dto.CitizenLocation,
                    RequestStatusId = dto.RequestStatusId,
                    CreatedAt = DateTime.UtcNow
                };
                request.RequestNumber = Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();

                // Определяем координаты и район через Nominatim
                var geocodeResult = await _nominatimService.GeocodeAsync(request.IncidentLocation);
                if (geocodeResult != null)
                {
                    // Сохраняем координаты
                    request.Location = new Point(geocodeResult.Value.lon, geocodeResult.Value.lat);

                    // Сохраняем район
                    var district = await _districtRepository.GetByNameAsync(geocodeResult.Value.district);
                    if (district != null)
                    {
                        request.DistrictId = district.Id;
                        _logger.LogInformation($"District set to: {district.Name}");
                    }
                    else
                    {
                        _logger.LogWarning($"District '{geocodeResult.Value.district}' not found in database");
                    }
                }
                else
                {
                    // Fallback: используем Gemini если Nominatim не сработал
                    _logger.LogInformation("Nominatim failed, using Gemini fallback");
                    var districtName = await _geminiService.DetectDistrictAsync(request.IncidentLocation);
                    if (districtName != "Не определен")
                    {
                        var district = await _districtRepository.GetByNameAsync(districtName);
                        if (district != null)
                        {
                            request.DistrictId = district.Id;
                            _logger.LogInformation($"District set via Gemini: {district.Name}");
                        }
                    }
                }

                await _requestRepository.AddAsync(request);
                await _requestRepository.SaveAsync();

                _ = Task.Run(async () =>
                {
                    // СОЗДАЕМ НОВЫЙ КОНТЕКСТ ДЛЯ ФОНОВОЙ ЗАДАЧИ
                    using var scope = _serviceProvider.CreateScope();
                    var scopedServices = scope.ServiceProvider;
                    var backgroundRepository = scopedServices.GetRequiredService<ICitizenRequestRepository>();

                    try
                    {
                        var analysis = await _geminiService.AnalyzeRequestAsync(request.Description);

                        // ПОЛУЧАЕМ ЗАПРОС ЗАНОВО ИЗ БАЗЫ
                        var requestToUpdate = await backgroundRepository.GetByIdAsync(request.Id);
                        if (requestToUpdate == null) return;

                        // СОХРАНЯЕМ AI АНАЛИЗ В БД
                        requestToUpdate.AiCategory = analysis.Category;
                        requestToUpdate.AiPriority = analysis.Priority;
                        requestToUpdate.AiSummary = analysis.Summary;
                        requestToUpdate.AiSuggestedAction = analysis.SuggestedAction;
                        requestToUpdate.AiSentiment = analysis.Sentiment;
                        requestToUpdate.AiAnalyzedAt = DateTime.UtcNow;
                        requestToUpdate.FinalCategory = analysis.Category;

                        backgroundRepository.Update(requestToUpdate);
                        await backgroundRepository.SaveAsync();

                        _logger.LogInformation($"AI analysis saved for request #{request.Id}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error in AI analysis for request #{request.Id}");
                    }
                });

                var responseDto = new CitizenRequestCreatedDto
                {
                    Id = request.Id,
                    Description = request.Description,
                    IncidentLocation = request.IncidentLocation,
                    CitizenLocation = request.CitizenLocation,
                    DistrictId = request.DistrictId,
                    CreatedAt = request.CreatedAt,
                    Latitude = geocodeResult?.lat,
                    Longitude = geocodeResult?.lon,
                    RequestNumber = request.RequestNumber
                };
                // Логируем создание заявления
                await _auditService.LogActionAsync(
                    "CREATE",
                    "CitizenRequest",
                    request.Id,
                    newValues: JsonSerializer.Serialize(new
                    {
                        Description = request.Description,
                        Location = request.IncidentLocation,
                        Status = "Создано"
                    }),
                    userId: dto.AcceptedById,
                    requestId: request.Id
                );
                return CreatedAtAction("GetCitizenRequest", new { id = request.Id }, responseDto);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating request: {ex.Message}");
            }
        }

        // PUT: api/CitizenRequests/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCitizenRequest(int id, [FromBody] UpdateCitizenRequestDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("ID mismatch");
            }

            var existingRequest = await _requestRepository.GetByIdAsync(id);
            if (existingRequest == null)
            {
                return NotFound();
            }

            // Сохраняем старый статус для логирования
            var oldStatusId = existingRequest.RequestStatusId;

            // Обновляем только необходимые поля
            existingRequest.CitizenId = dto.CitizenId;
            existingRequest.RequestTypeId = dto.RequestTypeId;
            existingRequest.CategoryId = dto.CategoryId;
            existingRequest.Description = dto.Description;
            existingRequest.AcceptedById = dto.AcceptedById;
            existingRequest.AssignedToId = dto.AssignedToId;
            existingRequest.IncidentTime = dto.IncidentTime;
            existingRequest.IncidentLocation = dto.IncidentLocation;
            existingRequest.CitizenLocation = dto.CitizenLocation;
            existingRequest.RequestStatusId = dto.RequestStatusId;
            existingRequest.DistrictId = dto.DistrictId;

            _requestRepository.Update(existingRequest);
            await _requestRepository.SaveAsync();

            // Логируем изменение статуса если оно было
            if (oldStatusId != dto.RequestStatusId)
            {
                var oldStatus = await _statusRepository.GetByIdAsync(oldStatusId);
                var newStatus = await _statusRepository.GetByIdAsync(dto.RequestStatusId);

                await _auditService.LogRequestStatusChangeAsync(
                    existingRequest.Id,
                    oldStatus?.Name ?? "Unknown",
                    newStatus?.Name ?? "Unknown"
                );
            }

            return NoContent();
        }

        // DELETE: api/CitizenRequests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCitizenRequest(int id)
        {
            var request = await _requestRepository.GetByIdAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            // Сериализуем только нужные поля, а не весь объект
            var requestData = new
            {
                request.Id,
                request.Description,
                request.IncidentLocation,
                request.CitizenLocation,
                request.RequestStatusId,
                request.CategoryId,
                request.DistrictId,
                request.CreatedAt
            };

            await _auditService.LogActionAsync(
                "DELETE",
                "CitizenRequest",
                id,
                oldValues: JsonSerializer.Serialize(requestData)
            );

            _requestRepository.Remove(request);
            await _requestRepository.SaveAsync();

            return NoContent();
        }
        [HttpPatch("{id}/correct-category")]
        public async Task<IActionResult> CorrectAiCategory(int id, [FromBody] string correctCategory)
        {
            var request = await _requestRepository.GetByIdAsync(id);
            if (request == null) return NotFound();

            request.FinalCategory = correctCategory;
            request.IsAiCorrected = true;

            _requestRepository.Update(request);
            await _requestRepository.SaveAsync();

            _ = Task.Run(async () =>
            {
                // Логируем корректировку
                await _auditService.LogActionAsync(
                    "AI_CORRECTION",
                    "CitizenRequest",
                    id,
                    oldValues: request.AiCategory,
                    newValues: correctCategory
                );
            });

            return Ok();
        }
        [HttpGet("check/{requestNumber}")]
        public async Task<IActionResult> GetStatusByNumber(string requestNumber)
        {
            var request = await _requestRepository.GetAllWithBasicDetailsAsync();
            var r = request.FirstOrDefault(x => x.RequestNumber == requestNumber);

            if (r == null)
                return NotFound("Обращение не найдено");

            return Ok(new
            {
                Number = r.RequestNumber,
                Status = r.RequestStatusId,
                CreatedAt = r.CreatedAt,
                Category = r.CategoryId,
                Description = r.Description
            });
        }
        // GET: api/CitizenRequests/by-number/ABCD123456
        [HttpGet("by-number/{requestNumber}")]
        public async Task<ActionResult<CitizenRequestDto>> GetByRequestNumber(string requestNumber)
        {
            var cr = await _requestRepository.GetByRequestNumberAsync(requestNumber);

            if (cr == null)
                return NotFound($"No request found with number {requestNumber}");

            var dto = new CitizenRequestDto
            {
                Id = cr.Id,
                CitizenId = cr.CitizenId,
                RequestTypeId = cr.RequestTypeId,
                CategoryId = cr.CategoryId,
                Description = cr.Description,
                AcceptedById = cr.AcceptedById,
                AssignedToId = cr.AssignedToId,
                IncidentTime = cr.IncidentTime,
                CreatedAt = cr.CreatedAt,
                IncidentLocation = cr.IncidentLocation,
                CitizenLocation = cr.CitizenLocation,
                RequestStatusId = cr.RequestStatusId,
                DistrictId = cr.DistrictId,
                Latitude = cr.Location is Point p ? (double?)p.Y : null,
                Longitude = cr.Location is Point p2 ? (double?)p2.X : null,
                RequestNumber = cr.RequestNumber,
                AiCategory = cr.AiCategory,
                AiPriority = cr.AiPriority,
                AiSummary = cr.AiSummary,
                AiSuggestedAction = cr.AiSuggestedAction,
                AiSentiment = cr.AiSentiment,
                AiAnalyzedAt = cr.AiAnalyzedAt,
                IsAiCorrected = cr.IsAiCorrected,
                FinalCategory = cr.FinalCategory
            };

            return Ok(dto);
        }


    }
}
