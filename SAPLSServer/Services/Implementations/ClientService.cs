using SAPLSServer.Constants;
using SAPLSServer.DTOs.Base;
using SAPLSServer.DTOs.Concrete.UserDtos;
using SAPLSServer.DTOs.Concrete.FileUploadDtos;
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
        private readonly IVehicleShareCodeService _vehicleShareCodeService;
        private readonly IFileService _fileService;

        public ClientService(IUserService userService, IVehicleShareCodeService vehicleShareCodeService, IClientProfileRepository clientProfileRepository, IFileService fileService)
        {
            _userService = userService;
            _vehicleShareCodeService = vehicleShareCodeService;

            _clientProfileRepository = clientProfileRepository;
            _fileService = fileService;
        }

        public async Task Create(CreateClientProfileRequest request)
        {
            bool citizenIdExists = await _clientProfileRepository.ExistsAsync(cp => cp.CitizenId == request.CitizenId);
            if (citizenIdExists)
                throw new InvalidInformationException(MessageKeys.CITIZEN_ID_ALREADY_EXISTS);
            
            string userId = await _userService.Create(request);
            var frontImageUploadRequest = new FileUploadRequest
            {
                File = request.FrontCitizenCardImage,
                Container = "citizen-id-cards",
                SubFolder = $"client-{userId}",
                GenerateUniqueFileName = true,
                Metadata = new Dictionary<string, string>
                {
                    { "UserId", userId },
                    { "ImageType", "FrontCitizenIdCard" }
                }
            };

            var frontImageResult = await _fileService.UploadFileAsync(frontImageUploadRequest);


            var backImageUploadRequest = new FileUploadRequest
            {
                File = request.BackCitizenCardImage,
                Container = "citizen-id-cards",
                SubFolder = $"client-{userId}",
                GenerateUniqueFileName = true,
                Metadata = new Dictionary<string, string>
                {
                    { "UserId", userId },
                    { "ImageType", "BackCitizenIdCard" }
                }
            };

            var backImageResult = await _fileService.UploadFileAsync(backImageUploadRequest);


            var clientProfile = new ClientProfile
            {
                UserId = userId,
                CitizenId = request.CitizenId,
                DateOfBirth = request.DateOfBirth,
                Sex = request.Sex,
                Nationality = request.Nationality,
                PlaceOfOrigin = request.PlaceOfOrigin,
                PlaceOfResidence = request.PlaceOfResidence,
                ShareCode = _vehicleShareCodeService.GenerateShareCode(VehicleShareCodeService.VEHICLE_SHARE_CODE_LENGTH),
                FrontCitizenIdCardImageUrl = frontImageResult.CloudUrl,
                BackCitizenIdCardImageUrl = backImageResult.CloudUrl,
            };
            
            await _clientProfileRepository.AddAsync(clientProfile);
            await _clientProfileRepository.SaveChangesAsync();
        }

        public async Task Update(UpdateClientProfileRequest request)
        {
            var clientProfile = await _clientProfileRepository.FindIncludingUser(request.Id);
            if (clientProfile == null)
                throw new InvalidInformationException(MessageKeys.CLIENT_PROFILE_NOT_FOUND);
            bool citizenIdExists = await _clientProfileRepository.ExistsAsync(cp => cp.CitizenId == request.CitizenId);
            if (citizenIdExists)
                throw new InvalidInformationException(MessageKeys.CITIZEN_ID_ALREADY_EXISTS);
                // Delete old front image if exists
            if (!string.IsNullOrEmpty(clientProfile.FrontCitizenIdCardImageUrl))
            {
                await _fileService.DeleteFileByUrlAsync(clientProfile.FrontCitizenIdCardImageUrl);
            }

            // Upload new front image
            var frontImageUploadRequest = new FileUploadRequest
            {
                File = request.FrontIdCardImage,
                Container = "citizen-id-cards",
                SubFolder = $"client-{clientProfile.UserId}",
                GenerateUniqueFileName = true,
                Metadata = new Dictionary<string, string>
                {
                    { "UserId", clientProfile.UserId },
                    { "ImageType", "FrontCitizenIdCard" }
                }
            };

            var frontImageResult = await _fileService.UploadFileAsync(frontImageUploadRequest);

            // Delete old back image if exists
            if (!string.IsNullOrEmpty(clientProfile.BackCitizenIdCardImageUrl))
            {
                await _fileService.DeleteFileByUrlAsync(clientProfile.BackCitizenIdCardImageUrl);
            }

            // Upload new back image
            var backImageUploadRequest = new FileUploadRequest
            {
                File = request.BackIdCardImage,
                Container = "citizen-id-cards",
                SubFolder = $"client-{clientProfile.UserId}",
                GenerateUniqueFileName = true,
                Metadata = new Dictionary<string, string>
                {
                    { "UserId", clientProfile.UserId },
                    { "ImageType", "BackCitizenIdCard" }
                }
            };

            var backImageResult = await _fileService.UploadFileAsync(backImageUploadRequest);

            clientProfile.CitizenId = request.CitizenId;
            clientProfile.DateOfBirth = request.DateOfBirth;
            clientProfile.Sex = request.Sex;
            clientProfile.Nationality = request.Nationality;
            clientProfile.PlaceOfOrigin = request.PlaceOfOrigin;
            clientProfile.PlaceOfResidence = request.PlaceOfResidence;
            clientProfile.FrontCitizenIdCardImageUrl = frontImageResult.CloudUrl;
            clientProfile.BackCitizenIdCardImageUrl = backImageResult.CloudUrl;
            clientProfile.User.UpdatedAt = DateTime.UtcNow;

            _clientProfileRepository.Update(clientProfile);
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
            var criterias = new List<Expression<Func<ClientProfile, bool>>>();

            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                criterias.Add(cp => cp.User.Status == request.Status);
            }
            if (!string.IsNullOrWhiteSpace(request.SearchCriteria))
            {
                criterias.Add(cp => cp.User.FullName.Contains(request.SearchCriteria) ||
                        cp.User.Email.Contains(request.SearchCriteria) ||
                        cp.User.Phone.Contains(request.SearchCriteria) ||
                        cp.CitizenId.Contains(request.SearchCriteria));
            }
            var criteriasArray = criterias.ToArray();
            var totalCount = await _clientProfileRepository.CountAsync(criteriasArray);
            var clients = await _clientProfileRepository.GetPageAsync(
                                        pageRequest.PageNumber, pageRequest.PageSize, 
                                        criteriasArray , null, request.Order == OrderType.Asc.ToString());
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

        public async Task<ClientProfileDetailsDto?> GetByCitizenIdNo(string citizenIdNo)
        {
            var clientProfile = await _clientProfileRepository.FindIncludingUserReadOnly([c => c.CitizenId == citizenIdNo])
                ?? throw new InvalidInformationException(MessageKeys.CLIENT_PROFILE_NOT_FOUND);
            return new ClientProfileDetailsDto(clientProfile);
        }

        public async Task<ClientProfileDetailsDto?> GetByUserId(string userId)
        {
            var clientProfile = await _clientProfileRepository.FindIncludingUserReadOnly([c => c.UserId == userId])
                ?? throw new InvalidInformationException(MessageKeys.CLIENT_PROFILE_NOT_FOUND);
            return new ClientProfileDetailsDto(clientProfile);
        }

        public async Task<ClientProfileSummaryDto> GetUserIdByShareCode(string shareCode)
        {
            var clientProfile = await _clientProfileRepository.FindIncludingUserReadOnly([c => c.ShareCode == shareCode])
                ?? throw new InvalidInformationException(MessageKeys.CLIENT_PROFILE_NOT_FOUND);
            return new ClientProfileSummaryDto(clientProfile);
        }

        public async Task UpdateDeviceToken(string userId, string? deviceToken)
        {
            var client = await _clientProfileRepository.FindIncludingUser(userId) 
                ?? throw new InvalidInformationException(MessageKeys.USER_NOT_FOUND);
            client.DeviceToken = deviceToken;
            client.User.UpdatedAt = DateTime.UtcNow;
            _clientProfileRepository.Update(client);
            await _clientProfileRepository.SaveChangesAsync();
        }

        public async Task<string?> GetDeviceToken(string userId)
        {
            var client = await _clientProfileRepository.Find(userId);
            if(client == null)
            {
                return null;
            }
            return client.DeviceToken;
        }
    }
}