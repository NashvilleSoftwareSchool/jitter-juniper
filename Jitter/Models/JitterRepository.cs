using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Jitter.Models
{
    public class JitterRepository
    {
        private JitterContext _context;
        public JitterContext Context { get {return _context;}}

        public JitterRepository()
        {
            _context = new JitterContext();
        }

        public JitterRepository(JitterContext a_context)
        {
            _context = a_context;
        }

        public List<JitterUser> GetAllUsers()
        {
            var query = from users in _context.JitterUsers select users;
            return query.ToList();
        }

        public JitterUser GetUserByHandle(string handle)
        {
            // SQL: select * from JitterUsers AS users where users.Handle = handle;
            var query = from user in _context.JitterUsers where user.Handle == handle select user;
            // IQueryable<JitterUser> query = from user in _context.JitterUsers where user.Handle == handle select user;
            // We need to make sure there's exactly one user returned. hmmmm.

            return query.SingleOrDefault();
        }

        public bool IsHandleAvailable(string handle)
        {
            bool available = false;
            try
            {
                JitterUser some_user = GetUserByHandle(handle);
                if (some_user == null)
                {
                    available = true;
                }
            }
            catch (InvalidOperationException){}

            return available;
        }

        public List<JitterUser> SearchByHandle(string handle)
        {
            // SQL: select * from JitterUsers As users where users.Handle like '%handle%';
            // handleblah
            // blahhandle
            // blahhandleblah
            var query = from user in _context.JitterUsers select user;
            List<JitterUser> found_users = query.Where(user => user.Handle.Contains(handle)).ToList();
            found_users.Sort();
            return found_users;
        }

        public List<JitterUser> SearchByName(string search_term)
        {
            // SQL: select * from JitterUsers AS users where users.FirstName like '%search_term%' OR users.LastName like '%search_term%';
            var query = from user in _context.JitterUsers select user;
            List<JitterUser> found_users = query.Where(user => Regex.IsMatch(user.FirstName, search_term, RegexOptions.IgnoreCase) || Regex.IsMatch(user.LastName, search_term, RegexOptions.IgnoreCase)).ToList();
            found_users.Sort();
            return found_users;
        }

        public List<Jot> GetAllJots()
        {
            // SQL: select * from Jots;
            var query = from jot in _context.Jots select jot;
            List<Jot> found_jots = query.ToList();
            found_jots.Sort();
            return found_jots;
        }

        public bool CreateJot(JitterUser jitter_user1, string content)
        {
            Jot a_jot = new Jot { Content = content, Date = DateTime.Now, Author = jitter_user1 };
            bool is_added = true;
            try
            {
                Jot added_jot = _context.Jots.Add(a_jot);
                _context.SaveChanges();
                // Why is this null? Are the Docs inaccurate?
                /*
                if (added_jot == null)
                {
                    is_added = false;
                }*/
            } catch (Exception)
            {
                is_added = false;
            }
            return is_added;
        }
    }
}