using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Domain.Entities
{
    public class Category : Base
    {
        public string Name { get; set; }
        public bool isDeleted { get; set; }
    }
}
