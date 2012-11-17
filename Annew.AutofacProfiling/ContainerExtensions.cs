using Autofac;

namespace Annew.AutofacProfiling {
    public static class ContainerExtensions {
        public static IContainer CreateProfiledContainer(this IContainer container) {
            return new ProfiledContainer(container);
        }
    }
}
