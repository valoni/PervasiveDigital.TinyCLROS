using System;

namespace PervasiveDigital.Net
{
    public interface INetworkAdapter
    {
        ISocket OpenSocket(string hostNameOrAddress, int portNumber, bool useTcp);
        object OperationLock { get; }
    }
}
