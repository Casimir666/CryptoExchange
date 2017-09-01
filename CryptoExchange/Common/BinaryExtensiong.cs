using System;
using System.IO;

namespace CryptoExchange.Common
{
    static class BinaryExtensiong
    {
        public static void Write(this BinaryWriter writer, Guid guid)
        {
            writer.Write(guid.ToByteArray());
        }

        public static void Write(this BinaryWriter writer, Guid? guid)
        {
            if (guid.HasValue)
                writer.Write(guid.Value.ToByteArray());
            else
                writer.Write(Guid.Empty);
        }

        public static Guid ReadGuid(this BinaryReader reader)
        {
            return new Guid(reader.ReadBytes(16));
        }

        public static Guid? ReadNullableGuid(this BinaryReader reader)
        {
            var result = new Guid(reader.ReadBytes(16));
            return result != Guid.Empty ? (Guid?)result : null;
        }
    }
}
