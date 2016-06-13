using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Models
{
    public class TextViewModel
    {
        public string Header { get; set; }
        
        public List<string> Text { get; set; }
    }
}
