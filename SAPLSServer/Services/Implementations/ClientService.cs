using SAPLSServer.Constants;
using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.UserDto;
using SAPLSServer.DTOs.PaginationDto;
using SAPLSServer.Exceptions;
using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;
using SAPLSServer.Services.Interfaces;
using System.Linq.Expressions;

namespace SAPLSServer.Services.Implementations
{
    public class ClientService : IClientService
    {
        private readonly IClientProfileRepository _clientProfileRepository;
        private readonly IUserService _userService;

        public ClientService(IUserService userService, IClientProfileRepository clientProfileRepository)
        {
            _userService = userService;
            _clientProfileRepository = clientProfileRepository;
        }

        public async Task CreateClient(CreateClientProfileRequest request)
        {
            bool citizenIdExists = await _clientProfileRepository.ExistsAsync(cp => cp.CitizenId == request.CitizenId);
            if (citizenIdExists)
                throw new InvalidInformationException(MessageKeys.CITIZEN_ID_ALREADY_EXISTS);
            string userId = await _userService.CreateUser(request);

            var clientProfile = new ClientProfile
            {
                UserId = userId,
                CitizenId = request.CitizenId,
                DateOfBirth = request.DateOfBirth,
                Sex = request.Sex,
                Nationality = request.Nationality,
                PlaceOfOrigin = request.PlaceOfOrigin,
                PlaceOfResidence = request.PlaceOfResidence
            };
            await _clientProfileRepository.AddAsync(clientProfile);
        }

        public async Task UpdateClient(UpdateClientProfileRequest request)
        {
            var clientProfile = await _clientProfileRepository.FindIncludingUserReadOnly(request.Id);
            if (clientProfile == null)
                throw new InvalidInformationException(MessageKeys.CLIENT_PROFILE_NOT_FOUND);

            clientProfile.CitizenId = request.CitizenId;
            //clientProfile.CitizenIdCardImageUrl = request.CitizenIdCardImageUrl;
            clientProfile.DateOfBirth = request.DateOfBirth;
            clientProfile.Sex = request.Sex;
            clientProfile.Nationality = request.Nationality;
            clientProfile.PlaceOfOrigin = request.PlaceOfOrigin;
            clientProfile.PlaceOfResidence = request.PlaceOfResidence;
            _clientProfileRepository.Update(clientProfile);
            clientProfile.User.UpdatedAt = DateTime.UtcNow;
            await _clientProfileRepository.SaveChangesAsync();
        }

        public async Task<ClientProfileDetailsDto?> GetClientProfileDetails(GetDetailsRequest request)
        {
            var clientProfile = await _clientProfileRepository.FindIncludingUserReadOnly(request.Id);
            if (clientProfile == null)
                return null;
            return new ClientProfileDetailsDto(clientProfile);
        }

        public async Task<PageResult<ClientProfileSummaryDto>> GetClientProfilesPage(PageRequest pageRequest, GetClientListRequest request)
        {
            var criteriaList = new List<Expression<Func<ClientProfile, bool>>>();
            
            // Ch? thêm ?i?u ki?n khi giá tr? t?n t?i
            if (!string.IsNullOrEmpty(request.Status))
                criteriaList.Add(cp => cp.User.Status == request.Status);
                
            if (!string.IsNullOrEmpty(request.SearchCriteria))
                criteriaList.Add(cp => cp.User.FullName.Contains(request.SearchCriteria) ||
                                      cp.User.Email.Contains(request.SearchCriteria) ||
                                      cp.User.Phone.Contains(request.SearchCriteria) ||
                                      cp.CitizenId.Contains(request.SearchCriteria));
            
            var totalCount = await _clientProfileRepository.CountAsync(
                criteriaList.Count > 0 ? criteriaList.ToArray() : null);
                
            var clients = await _clientProfileRepository.GetPageAsync(
                                    pageRequest.PageNumber, pageRequest.PageSize, 
                                    criteriaList.Count > 0 ? criteriaList.ToArray() : null, 
                                    null, request.Order == OrderType.Asc.ToString());
            
            var items = new List<ClientProfileSummaryDto>();
            foreach (var client in clients)
            {
                var clientIncludingUser = await _clientProfileRepository.FindIncludingUserReadOnly(client.UserId);
                if(clientIncludingUser == null)
                    continue; // Skip if client profile is not found
                items.Add(new ClientProfileSummaryDto(clientIncludingUser));
            }
            
            return new PageResult<ClientProfileSummaryDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageRequest.PageNumber,
                PageSize = pageRequest.PageSize,
            };
        }

        public async Task<List<ClientProfileSummaryDto>> GetAllClients(GetClientListRequest request)
        {
            var criteriaList = new List<Expression<Func<ClientProfile, bool>>>();
            
            // Ch? thêm ?i?u ki?n khi giá tr? t?n t?i
            if (!string.IsNullOrEmpty(request.Status))
                criteriaList.Add(cp => cp.User.Status == request.Status);
                
            if (!string.IsNullOrEmpty(request.SearchCriteria))
                criteriaList.Add(cp => cp.User.FullName.Contains(request.SearchCriteria) ||
                                      cp.User.Email.Contains(request.SearchCriteria) ||
                                      cp.User.Phone.Contains(request.SearchCriteria) ||
                                      cp.CitizenId.Contains(request.SearchCriteria));
        
            var clients = await _clientProfileRepository.GetAllAsync(
                criteriaList.Count > 0 ? criteriaList.ToArray() : null, 
                null, 
                request.Order == OrderType.Asc.ToString());
                
            var result = new List<ClientProfileSummaryDto>();
            foreach(var client in clients) {
                var clientIncludingUser = await _clientProfileRepository.FindIncludingUserReadOnly(client.UserId);
                if(clientIncludingUser == null) {
                    continue;
                }
                result.Add(new ClientProfileSummaryDto(clientIncludingUser));
            }
            return result;
        }
    }
}