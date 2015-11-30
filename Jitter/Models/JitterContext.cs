using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Jitter.Models
{
    public class JitterContext : DbContext 
    {
        // IDbSet, IQueryable
        public virtual DbSet<JitterUser> JitterUsers { get; set; }
        public virtual DbSet<Jot> Jots { get; set; }
    }
}