using Autofac;

namespace Annew.AutofacProfiling {
    public class ProfiledContainer : ProfiledLifetimeScope, IContainer {
        public ProfiledContainer(IContainer container)
            : base(container) {
        }
    }
}
