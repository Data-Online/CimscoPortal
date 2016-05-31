using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Data.Models
{
    public partial class IndustryClassification
    {
        public IndustryClassification()
        {

        }

        [System.ComponentModel.DataAnnotations.Key]
        public int IndustryId { get; set; }
        public string IndustryCode { get; set; }
        public string IndustryDescription { get; set; }
        public bool Enabled { get; set; }

    }
}
