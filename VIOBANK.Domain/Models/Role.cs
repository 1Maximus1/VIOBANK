﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIOBANK.Domain.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Permission> Permissions { get; set; } = [];
        public List<User> Users { get; set; } = [];
    }
}
