using ASMC6.Shared.Entities;

using EF.Support.RepositoryAsync;

namespace ASMC6.Server.Infrastructures.Repositories.UserRepositories;

public interface IUserRepository : IRepositoryAsync<UserEntity>
{
}