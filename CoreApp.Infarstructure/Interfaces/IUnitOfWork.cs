using System;

namespace CoreApp.Infrastructure.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        //Call Save Change from DbContext
        void Commit();
    }
}
