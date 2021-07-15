using Nancy;
using Nancy.Hosting.Self;
using System;
using StoreStockWeb.Services.BusinessLogic;
using Nancy.Bootstrapper;
using Nancy.ViewEngines;
using Nancy.TinyIoc;
using System.Reflection;
using Nancy.Conventions;
using Nancy.Responses;
using System.IO;
using Nancy.Session;
/*
using Nancy.ViewEngines;
using TinyIoC;
*/
namespace StoreStockWeb.Services {
	public class CustomBootstrapper : DefaultNancyBootstrapper
	{
    protected override void ConfigureApplicationContainer(TinyIoCContainer container) {
      base.ConfigureApplicationContainer(container);
      ResourceViewLocationProvider.RootNamespaces.Add(
				Assembly.GetAssembly(typeof(MainModule)), "StoreStockWeb.Services.Views");
    }


    protected override void ApplicationStartup(TinyIoCContainer container, 
			IPipelines pipelines) {
      base.ApplicationStartup(container, pipelines);
      CookieBasedSessions.Enable(pipelines);
    }

    protected override void ConfigureConventions(NancyConventions conventions)
		{
			base.ConfigureConventions(conventions);
			conventions.StaticContentsConventions.Add(
				AddStaticResourcePath("/content", Assembly.GetAssembly(
					typeof(MainModule)), "StoreStockWeb.Services.Views.content"));
		}

		protected override NancyInternalConfiguration InternalConfiguration
		{
			get
			{
				return NancyInternalConfiguration.WithOverrides(OnConfigurationBuilder);
			}
		}

		void OnConfigurationBuilder(NancyInternalConfiguration x)
		{
			x.ViewLocationProvider = typeof(ResourceViewLocationProvider);
		}

		public static Func<NancyContext, string, Response> AddStaticResourcePath(string requestedPath, Assembly assembly, string namespacePrefix)
		{
			return (context, s) =>
			       	{
			       		var path = context.Request.Path;
						if (!path.StartsWith(requestedPath))
						{
							return null;
						}

						string resourcePath;
						string name;

						var adjustedPath = path.Substring(requestedPath.Length + 1);
						if (adjustedPath.IndexOf('/') >= 0)
						{
							name = Path.GetFileName(adjustedPath);
							resourcePath = namespacePrefix + "." + adjustedPath.Substring(0, adjustedPath.Length - name.Length - 1).Replace('/', '.');
						}
						else
						{
							name = adjustedPath;
							resourcePath = namespacePrefix;
						}
						return new EmbeddedFileResponse(assembly, resourcePath, name);
			       	};
		}
	}
}

/*using Nancy;
using Nancy.Hosting.Self;
using System;
using StoreStockWebServices.BusinessLogic;
using Nancy.Bootstrapper;
using Nancy.ViewEngines;
using Nancy.TinyIoc;
using System.Reflection;

namespace StoreStockWebServicess {
  public class CustomBootstrapper : DefaultNancyBootstrapper {
    protected override NancyInternalConfiguration InternalConfiguration {
      get {
        return NancyInternalConfiguration.WithOverrides(OnConfigurationBuilder);
      }
    }
    void OnConfigurationBuilder(NancyInternalConfiguration x) {
      x.ViewLocationProvider = typeof(ResourceViewLocationProvider);
    }
    protected override void ConfigureApplicationContainer(TinyIoCContainer container) {
      base.ConfigureApplicationContainer(container);
      ResourceViewLocationProvider.RootNamespaces.Add(
        Assembly.GetAssembly(typeof(MainModule)), "StoreStockWebServices.Views");
    }
  }
}
*/