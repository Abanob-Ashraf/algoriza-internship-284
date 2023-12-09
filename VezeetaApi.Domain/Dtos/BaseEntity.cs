using VezeetaApi.Domain.Services;

namespace VezeetaApi.Domain.Dtos
{
    public class BaseEntity<T> : IActivatable
    {
        public T Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
