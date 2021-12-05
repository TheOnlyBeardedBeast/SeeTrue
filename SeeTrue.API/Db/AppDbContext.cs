using System;
using Microsoft.EntityFrameworkCore;
using SeeTrue.Models;

namespace SeeTrue.API.Db
{
    public class AppDbContext : SeeTrueDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
