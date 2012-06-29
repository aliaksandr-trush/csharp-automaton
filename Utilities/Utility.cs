namespace RegOnline.RegressionTest.Utilities
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using ICSharpCode.SharpZipLib.Zip;
    using NUnit.Framework;

    public class Utility
    {
        public static void ThreadSleep(int seconds)
        {
            System.Threading.Thread.Sleep(seconds * 1000);
        }

        public static void ThreadSleep(double seconds)
        {
            System.Threading.Thread.Sleep(Convert.ToInt32(seconds * 1000));
        }

        public static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;

            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            if (lowerCase)
            {
                return builder.ToString().ToLower();
            }

            return builder.ToString();
        }

        public static void VerifySHA512Hash(byte[] bytesA, byte[] bytesB)
        {
            SHA512 shaM = new SHA512Managed();
            byte[] hashA = shaM.ComputeHash(bytesA);
            byte[] hashB = shaM.ComputeHash(bytesB);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder hashAStringBuilder = new StringBuilder();
            StringBuilder hashBStringBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < hashA.Length; i++)
            {
                hashAStringBuilder.Append(hashA[i].ToString("x2"));
            }

            string inputA = hashAStringBuilder.ToString();

            for (int i = 0; i < hashB.Length; i++)
            {
                hashBStringBuilder.Append(hashB[i].ToString("x2"));
            }

            string inputB = hashBStringBuilder.ToString();

            if (comparer.Compare(inputA, inputB) != 0)
            {
                Assert.Fail("Hash not match! Expected: '{0}', but was '{1}'", inputA, inputB);
            }
        }

        public static void VerifySHA512Hash(string fileNameA, string fileNameB)
        {
            FileStream fileStreamForImage_before = new FileStream(
                    fileNameA,
                    FileMode.Open,
                    FileAccess.Read);

            BinaryReader binaryReaderForImage_before = new BinaryReader(fileStreamForImage_before);
            int length = (int)fileStreamForImage_before.Length;
            byte[] inputA = binaryReaderForImage_before.ReadBytes(length);

            fileStreamForImage_before.Close();
            binaryReaderForImage_before.Close();

            FileStream fileStreamForImage_after = new FileStream(
                fileNameB,
                FileMode.Open,
                FileAccess.Read);

            BinaryReader binaryReaderForImage_after = new BinaryReader(fileStreamForImage_after);
            length = (int)fileStreamForImage_after.Length;
            byte[] inputB = binaryReaderForImage_after.ReadBytes(length);

            fileStreamForImage_after.Close();
            binaryReaderForImage_after.Close();

            VerifySHA512Hash(inputA, inputB);
        }

        public static string GetEncryptedCCNumber(string ccNumber)
        {
            return string.Format(new CreditCardNumberFormatter(), "{0}", ccNumber);
        }

        public static byte[] ZipDecode(byte[] aBuff)
        {
            MemoryStream zipStream = new MemoryStream(aBuff);
            MemoryStream outputStream = new MemoryStream();
            ZipInputStream s = new ZipInputStream(zipStream);

            ZipEntry theEntry;
            while ((theEntry = s.GetNextEntry()) != null)
            {
                int size = 2048;
                byte[] data = new byte[2048];
                while (true)
                {
                    size = s.Read(data, 0, data.Length);
                    if (size > 0)
                    {
                        outputStream.Write(data, 0, size);
                    }
                    else
                    {
                        break;
                    }
                }

                outputStream.Close();
            }
            s.Close();

            return outputStream.GetBuffer();
        }
    }
}
