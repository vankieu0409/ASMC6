using EF.Support.Entities.Interfaces;

using Microsoft.AspNetCore.Identity;

namespace ASMC6.Shared.Entities;

public class Roles : IdentityRole<Guid>, IEntity
{
}