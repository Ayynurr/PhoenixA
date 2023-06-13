using Domain.Entities.Entities;

namespace Domain.Entities.Base;

public class BaseAuditable : BaseEntity
{
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    
}
