namespace Domain.Entities.Entities;
public class BaseEntity
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; } 
    public bool IsActived { get; set; }
}
