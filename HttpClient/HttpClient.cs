// Decompiled with JetBrains decompiler
// Type: AstralToolbox.HttpClient.HttpClient
// Assembly: 1Ctoolbox, Version=2.4.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2A5A1506-7F8E-48A5-97C3-E797DB6697E3
// Assembly location: C:\Program Files (x86)\1Сtoolbox\1Ctoolbox.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace AstralToolbox.HttpClient
{
  public static class HttpClient
  {
    private const int CHUNK_SIZE = 102400;

    public static string DownloadFile(DownloadFileData downloadFileData)
    {
      if (downloadFileData == null)
        throw new ArgumentNullException(nameof (downloadFileData));
      if (string.IsNullOrEmpty(downloadFileData.Url))
        throw new ArgumentException("Не задан url.");
      if (string.IsNullOrEmpty(downloadFileData.FileName))
        throw new ArgumentException("Не задано имя файла.");
      string path;
      try
      {
        using (HttpWebResponse response = (HttpWebResponse) WebRequest.Create(downloadFileData.Url).GetResponse())
        {
          Stream responseStream = response.GetResponseStream();
          if (responseStream == null)
            throw new Exception("Сервер не отвечает.");
          using (BinaryReader binaryReader = new BinaryReader(responseStream))
          {
            List<byte> byteList = new List<byte>();
            byte[] numArray;
            do
            {
              numArray = binaryReader.ReadBytes(102400);
              byteList.AddRange((IEnumerable<byte>) numArray);
            }
            while (numArray.Length != 0);
            binaryReader.Close();
            string str = Path.Combine(Environment.GetEnvironmentVariable("APPDATA"), "AstralCryptoServices\\PrimaryRegPacketStore");
            if (!Directory.Exists(str))
              Directory.CreateDirectory(str);
            path = Path.Combine(str, downloadFileData.FileName);
            System.IO.File.WriteAllBytes(path, byteList.ToArray());
          }
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Не удалось скачать файл.", ex);
      }
      return path;
    }
  }
}
