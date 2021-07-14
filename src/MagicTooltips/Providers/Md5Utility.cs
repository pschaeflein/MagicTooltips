using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MagicTooltips.Providers
{
  static class Md5Utility
  {
    internal static string CalculateMd5(string filePath)
    {
      if (File.Exists(filePath))
      {

      using (var md5 = MD5.Create())
      {
        using (var stream = File.OpenRead(filePath))
        {
          var hash = md5.ComputeHash(stream);
          return BitConverter.ToString(hash);
        }
      }
      }
      else
      {
        return null;
      }
    }

  }
}
