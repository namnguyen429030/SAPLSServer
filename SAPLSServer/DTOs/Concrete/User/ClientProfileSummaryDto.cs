using SAPLSServer.Models;

namespace SAPLSServer.DTOs.Concrete
{
    public class ClientProfileSummaryDto : UserSummaryDto
    {
        public string CitizenId { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public bool Sex { get; set; }
        public string Nationality { get; set; } = null!;
        public string PlaceOfOrigin { get; set; } = null!;
        public string PlaceOfResidence { get; set; } = null!;

        public ClientProfileSummaryDto() { }

        public ClientProfileSummaryDto(ClientProfile model)
        {
            Id = model.UserId;
            Email = model.User.Email;
            FullName = model.User.FullName;
            CreatedAt = model.User.CreatedAt;
            Status = model.User.Status;
            CitizenId = model.CitizenId;
            DateOfBirth = model.DateOfBirth;
            Sex = model.Sex;
            Nationality = model.Nationality;
            PlaceOfOrigin = model.PlaceOfOrigin;
            PlaceOfResidence = model.PlaceOfResidence;
        }
    }
}
