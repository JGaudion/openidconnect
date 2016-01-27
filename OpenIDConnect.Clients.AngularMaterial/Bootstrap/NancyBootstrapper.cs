using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.Conventions;

namespace OpenIDConnect.Clients.AngularMaterial.Bootstrap
{
    public class NancyBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureConventions(NancyConventions conventions)
        {
            base.ConfigureConventions(conventions);
            
            conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("", "dist"));
            conventions.ViewLocationConventions.Add((viewName, model, context) => string.Concat("dist/", viewName));
        }
    }
}