﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskalu
{
    public class ListViewFile
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Memo { get; set; }
        public string Priority { get; set; }
        public string CreateDate { get; set; }
        public string DueDate { get; set; }
        public string Status { get; set; }
        public string WorkHolder { get; set; }
    }
}
