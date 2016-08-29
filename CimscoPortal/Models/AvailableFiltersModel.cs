using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CimscoPortal.Models
{
    public class AvailableFiltersModel
    {
        public List<FilterItem> Divisions { get; set; }
        public List<FilterItem> Categories { get; set; }
        public List<FilterItem> InvTypes { get; set; }
    }

    public class FilterItem
    {
        public int Id { get; set; }
        public string Label { get; set; }
    }
}
