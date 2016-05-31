using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Data.Models
{
    public partial class Division
    {
        public Division()
        {

        }

        public int DivisionId { get; set; }
        public int GroupId { get; set; }
        public string DivisionName { get; set; }

    }
}
