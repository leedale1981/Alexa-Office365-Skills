[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(ATT.Alexa.Office365.Service.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(ATT.Alexa.Office365.Service.App_Start.NinjectWebCommon), "Stop")]

namespace ATT.Alexa.Office365.Service.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using DI;
    using System.Web.Mvc;
    using System.Web.Http;
    using Repositories;
    using Models;
    using Office365.Models;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);

                NinjectDependencyResolver ninjectResolver = new NinjectDependencyResolver(kernel);
                DependencyResolver.SetResolver(ninjectResolver);
                GlobalConfiguration.Configuration.DependencyResolver = ninjectResolver;

                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<Database<User>>().To<Database<User>>().InSingletonScope()
                .WithConstructorArgument("collectionNameKey", "alexa:UserCollectionName");

            kernel.Bind<Database<Company>>().To<Database<Company>>().InSingletonScope()
                .WithConstructorArgument("collectionNameKey", "alexa:CompanyCollectionName");

            kernel.Bind<ICreateRepositoryAsync<User>>().To<UserRepository>().InTransientScope();
            kernel.Bind<IReadRepositoryAsync<User>>().To<UserRepository>().InTransientScope();
            kernel.Bind<IReadRepositoryAsync<Event>>().To<CalendarRepository>().InTransientScope();
            kernel.Bind<ICreateRepositoryAsync<Company>>().To<CompanyRepository>().InTransientScope();
            kernel.Bind<IReadRepositoryAsync<Company>>().To<CompanyRepository>().InTransientScope();
        }        
    }
}
