using System.Web.Http;
using Business.Contacts;
using Business.Contacts.Repositories;
using Business.Data.Repositories;
using Business.Repositories;
using Business.Services.Contracts;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;


namespace Business.Services.Configurations
{
    public class SimpleInjectorInitializer
    {
        private static SimpleInjector.Container _container = new SimpleInjector.Container();

        internal static Container Container
        {
            get { return _container; }
        }

        public static SimpleInjector.Container Initialize()
        {
            _container = new SimpleInjector.Container();

            _container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();
            Register();
            return _container;
        }

        public static SimpleInjector.Container ApiInitialize()
        {
            _container = new SimpleInjector.Container();

            _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            _container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
           
            Register();
         
            return _container;
        }

        private static void Register()
        {
            // repositories
            _container.Register<IPersonsRepository, PersonsRepository>(Lifestyle.Scoped);
            _container.Register<ICountryRepository, CountryRepository>(Lifestyle.Scoped);
            _container.Register<IAuthTokenRepository, AuthTokenRepository>(Lifestyle.Scoped);

            //services
            _container.Register<IPersonsService, PersonsService>(Lifestyle.Scoped);
            _container.Register<IAuthTokenService, AuthTokenService>(Lifestyle.Scoped);

            _container.Verify();

        }
    }
}
