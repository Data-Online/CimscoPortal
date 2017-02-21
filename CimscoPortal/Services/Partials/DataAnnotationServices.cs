using CimscoPortal.Extensions;
using CimscoPortal.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CimscoPortal.Services
{
    partial class PortalService
    {
        //private static string _decimalFormat = "#,##0.00";
        //private static string _currencyFormat = "C2";

        #region Tooltips and Information Panel text
        private DatapointDetailView AnnotateDatapoint(MonthlySummaryModel summary, MonthlySummaryModel summary12, string graphName, IList<int> _allSitesInCurrentSelection, 
            IList<DatapointSiteDetails> _missingSites, DateTime date)
        {
            //            "Invoice Total excl GST"
            //"Total Kwh"
            //"Cost / Sqm"
            //                "Previous Year Total" "Previous Year Kwh" "Previous Year Cost / Sqm" "Previous Year Kwh / SqM" "Project Saving Estimate"
            DatapointDetailView _result = new DatapointDetailView();

            _result.Date = date;

            string _notes = "";
            string _pointDataSummary = "";

            switch (graphName)
            {
                case "Invoice Total excl GST":
                    string _delta = FormatDelta(summary12.InvoiceTotal, summary.InvoiceTotal, currencyFormat);
                    _pointDataSummary = String.Format("Total for {0} invoice(s) = {1}", (_allSitesInCurrentSelection.Count - _missingSites.Count).ToString(), summary.InvoiceTotal.ToString(currencyFormat));
                    _notes = String.Format("Total cost {0} compared to {1}", _delta, date.AddYears(-1).ToString(standardMonthYearFormat));
                    break;
                case "Previous Year Total":
                    _pointDataSummary = String.Format("Total for {0} invoices = {1}", (_allSitesInCurrentSelection.Count - _missingSites.Count).ToString(), summary12.InvoiceTotal.ToString(currencyFormat));
                    //_notes = String.Format("");
                    break;
                case "Project Saving Estimate":
                    decimal _costSavingEstimate = summary.InvoiceTotal ==
                    0 ? 0 : NumericExtensions.SafeDivision(summary12.EnergyTotal, (NumericExtensions.SafeDivision(summary.EnergyTotal, summary.InvoiceTotal)));
                    _notes = CreateSavingsEstimateNotes(summary, summary12, _missingSites, date, _costSavingEstimate);
                    _pointDataSummary = String.Format("Estimated efficiency saving = {0}",
                                    (_costSavingEstimate - summary.InvoiceTotal).ToString(currencyFormat)
                                    );
                    break;
                default:
                    //_delta = "NAN";
                    break;
            }
            _result.Status = _pointDataSummary;
            _result.Notes = _notes;
            _result.SitesMissingInvoices = _missingSites;
            //var _delta = FormatDelta(summary12.InvoiceTotal, summary.InvoiceTotal, _currencyFormat);
            return _result;
        }

        private static string CreateSavingsEstimateNotes(MonthlySummaryModel summary, MonthlySummaryModel summary12, IList<DatapointSiteDetails> _missingSites, DateTime date, decimal estimatedValue)
        {
            StringBuilder sb = new StringBuilder();

            decimal _percentage;
            string _lossGain;
            //GetGainLossValue(summary12.EnergyTotal, summary.EnergyTotal, out _percentage, out _lossGain);
            GetGainLossValue(estimatedValue, summary.InvoiceTotal, out _percentage, out _lossGain);
            string delta = FormatDelta(estimatedValue, summary.InvoiceTotal, currencyFormat);

            sb.Append(String.Format("If consumption remained at {0} levels (<b>{1}</b>), then the bill for {2} has an estimated value of <b>{3}</b>.",
                            date.AddYears(-1).ToString(standardMonthYearFormat),
                            summary12.EnergyTotal.ToString(decimalFormat) + " " + energySymbol,
                            date.ToString(standardMonthYearFormat),
                            estimatedValue.ToString(currencyFormat)
                            )
                );
            sb.Append(String.Format(" This represents a cost <b>{0}</b> of {3} ({1}) when compared with the current bill total of <b>{2}</b>.",
                            _lossGain, _percentage.ToString(percentFormat),
                            summary.InvoiceTotal.ToString(currencyFormat),
                            delta
                            )
                );
            if (_missingSites.Count > 0)
            {
                sb.Append(NewLine());
                sb.AppendLine("<b>NOTE:</b> There are missing invoices for this month. As a result this analyis will be inaccurate.");
            }
            return sb.ToString();
        }

        //private static string ReturnHtmlFormattedTooltip(DateTime date, string value, string count, string variation, string flag)
        //{
        //    StringBuilder sb = new StringBuilder();

        //   // sb.Append()
        //    string _result = string.Format("<div style='width:180px; margin:10px;'><b>{0} {1}</b>{3}<br/>{2}", date.ToString("MMMM"), date.ToString("yyyy"), value, count);

        //    if (variation.Length > 0)
        //    {
        //        _result = _result + string.Format("{0} <span>cf {1}</span>", variation, date.AddYears(-1).ToString("yyyy"));
        //    }
        //    _result = _result + string.Format("</div");
        //    if (!string.IsNullOrEmpty(flag))
        //    {
        //        _result = _result + string.Format("<br/><div><i class='red fa fa-circle'></i>Note: invoice(s) missing</div>");
        //    }
        //    return _result;
        //}

        private static string ReturnHtmlFormattedTooltip(DateTime date, decimal value, decimal priorValue, string count,  string flag, string format)
        {
            StringBuilder sb = new StringBuilder();
            string delta = FormatDelta(priorValue, value, format);

            sb.Append(TooltipStandardHeader(date, value, count, format));
            sb.Append(NewLine());
            if (delta.Length > 0)
            {
                sb.Append(string.Format("{0} cf {1}", delta, date.AddYears(-1).ToString("yyyy")));
            }

            if (!string.IsNullOrEmpty(flag))
            {
                sb.Append(NewLine());
                sb.Append(string.Format("<div><i class='red fa fa-circle'></i> Note: invoice(s) missing</div>"));
            }

            return TooltipWrapper(sb.ToString());
        }

        private static string ReturnHtmlFormattedTooltip_CostSavings(DateTime date, decimal estimatedValue, decimal currentValue, string count, string flag)
        {
            string _line;
            string _result = "";
            bool _minimal = true;
            StringBuilder sb = new StringBuilder();
            string delta = FormatDelta(estimatedValue, currentValue, currencyFormat);

            decimal _percentage;
            string _lossSavings;
            GetGainLossValue(estimatedValue, currentValue, out _percentage, out _lossSavings);
            sb.Append(TooltipStandardHeader(date, estimatedValue, count, currencyFormat));
            sb.Append(" (estimate)" + NewLine());
            //sb.Append(string.Format("{0} cf actual bill<br/>", delta));
            sb.Append(string.Format("{0} (actual)", currentValue.ToString(currencyFormat)));
            sb.Append(NewLine());
            if (_minimal)
            {
                //sb.Append(String.Format("EEM is a <b>{0}</b> of <b>{1}</b>", _lossSavings, _percentage.ToString(percentFormat)));
                sb.Append(string.Format("Estimated <b>{0}</b> of {1} (<b>{2}</b>)", _lossSavings, delta, _percentage.ToString(percentFormat)));
                //sb.Append(String.Format(" from the actual bill of {0}", currentValue.ToString(currencyFormat)));
                _result = sb.ToString();
            }
            else
            {
                _line = string.Format("If consumption remained at {0} levels", date.AddYears(-1).ToString("yyyy"));
                _line = _line + string.Format(", the bill for <b>{0} {1}</b> ({3}) has an estimated value of {2}", date.ToString("MMMM"), date.ToString("yyyy"),
                    estimatedValue.ToString(currencyFormat), currentValue.ToString(currencyFormat));
                _result = TooltipLine(_line, _result);
                _line = string.Format("A difference of {0}", delta);
                _result = TooltipLine(_line, _result);

            }
            if (!String.IsNullOrEmpty(flag))
            {
                _line = string.Format("<br/><div><i class='red fa fa-circle'></i>Note: invoice(s) missing</div>");
                _result = TooltipLine(_line, _result);
            }
            return TooltipWrapper(_result);
        }
        #endregion

        #region Formatting tools
        private static void GetGainLossValue(decimal firstValue, decimal secondValue, out decimal _percentage, out string _lossSavings)
        {
            _lossSavings = (firstValue < secondValue) ? "increase" : "saving";
            _percentage = Math.Abs(NumericExtensions.SafeDivision(secondValue, firstValue) - 1.0M);
        }

        private static string NewLine()
        {
            return "<br/>";
        }

        private static string[] SetPointColourBasedOnInvoiceCounts(int count, int count12, int countExpected)
        {
            // Both counts below expected - both axes
            // count below expected 
            // count12 below expected

            string[] _flag = new string[3];
            if (count < countExpected)
            {
                _flag[0] = "point { fill-color: red; } ";
            }

            if (count12 < countExpected)
            {
                _flag[1] = "point { fill-color: red; } ";
            }

            if (count12 != count)
            {
                _flag[2] = "point { fill-color: red; } ";
            }
            return _flag;
        }

        private static string FormatDelta(decimal value1, decimal value2, string format)
        {
            if (value1 == 0 | value2 == 0) { return ""; }
            //if (value2 == 0) { return ""; }
            decimal _result = value2 - value1;
            string _positiveNegative = "positive fa fa-long-arrow-up";
            if (_result < 0) { _result = _result * -1; _positiveNegative = "negative fa fa-long-arrow-down"; }

            return string.Format("<span class='{0} {1}'> {2}</span>", "gc-tooltip", _positiveNegative, _result.ToString(format));
        }



        private static string TooltipStandardHeader(DateTime date, decimal value, string count, string format)
        {
            return string.Format("<b>{0}</b> {1}<br/>{2}", date.ToString("MMMM yyyy"), count, value.ToString(format));
        }

        private static string TooltipWrapper(string result)
        {
            string _boxWidth = "180";
            return string.Format("<div style='width:{0}px; margin:10px;'>{1}</div>", _boxWidth, result);
        }

        private static string TooltipLine(string line, string result)
        {
            string _divider = "";
            if (result.Length > 0) { _divider = "<br/>"; }
            return string.Format("{0}{2}{1}", result, line, _divider);
        }

        // fa-arrows-h
        // fa-long-arrow-up
        private static string ReturnInvoiceCountText(int total)
        {
            string _pl = "";
            if (total > 1)
            {
                _pl = "s";
            }
            return " (" + total.ToString() + " invoice" + _pl + ")";
        }
        #endregion
    }
}