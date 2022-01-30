using System;
using System.Linq;

namespace ChatApp.Dto
{
    public class BaseDto
    {
        public int Id { get; set; }
    }

    public class ModelBase : BaseDto 
    {
        public TViewModel AsViewModel<TViewModel>() where TViewModel : ViewModelBase
        {
            TViewModel viewModel = Activator.CreateInstance<TViewModel>();

            var properties = this.GetType().GetProperties();

            foreach (var modelProp in properties)
            {
                var modelPropValue = modelProp.GetValue(this);

                var viewModelProp = typeof(TViewModel).GetProperty(modelProp.Name);
                if (viewModelProp != null)
                    viewModelProp.SetValue(viewModel, modelPropValue);
            }

            return viewModel;
        }
    }

    public class ViewModelBase : BaseDto 
    {
        public TModel AsModel<TModel>(IQueryable<TModel> table = null) where TModel : ModelBase
        {
            TModel model = null;

            if (this.Id == 0)
                model = Activator.CreateInstance<TModel>();
            else
                model = table.FirstOrDefault(q => q.Id == this.Id);

            var properties = this.GetType().GetProperties();

            foreach (var viewModelProp in properties)
            {
                var viewModelPropValue = viewModelProp.GetValue(this);

                var modelProp = typeof(TModel).GetProperty(viewModelProp.Name);

                if (modelProp != null)
                    modelProp.SetValue(model, viewModelPropValue);
            }

            return model;
        }
    }
}
