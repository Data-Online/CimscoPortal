using CimscoPortal.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Models
{
    public class PortalDataFactory
    {
        //private IPortalService _portalService;

        //public PortalDataFactory(IPortalService portalService) 
        //{
        //    _portalService = portalService;
        //}

        public static IEnumerable<MessageViewModel> GetMessages(int customerId)
        {
            return new List<MessageViewModel>()
            {
                new MessageViewModel() { CategoryName="test" },
                new MessageViewModel() { CategoryName="test2"}
            };
        }

        //public IEnumerable<MessageViewModel> GetMessagesZ(int customerId)
        //{
        //    return _portalService.GetNavbarDataForZ(3, "Alerts");
        //}
    }
}
