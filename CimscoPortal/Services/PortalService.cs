using CimscoPortal.Data;
using CimscoPortal.Infrastructure;
using CimscoPortal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;

namespace CimscoPortal.Services
{
    class PortalService : IPortalService 
    {
        ICimscoPortalEntities _repository;

        public PortalService(ICimscoPortalEntities repository)
        {
            this._repository = repository;
        }

        public DbSet<PortalMessage> PortalMessages
        {
            get { return _repository.PortalMessages; }
        }

        public List<AlertViewModel> GetAlertsFor(int category)
        {
            return _repository.PortalMessages.Where(i => i.MessageCategoryId == category)
                                            .Project().To<AlertViewModel>()
                                            .ToList();
        }

        public List<AlertViewModel> GetNavbarDataFor(int customerId, string elementType)
        {
            return _repository.PortalMessages.Where(i => i.MessageType.TypeElement == elementType && i.CustomerId == customerId)
                                            .Project().To<AlertViewModel>()
                                            .ToList();
        }

    }
}
