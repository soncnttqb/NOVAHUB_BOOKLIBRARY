﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class AuthorModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CoverPhoto { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<System.DateTime> LastUpdateTime { get; set; }
    }
}
