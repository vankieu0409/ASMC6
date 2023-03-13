﻿using ASMC6.Server.Infrastructures.Data;
using ASMC6.Server.Infrastructures.Repositories.Interfaces;
using ASMC6.Shared.Entities;

using EF.Support.RepositoryAsync;

namespace ASMC6.Server.Infrastructures.Repositories.Implements;

public class UserRepository : RepositoryAsync<Users>, IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context) : base(context, context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
}