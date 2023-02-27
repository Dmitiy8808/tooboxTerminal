// Decompiled with JetBrains decompiler
// Type: AstralToolbox.CryptoAPI.JaCartaCryptoEngine
// Assembly: 1Ctoolbox, Version=2.4.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2A5A1506-7F8E-48A5-97C3-E797DB6697E3
// Assembly location: C:\Program Files (x86)\1Сtoolbox\1Ctoolbox.exe

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace AstralToolbox.CryptoAPI
{
  public static class JaCartaCryptoEngine
  {
    private static void JaCartaInit(string pin)
    {
      if (!JaCartaCryptoEngine.Init(pin))
        throw new Exception(JaCartaCryptoEngine.GetErrorDescription(JaCartaCryptoEngine.GetErrorCode()));
    }

    public static string CreateCertRequest(JaCartaCertRequestParam data)
    {
      if (data == null)
        throw new ArgumentNullException(nameof (data));
      JaCartaCryptoEngine.JaCartaInit(data.Pin);
      string str = Path.Combine(Path.GetTempPath(), string.Format("{0}.bin", (object) Guid.NewGuid()));
      if (!JaCartaCryptoEngine.CreateCertificateRequest(data.RequestData, str))
        throw new Exception(JaCartaCryptoEngine.GetErrorDescription(JaCartaCryptoEngine.GetErrorCode()));
      return Encoding.ASCII.GetString(File.ReadAllBytes(str));
    }

    public static void InstallCert(JaCartaInstalCertData data)
    {
      string str = data != null ? Path.Combine(Path.GetTempPath(), data.ContainerName + ".cer") : throw new ArgumentNullException(nameof (data));
      File.WriteAllText(str, data.CertData);
      JaCartaCryptoEngine.JaCartaInit(data.Pin);
      if (!JaCartaCryptoEngine.SetCertificateToContainer(str, data.ContainerName))
        throw new Exception(JaCartaCryptoEngine.GetErrorDescription(JaCartaCryptoEngine.GetErrorCode()));
    }

    [DllImport("jcrypt.dll", CharSet = CharSet.Unicode)]
    private static extern bool Init(string pin);

    [DllImport("jcrypt.dll", CharSet = CharSet.Unicode)]
    private static extern uint GetErrorCode();

    [DllImport("jcrypt.dll", CharSet = CharSet.Unicode)]
    private static extern string GetErrorDescription(uint error);

    [DllImport("jcrypt.dll", CharSet = CharSet.Unicode)]
    private static extern bool CreateCertificateRequest(
      JaCartaCertRequestData request,
      string fileName);

    [DllImport("jcrypt.dll", CharSet = CharSet.Unicode)]
    private static extern bool SetCertificateToContainer(string fileName, string containerName);
  }
}
