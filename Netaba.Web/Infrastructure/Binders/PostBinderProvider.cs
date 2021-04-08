using Netaba.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;

namespace Netaba.Web.Infrastructure.Binders
{
    public class PostBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(Post))
            {
                return new BinderTypeModelBinder(typeof(PostBinder));
            }

            return null;
        }
    }
}
