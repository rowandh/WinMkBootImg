using System;
using System.IO;
using System.Linq;

namespace WinMkBootImg
{
    public class Program
    {
        static void Main(string[] args)
        {
            var path = "boot.img";
            int pos;

            if (!AndroidMagicFound(path, out pos)) return;

            var header = ReadBinaryHeader(path);
            var bootBase = header.KernelAddr - 0x00008000;

            Console.WriteLine("BOARD_KERNEL_CMDLINE {0}", new string(header.CmdLine));
            Console.WriteLine("BOARD_KERNEL_BASE {0}", bootBase.ToString("X8"));
            Console.WriteLine("BOARD_NAME {0}", new string(header.Name));
            Console.WriteLine("BOARD_PAGE_SIZE {0}", header.PageSize.ToString("D"));
            Console.WriteLine("BOARD_KERNEL_SIZE {0}", header.KernelSize.ToString("X8"));
            Console.WriteLine("BOARD_KERNEL_OFFSET {0}", (header.KernelAddr - bootBase).ToString("X8"));
            Console.WriteLine("BOARD_RAMDISK_OFFSET {0}", (header.RamdiskAddr - bootBase).ToString("X8"));

            if (header.SecondSize != 0)
                Console.WriteLine("BOARD_SECOND_OFFSET {0}", (header.SecondAddr - bootBase).ToString("X8"));
        
            Console.WriteLine("BOARD_TAGS_OFFSET {0}", (header.TagsAddr - bootBase).ToString("X8"));

            if (header.DtSize != 0)
                Console.WriteLine("BOARD_DT_SIZE {0}", header.DtSize.ToString("D"));

            Console.ReadKey();
        }

        private static BootImgHeader ReadBinaryHeader(string path)
        {
            using (var reader = new BinaryReader(File.OpenRead(path)))
            {
                return new BootImgHeader
                {
                    Magic = reader.ReadChars(BootImgHeader.BootMagicSize),
                    KernelSize = reader.ReadUInt32(),
                    KernelAddr = reader.ReadUInt32(),
                    RamdiskSize = reader.ReadUInt32(),
                    RamdiskAddr = reader.ReadUInt32(),
                    SecondSize = reader.ReadUInt32(),
                    SecondAddr = reader.ReadUInt32(),
                    TagsAddr = reader.ReadUInt32(),
                    PageSize = reader.ReadUInt32(),
                    DtSize = reader.ReadUInt32(),
                    Unused = reader.ReadUInt32(),
                    Name = reader.ReadChars(BootImgHeader.BootNameSize),
                    CmdLine = reader.ReadChars(BootImgHeader.BootArgsSize),
                    Id = reader.ReadChars(8),
                    ExtraCmdLine = reader.ReadChars(BootImgHeader.BootExtraArgsSize)
                };
            }            
        }

        /// <summary>
        /// Checks the first 512 bytes for the Android magic sequence
        /// </summary>
        /// <param name="path"></param>
        /// <param name="foundPos">The byte where the magic sequence was found.</param>
        /// <returns></returns>
        private static bool AndroidMagicFound(string path, out int foundPos)
        {
            using (var fs = new FileStream(path, FileMode.Open))
            {
                //fs.Seek(0, SeekOrigin.Begin);
                for (var i = 0; i <= 512; i++)
                {
                    foundPos = i;

                    //fs.Seek(i, SeekOrigin.Begin);
                    var asdf = new Byte[BootImgHeader.BootMagicSize];

                    fs.Read(asdf, i, BootImgHeader.BootMagicSize);

                    if (asdf.SequenceEqual(BootImgHeader.BootMagic))
                    {
                        Console.WriteLine("Android boot magic found");
                        return true;
                    }
                }
                
                foundPos = 0;
                return false;
            }
        }
    }


}
