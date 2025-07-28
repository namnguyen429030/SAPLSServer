using SAPLSServer.Constants;
using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete;
using SAPLSServer.Exceptions;
using SAPLSServer.Models;
using SAPLSServer.Repositories.Interfaces;
using SAPLSServer.Services.Interfaces;
using SAPLSServer.Helpers;

namespace SAPLSServer.Services.Implementations
{
    public class ClientService : IClientService
    {
        private readonly IClientProfileRepository _clientProfileRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;

        public ClientService(IUserService userService, IClientProfileRepository clientProfileRepository, IUserRepository userRepository)
        {
            _userService = userService;
            _clientProfileRepository = clientProfileRepository;
            _userRepository = userRepository;
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

            await _userRepository.SaveChangesAsync();
        }

        public async Task UpdateClient(UpdateClientProfileRequest request)
        {
            var clientProfile = await _clientProfileRepository.Find([cp => cp.UserId == request.Id]);
            if (clientProfile == null)
                throw new InvalidInformationException(MessageKeys.CLIENT_PROFILE_NOT_FOUND);

            clientProfile.CitizenId = request.CitizenId;
            clientProfile.CitizenIdCardImageUrl = request.CitizenIdCardImageUrl;
            clientProfile.DateOfBirth = request.DateOfBirth;
            clientProfile.Sex = request.Sex;
            clientProfile.Nationality = request.Nationality;
            clientProfile.PlaceOfOrigin = request.PlaceOfOrigin;
            clientProfile.PlaceOfResidence = request.PlaceOfResidence;
            _clientProfileRepository.Update(clientProfile);

            await _clientProfileRepository.SaveChangesAsync();
        }

        public async Task<ClientProfileDetailsDto?> GetClientProfileDetails(GetDetailsRequest request)
        {
            var clientProfile = await _clientProfileRepository.Find([cp => cp.UserId == request.Id]);
            return clientProfile == null ? null : new ClientProfileDetailsDto(clientProfile);
        }

        public async Task<PageResult<ClientProfileDetailsDto>> GetClientProfilesPage(PageRequest pageRequest, GetListRequest request)
        {
            var clients = await _clientProfileRepository.GetPageAsync(pageRequest.PageNumber, pageRequest.PageSize);
            var items = clients.Select(cp => new ClientProfileDetailsDto(cp)).ToList();

            return new PageResult<ClientProfileDetailsDto>
            {
                Items = items,
                TotalCount = items.Count,
                PageNumber = pageRequest.PageNumber,
                PageSize = pageRequest.PageSize,
            };
        }
    }
}