using System;

namespace PervasiveDigital.Security.ManagedProviders
{
    public interface IHashAlgorithm
    {
        byte[] GetHash();
        void AddData(byte[] data, uint offset, uint len);
    }
}
