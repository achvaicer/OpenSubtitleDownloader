using System;
using System.Text;
using System.IO;
   
namespace OpenSubtitleDownloader.Model
{
    public static class HashGenerator
    {
        public static string ComputeMovieHash(string filename)
        {
            byte[] result;
            using (Stream input = File.OpenRead(filename))
            {
                result = ComputeMovieHash(input);
            }
            return ToHexadecimal(result);
        }
 
        private static byte[] ComputeMovieHash(Stream input)
        {
            ulong lhash;
            long streamsize;
            streamsize = input.Length;
            lhash = (ulong)streamsize;
 
            long i = 0;
            byte[] buffer = new byte[sizeof(long)];
            input.Position = 0;
            while (i < 65536 / sizeof(long) && (input.Read(buffer, 0, sizeof(long)) > 0))
            {
                i++;
               unchecked { lhash += BitConverter.ToUInt64(buffer, 0); }
            }
 
            input.Position = Math.Max(0, streamsize - 65536);
            i = 0;
            while (i < 65536 / sizeof(long) && (input.Read(buffer, 0, sizeof(long)) > 0))
            {
                i++;
               unchecked { lhash += BitConverter.ToUInt64(buffer, 0); }
            }           
            byte[] result = BitConverter.GetBytes(lhash);
            Array.Reverse(result);
            return result;
        }
 
        private static string ToHexadecimal(byte[] bytes)
        {
            StringBuilder hexBuilder = new StringBuilder();
            for(int i = 0; i < bytes.Length; i++)
            {
                hexBuilder.Append(bytes[i].ToString("x2"));
            }
            return hexBuilder.ToString();
        }
 
        
    }
}