#region SignReferences
// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
#endregion

namespace BlueDwarf.ViewModel
{
    using System;
    using System.Linq;
    using Controls;
    using Net.Proxy.Client;
    using Utility;

    partial class HomeViewModel
    {

        /// <summary>
        /// Sets the status as pending (hides all statuses).
        /// </summary>
        private void SetStatusPending()
        {
            LocalProxyStatus = StatusCode.Pending;
            RemoteProxyStatus = RemoteProxy != null ? StatusCode.Pending : StatusCode.None;
            TestTargetStatus = StatusCode.Pending;
        }

        /// <summary>
        /// Sets the status given an established route.
        /// </summary>
        /// <param name="route">The route.</param>
        private void SetSuccessStatus(Route route)
        {
            if (route.Relays.Any(r => r == LocalProxy))
                LocalProxyStatus = StatusCode.OK;
            if (route.Relays.Any(r => r == RemoteProxy))
                RemoteProxyStatus = StatusCode.OK;
            if (TestTargetUri != null)
                TestTargetStatus = StatusCode.OK;
        }

        /// <summary>
        /// Describes a failure point
        /// </summary>
        private interface IFailurePoint
        {
            /// <summary>
            /// Determines whether the specified point has failed.
            /// </summary>
            /// <param name="proxyRouteException">The proxy route exception.</param>
            /// <returns></returns>
            bool HasFailed(ProxyRouteException proxyRouteException);
            /// <summary>
            /// Sets the status for the current point.
            /// </summary>
            /// <param name="code">The code.</param>
            void SetStatus(StatusCode code);
        }

        private class ProxyFailurePoint : IFailurePoint
        {
            private readonly Uri _proxy;
            private readonly PropertyAccessor<StatusCode> _statusProperty;

            public bool HasFailed(ProxyRouteException proxyRouteException)
            {
                return proxyRouteException.Proxy == _proxy;
            }

            public void SetStatus(StatusCode code)
            {
                if (_proxy == null)
                {
                    _statusProperty.Value = StatusCode.None;
                    return;
                }

                if (_statusProperty.Value == StatusCode.Pending)
                    _statusProperty.Value = code;
            }

            public ProxyFailurePoint(Uri proxy, PropertyAccessor<StatusCode> statusProperty)
            {
                _proxy = proxy;
                _statusProperty = statusProperty;
            }
        }

        private class TargetFailurePoint : IFailurePoint
        {
            private readonly Uri _target;
            private readonly PropertyAccessor<StatusCode> _statusProperty;

            public bool HasFailed(ProxyRouteException proxyRouteException)
            {
                if (_target == null)
                    return false;

                return proxyRouteException.TargetHost == _target.Host;
            }

            public void SetStatus(StatusCode code)
            {
                if (_statusProperty.Value == StatusCode.Pending)
                    _statusProperty.Value = code;
            }

            public TargetFailurePoint(Uri target, PropertyAccessor<StatusCode> statusProperty)
            {
                _target = target;
                _statusProperty = statusProperty;
            }
        }

        /// <summary>
        /// Sets the status, given a ProxyRouteException. If no ProxyRouteException, then we consider it's all OK
        /// (right, this is not very nice)
        /// </summary>
        /// <param name="proxyRouteException">The proxy route exception.</param>
        private void SetFailureStatus(ProxyRouteException proxyRouteException)
        {
            SetFailureStatusLines(proxyRouteException,
                new ProxyFailurePoint(LocalProxy, new PropertyAccessor<StatusCode>(() => LocalProxyStatus, v => LocalProxyStatus = v)),
                new ProxyFailurePoint(RemoteProxy, new PropertyAccessor<StatusCode>(() => RemoteProxyStatus, v => RemoteProxyStatus = v)),
                new TargetFailurePoint(TestTargetUri, new PropertyAccessor<StatusCode>(() => TestTargetStatus, v => TestTargetStatus = v)));
        }

        private static void SetFailureStatusLines(ProxyRouteException proxyRouteException, params IFailurePoint[] failurePoints)
        {
            var code = StatusCode.OK;
            foreach (var failurePoint in failurePoints)
            {
                if (proxyRouteException != null && failurePoint.HasFailed(proxyRouteException))
                {
                    failurePoint.SetStatus(StatusCode.Error);
                    code = StatusCode.None;
                }
                else
                    failurePoint.SetStatus(code);
            }
        }
    }
}
