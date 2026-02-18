using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.WebRequestMethods;

namespace Favly.Domain.Entities
{
    public class EvaluationItem : Base
    {
        public string ExternalId { get; set; }

        public string Name {  get; set; }
        public string Description {  get; set; }
        
        public string ImageUrl {  get; set; }
        public int CategoryId {  get; set; }
        public string SecondaryInfo { get; set; } 
        
       public DateTime LastSync { get; set; } 

        public virtual Category Category { get; set; }
    }
}
