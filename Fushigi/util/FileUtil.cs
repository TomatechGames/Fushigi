﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fushigi.util
{
    public class FileUtil
    {
        public static byte[] DecompressFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new Exception("FileUtil::DecompressFile -- File not found.");
            }

            var compressedBytes = File.ReadAllBytes(filePath);
            byte[] decompressedData = DecompressData(compressedBytes);
            return decompressedData;
        }

        public static byte[] DecompressData(byte[] fileBytes)
        {
            byte[] decompressedData;

            if (!IsFileCompressed(fileBytes)) {
                throw new Exception("FileUtil::DecompressData -- File not ZSTD Compressed.");
            }
            using (var decompressor = new ZstdNet.Decompressor())
            {
                decompressedData = decompressor.Unwrap(fileBytes);
            }

            return decompressedData;
        }

        public static bool IsFileCompressed(byte[] fileBytes)
        {
            if (fileBytes[0] == 0x28 && fileBytes[1] == 0xb5) {
                return true;
            }
            else {
                return false;
            }
        }

        public static byte[] CompressData(byte[] fileBytes)
        {
            byte[] compressedData;

            using (var compressor = new ZstdNet.Compressor(new ZstdNet.CompressionOptions(19)))
            {
                compressedData = compressor.Wrap(fileBytes);
            }

            return compressedData;
        }

        public static bool TryGetFileInfo(string filename, out FileInfo fileInfo)
        {
            try
            {
                fileInfo = new FileInfo(filename);
                return true;
            }
            catch
            {
                fileInfo = null;
                return false;
            }
        }
    }
}
