using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CimscoPortal.Infrastructure
{
    class StructureMapDependencyResolver : IDependencyResolver
    {
        Container container = new Container();

        public object GetService(Type serviceType)
        {
            if (serviceType == null)
            {
                return null;
            }
           
            return serviceType.IsAbstract || serviceType.IsInterface
                ? container.TryGetInstance(serviceType)
                : container.GetInstance(serviceType); 
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return container.GetAllInstances(serviceType).Cast<object>();
        }
    }
}
