﻿using System;
using System.Collections.Generic;

#nullable disable

namespace ASM.Models
{
    public partial class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public string Img { get; set; }
        public bool Published { get; set; }
        public string Alias { get; set; }
        public DateTime? CreateData { get; set; }
        public string Author { get; set; }
        public int? AccountId { get; set; }
        public string ShortContents { get; set; }
        public string Tags { get; set; }
        public int? CateId { get; set; }
        public bool IsHot { get; set; }
        public bool IsNewFeed { get; set; }

        public virtual Account Account { get; set; }
        public virtual Category Cate { get; set; }
    }
}
