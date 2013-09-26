using Ninject;
using Ninject.Modules;
using Site.Domain.Abstract;
using Site.Domain.Concrete;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace CustomMembershipExample.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel kernel = new StandardKernel(new SiteServices());

        protected override IController GetControllerInstance(RequestContext context, Type controllerType)
        {
            if (controllerType == null)
                return null;
            return (IController)kernel.Get(controllerType);
        }

        private class SiteServices : NinjectModule
        {
            public override void Load()
            {
                Bind<IAccountRepository>()
                    .To<SqlAccountRepository>()
                    .WithConstructorArgument("connectionString",
                    ConfigurationManager.ConnectionStrings["SiteConnection"].ConnectionString
                );
            }
        }

        public void InjectMembership(MembershipProvider provider)
        {
            kernel.Inject(provider);
        }

        public void InjectRoleProvider(RoleProvider provider)
        {
            kernel.Inject(provider);
        }
    }
}