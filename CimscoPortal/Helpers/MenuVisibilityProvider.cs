using MvcSiteMapProvider;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;

namespace CimscoPortal.Helpers
{
    class MenuVisibilityProvider : SiteMapNodeVisibilityProviderBase
    {
        public override bool IsVisible(ISiteMapNode node, IDictionary<string, object> sourceMetadata)
        {
            if (node == null)
            {
                return true;
            }
            string visibilityValue = "";
            // Is a visibility attribute specified?
            try { 
                    visibilityValue = node.Attributes["visibility"].ToString().Trim();
                }
            catch { return true;  }

            switch (visibilityValue)
            {
                case "AllUsers":
                    return true;
                case "AdminOnly":
                    return true;
                case "Hidden":
                    return false;
            }
            return true;
        }
    }
}
