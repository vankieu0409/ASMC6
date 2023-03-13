using ASMC6.Server.Infrastructures.Data;
using ASMC6.Server.Infrastructures.Repositories.Interfaces;
using ASMC6.Shared.Entities;

using EF.Support.RepositoryAsync;

namespace ASMC6.Server.Infrastructures.Repositories.Implements;

public class RoleRepository : RepositoryAsync<Roles>, IRoleRepository
{
    private readonly ApplicationDbContext _context;

    public RoleRepository(ApplicationDbContext context) : base(context, context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
}