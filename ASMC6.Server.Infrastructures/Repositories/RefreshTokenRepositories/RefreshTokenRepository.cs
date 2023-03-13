using ASMC6.Server.Infrastructures.Data;
using ASMC6.Shared.Entities;

using EF.Support.RepositoryAsync;

namespace ASMC6.Server.Infrastructures.Repositories.RefreshTokenRepositories;

public class RefreshTokenRepository : RepositoryAsync<RefreshTokenEntity>, IRefreshTokenRepository
{
    private readonly ApplicationDbContext _context;

    public RefreshTokenRepository(ApplicationDbContext context) : base(context, context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
}