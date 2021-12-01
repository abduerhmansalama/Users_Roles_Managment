using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User_Managment.Models
{
    public class Role
    {
        public Role()
        {
            this.users = new HashSet<User>();
        }
        public  int Id { get; set; }
        public  string Name { get; set; }
        public ICollection<User> users { get; set; }


    }
}
