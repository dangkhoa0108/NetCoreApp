using CoreApp.Data.IRepositories;

namespace CoreApp.Data.EF.Registration
{
    public interface IUnitOfWork
    {
        IProductCategoryRepository ProductCategoryRepository { get; }
        IProductRepository ProductRepository { get; }
        IFunctionRepository FunctionRepository { get; }
        ITagRepository TagRepository { get; }
        IProductTagRepository ProductTagRepository { get; }
        void Commit();
    }
}
