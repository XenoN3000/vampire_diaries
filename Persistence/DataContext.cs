using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Persistence;

public class DataContext: IdentityDbContext<AppUser>
{
    
}