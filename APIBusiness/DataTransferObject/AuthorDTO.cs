﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace APIBusiness.DataTransferObject
{
    public class AuthorDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CoverPhoto { get; set; }
        public string Description { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? LastUpdateTime { get; set; }
    }
}