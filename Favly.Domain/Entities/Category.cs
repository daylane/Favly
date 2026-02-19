using Favly.Domain.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Domain.Entities
{
    public class Category : Entity
    {
        public string Name { get; set; }
        public bool isDeleted { get; set; }
    }
}
