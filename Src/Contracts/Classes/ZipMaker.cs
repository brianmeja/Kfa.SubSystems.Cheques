using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Kfa.SubSystems.Cheques.Contracts.Classes
{
    public static class ZipMaker
    {
        public static void Create(string zipName, Dictionary<string, string> files, string password = "Kfa654321")
        {
            // Perform some simple parameter checking.  More could be done
            // like checking the target file name is ok, disk space, and lots
            // of other things, but for a demo this covers some obvious traps.

            try
            {
                // 'using' statements guarantee the stream is closed properly which is a big source
                // of problems otherwise.  Its exception safe as well which is great.
                using var s = new ZipOutputStream(File.Create(zipName));
                s.SetLevel(9); // 0 - store only to 9 - means best compression

                byte[] buffer = new byte[4096];

                foreach (var file in files)
                {
                    s.Password = password;
                    s.UseZip64 = UseZip64.On;
                    // Using GetFileName makes the result compatible with XP
                    // as the resulting path is not absolute.
                    var entry = new ZipEntry(file.Value)
                    {
                        // Setup the entry data as required.

                        // Crc and size are handled by the library for seakable streams
                        // so no need to do them here.

                        // Could also use the last write time or similar for the file.
                        DateTime = DateTime.Now
                    };
                    s.PutNextEntry(entry);

                    using var fs = File.OpenRead(file.Key);
                    // Using a fixed size buffer here makes no noticeable difference for output
                    // but keeps a lid on memory usage.
                    int sourceBytes;
                    do
                    {
                        sourceBytes = fs.Read(buffer, 0, buffer.Length);
                        s.Write(buffer, 0, sourceBytes);
                    } while (sourceBytes > 0);
                }

                // Finish/Close arent needed strictly as the using statement does this automatically

                // Finish is important to ensure trailing information for a Zip file is appended.  Without this
                // the created file would be invalid.
                s.Finish();

                // Close is important to wrap things up and unlock the file.
                s.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception during processing {0}", ex);

                // No need to rethrow the exception as for our purposes its handled.
            }
        }

        public static void Create(string zipName, IEnumerable<string> filenames, string password = "Kfa654321")
        {
            // Perform some simple parameter checking.  More could be done
            // like checking the target file name is ok, disk space, and lots
            // of other things, but for a demo this covers some obvious traps.

            try
            {
                // 'using' statements guarantee the stream is closed properly which is a big source
                // of problems otherwise.  Its exception safe as well which is great.
                using var s = new ZipOutputStream(File.Create(zipName));
                s.SetLevel(9); // 0 - store only to 9 - means best compression
                s.Password = password;
                s.UseZip64 = UseZip64.On;

                byte[] buffer = new byte[4096];

                foreach (string file in filenames)
                {
                    // Using GetFileName makes the result compatible with XP
                    // as the resulting path is not absolute.
                    var entry = new ZipEntry(Path.GetFileName(file))
                    {
                        // Setup the entry data as required.

                        // Crc and size are handled by the library for seakable streams
                        // so no need to do them here.

                        // Could also use the last write time or similar for the file.
                        DateTime = DateTime.Now
                    };
                    s.PutNextEntry(entry);
                    using var fs = File.OpenRead(file);
                    // Using a fixed size buffer here makes no noticeable difference for output
                    // but keeps a lid on memory usage.
                    int sourceBytes;
                    do
                    {
                        sourceBytes = fs.Read(buffer, 0, buffer.Length);
                        s.Write(buffer, 0, sourceBytes);
                    } while (sourceBytes > 0);
                }

                // Finish/Close arent needed strictly as the using statement does this automatically

                // Finish is important to ensure trailing information for a Zip file is appended.  Without this
                // the created file would be invalid.
                s.Finish();

                // Close is important to wrap things up and unlock the file.
                s.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception during processing {0}", ex);

                // No need to rethrow the exception as for our purposes its handled.
            }
        }

        public static void Create(string zipName, string contentFolder, string password = "Kfa654321")
        {
            Dictionary<string, string> GetSubFiles(string parentFolder, string dir)
            {
                Dictionary<string, string> ans = new();
                var dirs = new DirectoryInfo(dir).GetDirectories("*", SearchOption.TopDirectoryOnly);
                if (dirs.Any())
                {
                    foreach (var folder in dirs)
                    {
                        var dirNew = folder.Name;
                        if (!string.IsNullOrWhiteSpace(parentFolder))
                            dirNew = Path.Combine(parentFolder, dirNew);
                        var files1 = GetSubFiles(dirNew, folder.FullName);
                        if (files1.Any())
                            files1.ToList().ForEach(cc => ans.Add(cc.Key, cc.Value));
                    }
                }
                var files = Directory.GetFiles(dir);
                if (files.Any())
                    files.ToList().ForEach(cc =>
                    {
                        if (string.IsNullOrWhiteSpace(parentFolder))
                            ans.Add(cc, new FileInfo(cc).Name);
                        else
                            ans.Add(cc, Path.Combine(parentFolder, new FileInfo(cc).Name));
                    });
                return ans;
            }
            Create(zipName, GetSubFiles(null, contentFolder));
        }

        /// <summary>
        /// Method that compress all the files inside a folder (non-recursive) into a zip file.
        /// </summary>
        /// <param name="DirectoryPath"></param>
        /// <param name="OutputFilePath"></param>
        /// <param name="CompressionLevel"></param>
        private static void compressDirectoryWithPassword(string DirectoryPath, string OutputFilePath, string Password = null, int CompressionLevel = 9)
        {
            try
            {
                // Depending on the directory this could be very large and would require more attention
                // in a commercial package.
                string[] filenames = Directory.GetFiles(DirectoryPath);

                // 'using' statements guarantee the stream is closed properly which is a big source
                // of problems otherwise.  Its exception safe as well which is great.
                using (ZipOutputStream OutputStream = new ZipOutputStream(File.Create(OutputFilePath)))
                {
                    // Define a password for the file (if providen)
                    // set its value to null or don't declare it to leave the file
                    // without password protection
                    OutputStream.Password = Password;

                    // Define the compression level
                    // 0 - store only to 9 - means best compression
                    OutputStream.SetLevel(CompressionLevel);

                    byte[] buffer = new byte[4096];

                    foreach (string file in filenames)
                    {
                        // Using GetFileName makes the result compatible with XP
                        // as the resulting path is not absolute.
                        ZipEntry entry = new ZipEntry(Path.GetFileName(file));

                        // Setup the entry data as required.

                        // Crc and size are handled by the library for seakable streams
                        // so no need to do them here.

                        // Could also use the last write time or similar for the file.
                        entry.DateTime = DateTime.Now;
                        OutputStream.PutNextEntry(entry);

                        using (FileStream fs = File.OpenRead(file))
                        {
                            // Using a fixed size buffer here makes no noticeable difference for output
                            // but keeps a lid on memory usage.
                            int sourceBytes;

                            do
                            {
                                sourceBytes = fs.Read(buffer, 0, buffer.Length);
                                OutputStream.Write(buffer, 0, sourceBytes);
                            } while (sourceBytes > 0);
                        }
                    }

                    // Finish/Close arent needed strictly as the using statement does this automatically

                    // Finish is important to ensure trailing information for a Zip file is appended.  Without this
                    // the created file would be invalid.
                    OutputStream.Finish();

                    // Close is important to wrap things up and unlock the file.
                    OutputStream.Close();

                    Console.WriteLine("Files successfully compressed");
                }
            }
            catch (Exception ex)
            {
                // No need to rethrow the exception as for our purposes its handled.
                Console.WriteLine("Exception during processing {0}", ex);
            }
        }
    }
}