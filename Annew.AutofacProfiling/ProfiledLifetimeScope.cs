using Autofac;
using Autofac.Core;
using Autofac.Core.Lifetime;
using Autofac.Core.Resolving;
using StackExchange.Profiling;
using System;

namespace Annew.AutofacProfiling {
    public class ProfiledLifetimeScope : ProfiledComponentContext, ILifetimeScope {
        private readonly ILifetimeScope _lifetimeScope;

        public ProfiledLifetimeScope(ILifetimeScope lifetimeScope)
            : base(lifetimeScope) {
            _lifetimeScope = lifetimeScope;

            _lifetimeScope.ResolveOperationBeginning += (s1, operationBeginningEventArgs) => {

                operationBeginningEventArgs.ResolveOperation.InstanceLookupBeginning += (sender, args) => {
                    var step = MiniProfiler.Current.Step("Resolving: " + args.InstanceLookup.ComponentRegistration.Target.Activator.LimitType.Name);
                    args.InstanceLookup.InstanceLookupEnding += (o, eventArgs) => {
                        if (step != null)
                            step.Dispose();
                    }
                };
            };
        }

        public void Dispose() {
            _lifetimeScope.Dispose();
        }

        public ILifetimeScope BeginLifetimeScope() {
            return new ProfiledLifetimeScope(_lifetimeScope.BeginLifetimeScope());
        }

        public ILifetimeScope BeginLifetimeScope(object tag) {
            return new ProfiledLifetimeScope(_lifetimeScope.BeginLifetimeScope(tag));
        }

        public ILifetimeScope BeginLifetimeScope(Action<ContainerBuilder> configurationAction) {
            return new ProfiledLifetimeScope(_lifetimeScope.BeginLifetimeScope(configurationAction));
        }

        public ILifetimeScope BeginLifetimeScope(object tag, Action<ContainerBuilder> configurationAction) {
            return new ProfiledLifetimeScope(_lifetimeScope.BeginLifetimeScope(tag, configurationAction));
        }

        public IDisposer Disposer {
            get { return _lifetimeScope.Disposer; }
        }

        public object Tag {
            get { return _lifetimeScope.Tag; }
        }
        public event EventHandler<LifetimeScopeBeginningEventArgs> ChildLifetimeScopeBeginning {
            add { _lifetimeScope.ChildLifetimeScopeBeginning += value; }
            remove { _lifetimeScope.ChildLifetimeScopeBeginning -= value; }
        }
        public event EventHandler<LifetimeScopeEndingEventArgs> CurrentScopeEnding {
            add { _lifetimeScope.CurrentScopeEnding += value; }
            remove { _lifetimeScope.CurrentScopeEnding -= value; }
        }
        public event EventHandler<ResolveOperationBeginningEventArgs> ResolveOperationBeginning {
            add { _lifetimeScope.ResolveOperationBeginning += value; }
            remove { _lifetimeScope.ResolveOperationBeginning -= value; }
        }
    }
}
