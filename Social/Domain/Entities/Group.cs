
using Domain.Entities.Base;

namespace Domain.Entities;

public class Group : BaseAuditable
{
    public string Name { get; set; }
    public ICollection<GroupMembership> GroupMemberships { get; set; }
    public ICollection<Post> Posts { get; set; }
}
