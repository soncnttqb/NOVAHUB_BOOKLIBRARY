﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API
{
    public class CategorySearchResultDTO
    {
        public int Total { get; set; }
        public List<CategoryDTO> Results { get; set; }
    }
}