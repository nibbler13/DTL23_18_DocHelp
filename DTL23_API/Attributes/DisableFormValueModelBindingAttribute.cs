using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DTL23_API.Attributes {
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	internal class DisableFormValueModelBindingAttribute : Attribute, IResourceFilter {
		public void OnResourceExecuting(ResourceExecutingContext context) {
			var factories = context.ValueProviderFactories;
			factories.RemoveType<FormValueProviderFactory>();
			factories.RemoveType<FormFileValueProviderFactory>();
			factories.RemoveType<JQueryFormValueProviderFactory>();
		}

		public void OnResourceExecuted(ResourceExecutedContext context) {
			
		}
	}
}
