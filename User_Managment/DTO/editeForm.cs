using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User_Managment.DTO
{
    public class editeForm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public List<int> roles { get; set; }
    }
}
