﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Models
{
    public class DonutChartViewModel
    {
        public List<DonutChartData> DonutChartData { get; set; }
        public List<SummaryData> SummaryData { get; set; }
        public HeaderData HeaderData { get; set; }
    }

    public class DonutChartData
    {
        public string Label { get; set; }
        public decimal Value { get; set; }
    }

    public class SummaryData
    {
        public string Title { get; set; }
        public string Detail { get; set; }
    }

    public class HeaderData
    {
        public string Header { get; set; }
        public string DataFor {  get; set; }
    }
}
