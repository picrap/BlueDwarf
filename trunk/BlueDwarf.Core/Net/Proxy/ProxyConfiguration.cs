namespace BlueDwarf.Net.Proxy
{
    using System;
    using System.Runtime.InteropServices;
    using Annotations;

    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    internal class ProxyConfiguration : IDisposable, IProxyConfiguration
    {
        const int INTERNET_OPEN_TYPE_PRECONFIG = 0;	// use registry configuration
        const int INTERNET_OPEN_TYPE_DIRECT = 1;	// direct to net
        const int INTERNET_OPEN_TYPE_PROXY = 3;	// via named proxy
        const int INTERNET_OPEN_TYPE_PRECONFIG_WITH_NO_AUTOPROXY = 4; // prevent using java/script/INS

        public struct INTERNET_PROXY_INFO
        {
            public int dwAccessType;
            public IntPtr proxy;
            public IntPtr proxyBypass;
        };

        [DllImport("wininet", SetLastError = true)]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);

        [DllImport("wininet", SetLastError = true)]
        private static extern IntPtr InternetOpen(string lpszAgent, int dwAccessType, string lpszProxyName, string lpszProxyBypass, int dwFlags);

        [DllImport("wininet")]
        private static extern long InternetCloseHandle(IntPtr hInet);

        [DllImport("urlmon")]
        private static extern int UrlMkSetSessionOption(int dwOption, IntPtr pBuffer, int dwBufferLength, int dwReserved);

        private IntPtr _internet;
        private IntPtr Internet
        {
            get
            {
                if (_internet == IntPtr.Zero)
                    _internet = InternetOpen("BlueDwarf", INTERNET_OPEN_TYPE_PRECONFIG, null, null, 0);
                return _internet;
            }
        }

        public void Dispose()
        {
            if (_internet != IntPtr.Zero)
            {
                InternetCloseHandle(_internet);
                _internet = IntPtr.Zero;
            }
        }

        private static void SetProxy(string strProxy, IntPtr hInternet)
        {
            const int INTERNET_OPTION_PROXY = 38;
            const int INTERNET_OPEN_TYPE_PROXY = 3;

            INTERNET_PROXY_INFO ipi;

            // Filling in structure 
            ipi.dwAccessType = INTERNET_OPEN_TYPE_PROXY;
            ipi.proxy = Marshal.StringToHGlobalAnsi(strProxy);
            ipi.proxyBypass = Marshal.StringToHGlobalAnsi("local");

            // Allocating memory 
            var intptrStruct = Marshal.AllocCoTaskMem(Marshal.SizeOf(ipi));

            // Converting structure to IntPtr	
            Marshal.StructureToPtr(ipi, intptrStruct, true);

            bool iReturn = InternetSetOption(hInternet, INTERNET_OPTION_PROXY, intptrStruct, Marshal.SizeOf(ipi));
            //var v = UrlMkSetSessionOption(INTERNET_OPTION_PROXY, intptrStruct, Marshal.SizeOf(ipi), 0);
            var e = Marshal.GetLastWin32Error();
        }

        public void SetApplicationProxy(Uri proxy)
        {
            SetProxy(string.Format("{0}={1}:{2}", proxy.Scheme, proxy.Host, proxy.Port), IntPtr.Zero);
        }
    }
}
