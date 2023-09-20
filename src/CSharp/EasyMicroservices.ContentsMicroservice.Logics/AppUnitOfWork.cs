using EasyMicroservices.Cores.AspEntityFrameworkCoreApi;
using System;
using System.Threading.Tasks;

namespace EasyMicroservices.ContentsMicroservice
{
    public class AppUnitOfWork : UnitOfWork
    {
        public AppUnitOfWork(IServiceProvider service) : base(service)
        {
        }


        public override void Dispose()
        {
            base.Dispose();
        }


        public override ValueTask DisposeAsync()
        {
            return base.DisposeAsync();
        }
    }
}
