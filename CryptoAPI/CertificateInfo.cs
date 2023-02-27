// Decompiled with JetBrains decompiler
// Type: AstralToolbox.CryptoAPI.CertificateInfo
// Assembly: 1Ctoolbox, Version=2.4.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2A5A1506-7F8E-48A5-97C3-E797DB6697E3
// Assembly location: C:\Program Files (x86)\1Сtoolbox\1Ctoolbox.exe

namespace AstralToolbox.CryptoAPI
{
  public class CertificateInfo
  {
    public string NotBefore { get; set; }

    public string NotAfter { get; set; }

    public string Thumbprint { get; set; }

    public string SubjectName { get; set; }

    public string Algorithm { get; set; }

    public string SubjectKeyId { get; set; }
  }
}
