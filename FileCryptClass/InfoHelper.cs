using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCryptClass
{
    internal class InfoHelper
    {
        internal byte[] InsertInfoIntoByte(string Info, byte[] Content)
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(Encoding.UTF8.GetBytes(Info));
            bytes.Add(byte.MaxValue);
            bytes.AddRange(Content);
            return bytes.ToArray();
        }

        internal KeyValuePair<string, byte[]> GetInfoAndContent(byte[] MergedData)
        {
            
            int splitIndex = Array.IndexOf(MergedData, byte.MaxValue);
            byte[] RawInfo = MergedData[0..splitIndex];
            string ExtractedFileName = Encoding.UTF8.GetString(RawInfo.ToArray());
            byte[] RawContent = MergedData[(splitIndex +1)..];

            return new KeyValuePair<string, byte[]>(ExtractedFileName, RawContent);
        }
    }
}
