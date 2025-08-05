using SAPLSServer.Constants;
using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.IncidentReportDto;
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
        private readonly IStaffProfileRepository _staffProfileRepository;

        public IncidentReportService(
            IIncidenceReportRepository incidentReportRepository,
            IStaffProfileRepository staffProfileRepository)
        {
            _incidentReportRepository = incidentReportRepository;
            _staffProfileRepository = staffProfileRepository;
        }

        public async Task CreateIncidentReport(CreateIncidentReportRequest request, string reporterId)
        {
            // Verify that the reporter exists and is a staff member
            var staffProfile = await _staffProfileRepository.Find([sp => sp.UserId == reporterId]);
            if (staffProfile == null)
                throw new InvalidInformationException(MessageKeys.STAFF_PROFILE_NOT_FOUND);

            var incidentReport = new IncidenceReport
            {
                Id = Guid.NewGuid().ToString(),
                Header = request.Header,
                Priority = request.Priority,
                Description = request.Description,
                Status = request.Status,
                ReporterId = reporterId,
                ParkingLotId = request.ParkingLotId,
                ReportedDate = DateTime.UtcNow
            };

            await _incidentReportRepository.AddAsync(incidentReport);
            await _incidentReportRepository.SaveChangesAsync();
        }

        public async Task UpdateIncidentReportStatus(UpdateIncidentReportStatusRequest request)
        {
            var incidentReport = await _incidentReportRepository.Find(request.Id);
            if (incidentReport == null)
                throw new InvalidInformationException(MessageKeys.INCIDENT_REPORT_NOT_FOUND);

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
            if (!string.IsNullOrEmpty(request.ParkingLotId))
            {
                criterias.Add(ir => ir.ParkingLotId == request.ParkingLotId);
            }

            // Filter by priority
            if (!string.IsNullOrEmpty(request.Priority))
            {
                criterias.Add(ir => ir.Priority == request.Priority);
            }

            // Filter by status
            if (!string.IsNullOrEmpty(request.Status))
            {
                criterias.Add(ir => ir.Status == request.Status);
            }

            // Filter by date range
            if (request.StartDate.HasValue)
            {
                var startDateTime = request.StartDate.Value.ToDateTime(TimeOnly.MinValue);
                criterias.Add(ir => ir.ReportedDate >= startDateTime);
            }

            if (request.EndDate.HasValue)
            {
                var endDateTime = request.EndDate.Value.ToDateTime(TimeOnly.MaxValue);
                criterias.Add(ir => ir.ReportedDate <= endDateTime);
            }

            // Filter by search criteria (search in header and description)
            if (!string.IsNullOrEmpty(request.SearchCriteria))
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