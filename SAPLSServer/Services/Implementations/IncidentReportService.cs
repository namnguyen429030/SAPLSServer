using SAPLSServer.Constants;
using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.IncidenceReportDtos;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Exceptions;
using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;
using SAPLSServer.Services.Interfaces;
using System.Linq.Expressions;

namespace SAPLSServer.Services.Implementations
{
    public class IncidentReportService : IIncidentReportService
    {
        private readonly IIncidenceReportRepository _incidentReportRepository;
        private readonly IStaffService _staffService;
        private readonly IFileService _fileService;
        private readonly IParkingLotOwnerService _parkingLotOwnerService;
        private readonly IParkingLotService _parkingLotService;

        public IncidentReportService(
            IIncidenceReportRepository incidentReportRepository,
            IStaffService staffService, 
            IFileService fileService,
            IParkingLotOwnerService parkingLotOwnerService,
            IParkingLotService parkingLotService)
        {
            _incidentReportRepository = incidentReportRepository;
            _staffService = staffService;
            _fileService = fileService;
            _parkingLotOwnerService = parkingLotOwnerService;
            _parkingLotService = parkingLotService;
        }

        public async Task CreateIncidentReport(CreateIncidentReportRequest request, string reporterId)
        {
            var reporter = await _staffService.FindByStaffId(reporterId);
            if (reporter == null)
            {
                throw new InvalidInformationException(MessageKeys.STAFF_PROFILE_NOT_FOUND);
            }
            var incidentReport = new IncidenceReport
            {
                Id = Guid.NewGuid().ToString(),
                Header = request.Header,
                Priority = request.Priority,
                Description = request.Description,
                Status = IncidentReportStatus.Open.ToString(),
                ReporterId = reporterId,
                ParkingLotId = reporter.ParkingLotId,
                ReportedDate = DateTime.UtcNow
            };

            await _incidentReportRepository.AddAsync(incidentReport);
            await _incidentReportRepository.SaveChangesAsync();
        }

        public async Task UpdateIncidentReportStatus(UpdateIncidentReportStatusRequest request, string performerId)
        {
            var parkingLotOwner = await _parkingLotOwnerService.IsParkingLotOwnerValid(performerId);
            if (!parkingLotOwner)
                throw new InvalidInformationException(MessageKeys.PARKING_LOT_OWNER_PROFILE_NOT_FOUND);
            var incidentReport = await _incidentReportRepository.Find(request.Id) 
                ?? throw new InvalidInformationException(MessageKeys.INCIDENT_REPORT_NOT_FOUND);
            // Only the owner of the parking lot can update the status
            if (!await _parkingLotService.IsParkingLotOwner(incidentReport.ParkingLotId, performerId))
                throw new UnauthorizedAccessException(MessageKeys.UNAUTHORIZED_ACCESS);

            incidentReport.Status = request.Status;
            _incidentReportRepository.Update(incidentReport);
            await _incidentReportRepository.SaveChangesAsync();
        }

        public async Task<IncidentReportDetailsDto?> GetIncidentReportDetails(GetDetailsRequest request)
        {
            var incidentReport = await _incidentReportRepository.FindIncludeSenderInformationReadOnly(request.Id);
            if (incidentReport == null)
                return null;
            return new IncidentReportDetailsDto(incidentReport);
        }

        public async Task<PageResult<IncidentReportSummaryDto>> GetIncidentReportsPage(PageRequest pageRequest, GetIncidenReportListRequest request)
        {
            // Build filter criteria based on the request parameters
            var criterias = new List<Expression<Func<IncidenceReport, bool>>>();

            // Filter by parking lot (required field)
            if (!string.IsNullOrWhiteSpace(request.ParkingLotId))
            {
                criterias.Add(ir => ir.ParkingLotId == request.ParkingLotId);
            }

            // Filter by priority
            if (!string.IsNullOrWhiteSpace(request.Priority))
            {
                criterias.Add(ir => ir.Priority == request.Priority);
            }

            // Filter by status
            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                criterias.Add(ir => ir.Status == request.Status);
            }

            // Filter by date range
            if (request.StartDate.HasValue && request.EndDate.HasValue)
            {
                var startDateTime = request.StartDate.Value.ToDateTime(TimeOnly.MinValue);
                criterias.Add(ir => ir.ReportedDate >= startDateTime);
                var endDateTime = request.EndDate.Value.ToDateTime(TimeOnly.MaxValue);
                criterias.Add(ir => ir.ReportedDate <= endDateTime);
            }

            // Filter by search criteria (search in header and description)
            if (!string.IsNullOrWhiteSpace(request.SearchCriteria))
            {
                criterias.Add(ir => ir.Header.Contains(request.SearchCriteria) ||
                                   ir.Description.Contains(request.SearchCriteria));
            }


            // Determine sort order (default to descending for dates)
            bool isAscending = request.Order == OrderType.Asc.ToString();

            // Get total count for pagination
            var totalCount = await _incidentReportRepository.CountAsync(criterias.ToArray());

            // Get paginated results
            var incidentReports = await _incidentReportRepository.GetPageAsync(
                pageRequest.PageNumber,
                pageRequest.PageSize,
                criterias.ToArray(),
                null,
                isAscending);

            // Convert to DTOs
            var items = incidentReports.Select(ir => new IncidentReportSummaryDto(ir)).ToList();

            return new PageResult<IncidentReportSummaryDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageRequest.PageNumber,
                PageSize = pageRequest.PageSize
            };
        }
    }
}