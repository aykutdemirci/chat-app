using ChatApp.Dto;
using System;
using System.Linq;

namespace ChatApp.Business.Interfaces
{
    public interface IRepository<TModel, TViewModel>
        where TModel : ModelBase
        where TViewModel : ViewModelBase
    {
        IQueryable<TModel> Table { get; }

        TViewModel Take(int id);

        TViewModel Take(Func<TModel, bool> predicate);

        void Save(TViewModel viewModel);

        void SaveWithTransaction(TViewModel viewModel);

        void Delete(int id);

        void Delete(int[] ids);
    }
}
