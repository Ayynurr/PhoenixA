using Application.Abstracts;
using Persistance.DataContext;

namespace Persistance.Concretes;

public class SearchService : ISearchService
{
    readonly AppDbContext _dbcontext;

    public SearchService(AppDbContext dbcontext)
    {
        _dbcontext = dbcontext;
    }

    public async Task<List<AppUser>> Search(string? username,string? groupname)
    {
        var query = _dbcontext.Users.AsQueryable(); 
        if (username != null)
        {
            query = query.Where(u => u.UserName == username); 
        }

        if (groupname != null)
        {
            query = query.Where(u => u.GroupMemberships.Any(gm => gm.Group.Name == groupname));
        }

        List<AppUser> users = await query.ToListAsync();
        return users;

    }
}
