using Autofac;
using StackExchange.Profiling;

namespace Annew.AutofacProfiling {
    public static class LifetimeScopeExtensions {
        
        /// <summary>
        /// Enables profiling on this container and any lifetime scopes created from
        /// this container.
        /// </summary>
        /// <param name="container">The container to be profiled.</param>
        public static void EnableProfiling(this ILifetimeScope container) {
            BindToResolveOperations(container);
            BindToLifetimeScopeCreation(container);
        }

        /// <summary>
        /// Ensures that whenever a descendent lifetime scope is created, it too 
        /// will be profiled.
        /// </summary>
        /// <param name="scope"></param>
        private static void BindToLifetimeScopeCreation(ILifetimeScope scope) {
            scope.ChildLifetimeScopeBeginning += (s, a) => {
                BindToLifetimeScopeCreation(a.LifetimeScope);
                BindToResolveOperations(a.LifetimeScope);
            };
        }

        /// <summary>
        /// Enables profiling on the lifetime scope.
        /// </summary>
        /// <param name="scope">The lifetime scope to be profiled.</param>
        private static void BindToResolveOperations(ILifetimeScope scope) {
            scope.ResolveOperationBeginning += (s1, a1) => {
                a1.ResolveOperation.InstanceLookupBeginning += (s2, a2) => {
                    // Only profile lookups when the registration is owned by this lifetime scope. If the registration
                    // is owned by a parent scope, we'll catch it there.
                    if (a2.InstanceLookup.ComponentRegistration.Ownership == Autofac.Core.InstanceOwnership.OwnedByLifetimeScope) {
                        var step = MiniProfiler.Current.Step("Resolving: " + a2.InstanceLookup.ComponentRegistration.Target.Activator.LimitType.Name);
                        a2.InstanceLookup.InstanceLookupEnding += (s3, a3) => {
                            if (step != null)
                                step.Dispose();
                        };
                    };
                };
            };
        }
    }
}
