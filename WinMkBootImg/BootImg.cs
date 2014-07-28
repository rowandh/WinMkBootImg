using System.Text;

namespace WinMkBootImg
{
    public class BootImgHeader
    {
        // Header size in bytes
        public static readonly int BootImgSizeBytes = 1608;

        public static readonly byte[] BootMagic = Encoding.UTF8.GetBytes("ANDROID!");
        public static readonly int BootMagicSize = 8;
        public static readonly int BootNameSize = 16;
        public static readonly int BootArgsSize = 512;
        public static readonly int BootExtraArgsSize = 1024;

        // 8 bytes
        public char[] Magic;

        // 4 bytes
        public uint KernelSize;
        
        // 4 bytes
        public uint KernelAddr;

        // 4 bytes
        public uint RamdiskSize;

        // 4 bytes
        public uint RamdiskAddr;

        // 4 bytes
        public uint SecondSize;

        // 4 bytes
        public uint SecondAddr;

        // 4 bytes
        public uint TagsAddr;

        // 4 bytes
        public uint PageSize;

        // 4 bytes
        public uint DtSize;

        // 4 bytes
        public uint Unused;

        // 16 bytes
        public char[] Name;

        // 512 bytes
        public char[] CmdLine;

        // 8 bytes
        public char[] Id;

        // 1024 bytes
        public char[] ExtraCmdLine;
    }
}
