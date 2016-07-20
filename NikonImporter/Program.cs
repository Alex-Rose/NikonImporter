using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NikonImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 3)
            {
                string input = args[0];
                string source = args[1];
                string dest = args[2];

                TransferPhotos(input, source, dest);
                return;
            }

            PrintHelp();
        }

        private static void TransferPhotos(string input, string source, string dest)
        {
            var errors = new List<string>();
            try
            {
                var lines = File.ReadAllLines(input);

                if (!Directory.Exists(source))
                {
                    throw new DirectoryNotFoundException("Source directory not found");
                }

                if (!Directory.Exists(dest))
                {
                    throw new DirectoryNotFoundException("Destination directory not found");
                }

                int i = 0;
                int size = lines.Length;
                foreach (var line in lines) 
                {
                    i++;
                    Console.Clear();
                    Console.WriteLine("Copying files...");
                    Console.ForegroundColor = ConsoleColor.Red;
                    foreach (var error in errors)
                    {
                        Console.WriteLine("Error : {0}", error);
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("{0} of {1}", i, size);

                    string fileName = line;
                    bool includeRaw = false;
                    if (line.EndsWith("*"))
                    {
                        fileName = line.TrimEnd('*');
                        includeRaw = true;
                    }

                    var nb = int.Parse(fileName);
                    try
                    {
                        CopyFile(nb, source, dest, "JPG");
                    }
                    catch (FileNotFoundException e)
                    {
                        errors.Add(e.Message);
                    }

                    if (includeRaw)
                    {
                        try
                        {
                            CopyFile(nb, source, dest, "NEF");
                        }
                        catch (FileNotFoundException e)
                        {
                            errors.Add(e.Message);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (errors.Count > 0)
                {
                    Console.Clear();
                    Console.WriteLine("Copying files...");
                    Console.WriteLine("... completed with error(s)");
                    Console.ForegroundColor = ConsoleColor.Red;
                    foreach (var error in errors)
                    {
                        Console.WriteLine("Error : {0}", error);
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("... completed!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        private static void CopyFile(int nb, string source, string dest, string ext)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("DSC_{0}.{1}", nb.ToString("D4"), ext);
            string fileName = builder.ToString();

            string sourcePath = Path.Combine(source, fileName);
            string destPath = Path.Combine(dest, fileName);

            if (File.Exists(sourcePath))
            {
                File.Copy(sourcePath, destPath);
            }
            else
            {
                throw new FileNotFoundException(sourcePath);
            }
        }

        private static void PrintHelp()
        {
            Console.WriteLine("USAGE:");
            Console.WriteLine("NikonImporter input_list.txt source_folder dest_folder");
        }
    }
}
