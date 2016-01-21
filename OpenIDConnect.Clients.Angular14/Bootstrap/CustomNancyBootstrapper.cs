using Nancy;
using Nancy.Conventions;
using Nancy.TinyIoc;

namespace OpenIDConnect.Clients.Angular14.Bootstrap
{
    public class CustomNancyBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            this.Conventions.ViewLocationConventions.Add((viewName, model, context) =>
            {
                return string.Concat("content/dist/", viewName);
            });
        }

        protected override void ConfigureConventions(NancyConventions conventions)
        {
            base.ConfigureConventions(conventions);

            conventions.StaticContentsConventions.AddDirectory("", "content/dist");
        }
    }    
}