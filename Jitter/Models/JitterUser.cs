using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Jitter.Models
{
    public class JitterUser : IComparable
    {
        [Key]
        public int JitterUserId { get; set; }

        [MaxLength(161)]
        public string Description { get; set; }
        public string FirstName { get; set; }

        [Required]
        [MaxLength(20)]
        [MinLength(3)]
        [RegularExpression(@"^[a-zA-Z\d]+[-_a-zA-Z\d]{0,2}[a-zA-Z\d]+")]
        public string Handle { get; set; }

        public string LastName { get; set; }
        public string Picture { get; set; }

        // ICollection, IEnumerable, IQueryable
        public List<Jot> Jots { get; set; }
        public List<JitterUser> Following { get; set; }

        public int CompareTo(object obj)
        {
            // Let's sort user based on their Handles b/c handles are strings AND...
            // .NET knows how to compare strings already. ha!

            // We need to explicitly cast from object type to JitterUser Type
            JitterUser other_user = obj as JitterUser;
            int answer = this.Handle.CompareTo(other_user.Handle);
            return answer;
        }
        //public List<JitterUser> Followers { get; set; } // Again, this is just one way to do this. Not the only way.
    }
}