using System;
using Microsoft.AspNetCore.Identity;

namespace RideSharing.API.Repositories.Interface;

public interface ITokenRepository
{
    string CreateJwtToken(IdentityUser user, List<string> roles);
}
