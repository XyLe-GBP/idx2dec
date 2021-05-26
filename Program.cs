using System;
using System.Diagnostics;
using System.IO;

namespace idx2dec
{
    class Program
    {
        static void Main(string[] args)
        {
            if (File.Exists(@"C:\\Perl64\\bin\\perl.exe"))
            {
                Console.WriteLine("ActivePerl found.\n");
            }
            else if (File.Exists(@"C:\\Strawberry\\perl\\bin\\perl.exe"))
            {
                Console.WriteLine("Strawberry Perl found.\n");
            }
            else
            {
                Console.WriteLine("An unexpected error has occurred.\nCannot find perl.\nActivePerl or Strawberry Perl must be installed to use this application.\n");
                return;
            }
            if (args.Length != 2)
            {
                Console.Clear();
                Console.WriteLine("idx2dec - IDOLM@STER Deary Stars: Archive decompress utility - CUI Edition\nApplication by: XyLeP.\nPerl script by: mirai-iro\n\nUsage: idx2dec <BIN-Path> <IDX-Path>\n");
                Console.WriteLine("<BIN-Path>: Specify the full path of the BIN archive file.\n<IDX-Path>: Specify the full path of the IDX file.\nNote: Both the BIN archive file and the IDX file must reside in the same directory.\n");
                return;
            }
            else
            {
                Console.Clear();
                if (!File.Exists(Path.GetFullPath(args[0])))
                {
                    Console.WriteLine("An unexpected error has occurred.\n");
                    Console.WriteLine(args[0] + " : No such file or directory.\n");
                    return;
                }
                if (!File.Exists(Path.GetFullPath(args[1])))
                {
                    Console.WriteLine("An unexpected error has occurred.\n");
                    Console.WriteLine(args[1] + " : No such file or directory.\n");
                    return;
                }
                if (Path.GetExtension(args[0]) != ".BIN")
                {
                    Console.WriteLine("An unexpected error has occurred.\nArgument 1 is not a BIN archive file.\n");
                    return;
                }
                if (Path.GetExtension(args[1]) != ".IDX")
                {
                    Console.WriteLine("An unexpected error has occurred.\nArgument 2 is not a IDX archive file.\n");
                    return;
                }
                if (Path.GetFileNameWithoutExtension(args[0]) != Path.GetFileNameWithoutExtension(args[1]))
                {
                    Console.WriteLine("An unexpected error has occurred.\nThe file names of argument 1 and argument 2 are different.\n");
                    return;
                }
                Console.WriteLine("Readed BIN: {0}\nReaded IDX: {1}\n", Path.GetFileName(args[0]), Path.GetFileName(args[1]));
                byte[] plxFile = Properties.Resources.main;
                File.WriteAllBytes(Directory.GetCurrentDirectory() + "\\main.plx", plxFile);
                if (Directory.Exists(Directory.GetCurrentDirectory() + "\\" + Path.GetFileNameWithoutExtension(args[0])))
                {
                    Delete(Directory.GetCurrentDirectory() + "\\" + Path.GetFileNameWithoutExtension(args[0]));
                }
                Console.WriteLine("Decrypting and decompressing...\n----------------------------------");

                ProcessStartInfo pInfo = new ProcessStartInfo();
                Process process;
                pInfo.FileName = "perl";
                pInfo.Arguments = @".\\main.plx " + Path.GetFileNameWithoutExtension(args[0]);
                pInfo.UseShellExecute = true;
                process = Process.Start(pInfo);
                process.WaitForExit();
                File.Delete(Directory.GetCurrentDirectory() + "\\main.plx");

                if (Directory.Exists(Directory.GetCurrentDirectory() + "\\" + Path.GetFileNameWithoutExtension(args[0])))
                {
                    foreach (string file in Directory.GetFiles(Directory.GetCurrentDirectory() + "\\" + Path.GetFileNameWithoutExtension(args[0]), "*.*"))
                    {
                        Console.WriteLine(file);
                    }
                    DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\" + Path.GetFileNameWithoutExtension(args[0]));

                    // フォルダサイズ取得
                    long size = GetFolderSize(di);

                    Console.WriteLine("\nSize: {0} bytes, OK.\n", size.ToString());
                    Console.WriteLine("Archive decompression complete.\n");
                    return;
                }
                else
                {
                    Console.WriteLine("An unexpected error has occurred.\n");
                    Console.WriteLine(Directory.GetCurrentDirectory() + "\\" + Path.GetFileNameWithoutExtension(args[0]) + " : No such directory.\n");
                    return;
                }
            }
        }
        static long GetFolderSize(DirectoryInfo di)
        {
            long size = 0;

            foreach (FileInfo f in di.GetFiles())
            {
                size += f.Length;
            }
            foreach (DirectoryInfo d in di.GetDirectories())
            {
                size += GetFolderSize(d);
            }
            return size;
        }

        public static void Delete(string targetDirectoryPath)
        {
            if (!Directory.Exists(targetDirectoryPath))
            {
                return;
            }

            string[] filePaths = Directory.GetFiles(targetDirectoryPath);
            foreach (string filePath in filePaths)
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
                File.Delete(filePath);
            }

            string[] directoryPaths = Directory.GetDirectories(targetDirectoryPath);
            foreach (string directoryPath in directoryPaths)
            {
                Delete(directoryPath);
            }

            Directory.Delete(targetDirectoryPath, false);
        }
    }
}


