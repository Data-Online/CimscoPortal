using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc4;
using CimscoPortal.Infrastructure;
using CimscoPortal.Services;
using CimscoPortal.Data;
using CimscoPortal.Controllers;
using CimscoPortal.Data.Models;

namespace CimscoPortal
{
    public static class Bootstrapper
    {
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            return container;
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();    
            RegisterTypes(container);

            // Web.API DI 
            System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = new WebApiContrib.IoC.Unity.UnityResolver(container);

            return container;
        }

        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IPortalService, PortalService>();
            container.RegisterType<ICimscoPortalEntities, CimscoPortalEntities>();
            container.RegisterType<ICimscoPortalContext, CimscoPortalContext>(new PerRequestLifetimeManager());

            // I dont understand why this is required, but account controller fails with 
            // Microsoft.AspNet.Identity.IUserStore`1 [...], is an interface and cannot be constructed. Are you missing a type mapping?
            // Ref http://stackoverflow.com/questions/24740619/identity-provider-and-unity-dependency-injection
            container.RegisterType<AccountController>(new InjectionConstructor());
            container.RegisterType<ManageController>(new InjectionConstructor());
            container.RegisterType<UserAdminController>(new InjectionConstructor());
        }
    }
}