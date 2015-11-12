using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using log4net;
using SimpleBundler.Compressor;

namespace SimpleBundler
{
    internal static class Utils
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static string GetSignature(string content)
        {
            using (var md5 = MD5.Create())
            {
                var hashedBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(content));
                return BitConverter.ToString(hashedBytes).Replace("-", String.Empty);
            }
        }

        public static string GetCompressedContents(IEnumerable<string> bundle, string basePath, ICompressor compressor)
        {
            var fileList = bundle.ToList();
            var tempLookup = new ConcurrentDictionary<int, string>();
            Parallel.For(0, fileList.Count(), index =>
            {
                var file = fileList[index];
                var filePathName = Path.Combine(basePath, file);
                var fileContents = File.ReadAllText(filePathName, Encoding.UTF8);
                try
                {
                    fileContents = compressor.Compress(fileContents);
                }
                catch (Exception)
                {
                    Log.Warn("Error during compression (using uncompressed file) - " + file);
                }

                tempLookup[index] = fileContents;
            });

            var contents = new StringBuilder();
            for (var index = 0; index < fileList.Count; index++)
            {
                contents.Append(tempLookup[index]);
            }

            return contents.ToString();
        }
    }
}
