using System;
using System.Collections.Generic;

//Developer: SangonomiyaSakunovi

namespace SangoIOCPNet
{
    public static class IOCPTool
    {
        public static byte[] SplitLogicBytes(ref List<byte> bytesList)
        {
            byte[] buff = null;
            if (bytesList.Count > 4)
            {
                byte[] data = bytesList.ToArray();
                int length = BitConverter.ToInt32(data, 0);
                if (bytesList.Count >= length + 4)
                {
                    buff = new byte[length];
                    Buffer.BlockCopy(data, 4, buff, 0, length);
                    bytesList.RemoveRange(0, length + 4);
                }
            }
            return buff;
        }

        public static byte[] PackMessageLengthInfo(byte[] body)
        {
            int length = body.Length;
            byte[] package = new byte[length + 4];
            byte[] head = BitConverter.GetBytes(length);
            head.CopyTo(package, 0);
            body.CopyTo(package, 4);
            return package;
        }
    }
}
