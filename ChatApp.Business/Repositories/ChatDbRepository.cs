using ChatApp.Business.Interfaces;
using ChatApp.Db.AppDbContext;
using ChatApp.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace ChatApp.Business.Repositories
{
    public class ChatDbRepository<TModel, TViewModel> : IRepository<TModel, TViewModel>
    where TModel : ModelBase
    where TViewModel : ViewModelBase
    {
        private readonly ChatAppDbContext _dbContext;
        public IQueryable<TModel> Table { get { return _dbContext.Set<TModel>().AsNoTracking(); } }

        public ChatDbRepository(ChatAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public TViewModel Take(int id)
        {
            return this.TakeModel(id)?.AsViewModel<TViewModel>();
        }

        public TViewModel Take(Func<TModel, bool> predicate)
        {
            return Table.FirstOrDefault(predicate)?.AsViewModel<TViewModel>();
        }

        public void Save(TViewModel viewModel)
        {
            TModel model = viewModel.Id == 0 ? viewModel.AsModel<TModel>() : viewModel.AsModel(Table);

            if (viewModel.Id == 0)
            {
                _dbContext.Set<TModel>().Add(model);
            }
            else
            {
                _dbContext.Set<TModel>().Update(model);
            }

            _dbContext.SaveChanges();

            viewModel.Id = model.Id;
        }

        public void SaveWithTransaction(TViewModel viewModel)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                Save(viewModel);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Debug.WriteLine(ex);
            }
        }

        public void Delete(int id)
        {
            TModel model = this.TakeModel(id);
            if (model != null)
            {
                _dbContext.Set<TModel>().Remove(model);
                _dbContext.SaveChanges();
            }
        }

        public void Delete(int[] ids)
        {
            TModel[] models = Table.Where(q => ids.Contains(q.Id)).ToArray();
            foreach (var model in models)
            {
                _dbContext.Set<TModel>().Remove(model);
            }
            _dbContext.SaveChanges();
        }

        #region Private

        private TModel TakeModel(int id)
        {
            var param = Expression.Parameter(typeof(TModel));
            var condition = Expression.Lambda<Func<TModel, bool>>(Expression.Equal(Expression.Property(param, "Id"), Expression.Constant(id, typeof(int))), param).Compile();

            return Table.FirstOrDefault(condition);
        }

        #endregion
    }
}
