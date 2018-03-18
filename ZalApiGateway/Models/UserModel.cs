﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZalApiGateway.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string NickName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public int Id_Rank { get; set; }
        public int Id_Group { get; set; }
        //public int Body { get; set; }
        //public bool Pres_facebook { get; set; }
        //public bool Zaplatil_prispevek { get; set; }
    }
}
