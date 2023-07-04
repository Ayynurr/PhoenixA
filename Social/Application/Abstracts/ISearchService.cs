using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstracts;

public interface ISearchService
{
    Task<List<AppUser>> Search(string? username,string? gorupname);
}
