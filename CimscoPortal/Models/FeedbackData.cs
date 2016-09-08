using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Models
{
    public class FeedbackData
    {
        public feedback feedback { get; set; }
    }
    public class feedback
    {
        public string url { get; set; }
        public browser browser { get; set; }
        public string html { get; set; }
        public string timestamp { get; set; }
        public string img { get; set; }
        public string note { get; set; }
    }
    public class browser
    {
        public string appCodeName { get; set; }
        public string appName { get; set; }
        public string appVersion { get; set; }
        public string cookieEnabled { get; set; }
        public string onLine { get; set; }
        public string platform { get; set; }
        public string userAgent { get; set; }
        public IList<string> plugins { get; set; }
    }
}
