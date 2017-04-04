using CimscoPortal.Models;
using System.Collections.Generic;
using System;
using System.Text;

namespace CimscoPortal.Services
{
    internal class ConsumptionChartDatapoints
    {
        private double min;
        private double avg;
        private double max;
        private double metric;

        private double start;
        private double end;

        private string datatype;
        private string decimalFormat;

        private List<CPart> dataRows;

        private string minSiteName;
        private string maxSiteName;

        private struct options
        {
            const double endMargin = 0.1; // % margin to use at each end of bar
            public const double startMgn = 1.0 - endMargin;
            public const double endMgn = 1.0 + endMargin;
            public const double markerWidth = 0.06;
        }

        private struct formatting
        {
            public const string newLine = "<br/>";

        }

            public ConsumptionChartDatapoints()
        {

        }

        public void SetMinAvgMax(double min, double avg, double max)
        {
            this.min = min;
            this.avg = avg;
            this.max = max;

            this.start = GetDatum(min, options.startMgn);
            this.end = GetDatum(max, options.endMgn);
        }

        public void SetCurrentMetric(double val)
        {
            this.metric = val;
        }

        public void SetMetricValue(double val)
        {
            this.metric = val;
        }

        public void SetDatatype(string datatype)
        {
            this.datatype = datatype;
        }

        public void SetDecimalFormat(string format)
        {
            this.decimalFormat = format;
        }

        public List<CPart> GetRowData()
        {
            this.dataRows = new List<CPart>();

            this.dataRows.Add(new CPart { v = datatype });                                      // Title

            this.dataRows.Add(new CPart { v = (start + GetPaddingValue(start, min)).ToString() });        // pad (start --> min marker)

            this.dataRows.Add(new CPart { v = options.markerWidth.ToString() });                // Min marker
            this.dataRows.Add(new CPart { f = ReturnHtmlFormattedTooltip_MinAvgMax("Minimum", datatype, min, decimalFormat, minSiteName) });        // Tooltip
            this.dataRows.Add(new CPart() { });                                                         // Formatting

            this.dataRows.Add(new CPart { v = GetPaddingValue(min, avg).ToString() });        //  pad (start --> avg marker)

            this.dataRows.Add(new CPart { v = options.markerWidth.ToString() });                // Avg marker
            this.dataRows.Add(new CPart { f = ReturnHtmlFormattedTooltip_MinAvgMax("Average", datatype, avg, decimalFormat, "") });        // Tooltip
            this.dataRows.Add(new CPart() { });                                                         // Formatting

            this.dataRows.Add(new CPart { v = GetPaddingValue(avg, max).ToString() });        // pad (avg --> max marker)

            this.dataRows.Add(new CPart { v = options.markerWidth.ToString() });                // max marker
            this.dataRows.Add(new CPart { f = ReturnHtmlFormattedTooltip_MinAvgMax("Maximum", datatype, max, decimalFormat, maxSiteName) });        // Tooltip
            this.dataRows.Add(new CPart() { });                                                         // Formatting

            this.dataRows.Add(new CPart { v = GetPaddingValue(max, end).ToString() });        // pad (max --> end)


            return dataRows;
        }

        public List<double> GetBarMinMaxValues()
        {
            return new List<double>() { this.start, this.end };
        }

        private double GetDatum(double val, double margin)
        {
            return System.Math.Round(val * margin, 0, System.MidpointRounding.AwayFromZero );
        }

        private double GetPaddingValue(double startVal, double endVal)
        {
            double _padValue = options.markerWidth;
            if (startVal == this.start | endVal == this.end)
                _padValue = _padValue / 2;

            return endVal - startVal - _padValue;
        }

        //private double GetChartMinimum()
        //{
        //    return System.Math.Round(min * (0.9), 0, System.MidpointRounding.AwayFromZero );
        //}

        //private double GetChartMaximum()
        //{
        //    return System.Math.Round(max * (1.1), 0, System.MidpointRounding.AwayFromZero);
        //}

        private static string ReturnHtmlFormattedTooltip_MinAvgMax(string metric, string units, double value, string format, string annotaion)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format("{0} consumption", metric));
            sb.Append(formatting.newLine);
            if (annotaion.Length > 0)
            {
                sb.Append(string.Format("({0})", annotaion));
                sb.Append(formatting.newLine);
            }

            sb.Append(string.Format("<b>{0}</b> {1}", value.ToString(format), units));

            sb.Append(formatting.newLine);

            return TooltipWrapper(sb.ToString());
        }

        private static string TooltipWrapper(string result)
        {
            string _boxWidth = "180";
            return string.Format("<div style='width:{0}px; margin:10px;'><center>{1}</center></div>", _boxWidth, result);
        }

        internal void SetMinMaxAnnotation(string minSiteName, string maxSiteName)
        {
            this.minSiteName = minSiteName;
            this.maxSiteName = maxSiteName;
        }
    }
}