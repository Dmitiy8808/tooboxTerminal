// Decompiled with JetBrains decompiler
// Type: AstralToolbox.CryptoAPI.CertRequestData
// Assembly: 1Ctoolbox, Version=2.4.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2A5A1506-7F8E-48A5-97C3-E797DB6697E3
// Assembly location: C:\Program Files (x86)\1Сtoolbox\1Ctoolbox.exe

using Newtonsoft.Json;

namespace AstralToolbox.CryptoAPI
{
  public class CertRequestData
  {
    public string providerName { get; set; }

    public uint providerCode { get; set; }

    public string signTool { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public CertAttribute[] certAttributes { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string[] enhKeyUsage { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public CertAttribute[] certPolicies { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public CertAltarnativeName[] certAltarnativeNames { get; set; }

    public int keyUsage { get; set; }

    public string containerName { get; set; }

    public string notBefore { get; set; }

    public string notAfter { get; set; }

    public byte? identificationKind { get; set; }

    public CertRequestData()
    {
      this.certAttributes = new CertAttribute[0];
      this.enhKeyUsage = new string[0];
      this.certPolicies = new CertAttribute[0];
      this.certAltarnativeNames = new CertAltarnativeName[0];
    }
  }
}
