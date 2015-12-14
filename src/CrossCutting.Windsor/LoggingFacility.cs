using Castle.Core.Configuration;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;

namespace CrossCutting.Diagnostics
{
    public class LoggingFacility : IFacility
    {
        public void Init(IKernel kernel, IConfiguration facilityConfig)
        {
            kernel.Register(Component.For<LoggingInterceptor>());
        }

        public void Terminate()
        {
        }
    }
}