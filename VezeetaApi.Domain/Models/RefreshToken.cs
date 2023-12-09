using VezeetaApi.Domain.Dtos;

namespace VezeetaApi.Domain.Models
{
    public class RefreshToken : BaseEntity<int>
    {
        public string? Token { get; set; }
        public DateTime ExpiredOn { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiredOn;
        public new bool IsActive => UpdatedDate is null || !IsExpired;

        public string AppUserId { get; set; }
    }
}
