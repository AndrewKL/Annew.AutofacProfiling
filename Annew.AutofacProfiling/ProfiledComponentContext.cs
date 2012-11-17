using Autofac;
using Autofac.Core;
using System.Collections.Generic;

namespace Annew.AutofacProfiling
{

    public class ProfiledComponentContext : IComponentContext {
        private readonly IComponentContext _componentContext;

        public ProfiledComponentContext(IComponentContext componentContext) {
            _componentContext = componentContext;
        }

        public object ResolveComponent(IComponentRegistration registration, IEnumerable<Parameter> parameters) {
            return _componentContext.ResolveComponent(registration, parameters);
        }

        public IComponentRegistry ComponentRegistry {
            get { return _componentContext.ComponentRegistry; }
        }
    }
}
