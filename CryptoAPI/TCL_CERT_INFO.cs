// Decompiled with JetBrains decompiler
// Type: AstralToolbox.CryptoAPI.TCL_CERT_INFO
// Assembly: 1Ctoolbox, Version=2.4.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2A5A1506-7F8E-48A5-97C3-E797DB6697E3
// Assembly location: C:\Program Files (x86)\1Сtoolbox\1Ctoolbox.exe

using System.Runtime.InteropServices;

namespace AstralToolbox.CryptoAPI
{
  public struct TCL_CERT_INFO
  {
    [MarshalAs(UnmanagedType.BStr)]
    public string CommonName;
    [MarshalAs(UnmanagedType.BStr)]
    public string SerialNumber;
    [MarshalAs(UnmanagedType.BStr)]
    public string Title;
    [MarshalAs(UnmanagedType.BStr)]
    public string OrganizationalUnit;
    [MarshalAs(UnmanagedType.BStr)]
    public string Organization;
    [MarshalAs(UnmanagedType.BStr)]
    public string Locality;
    [MarshalAs(UnmanagedType.BStr)]
    public string Area;
    [MarshalAs(UnmanagedType.BStr)]
    public string Country;
    [MarshalAs(UnmanagedType.BStr)]
    public string Email;
    [MarshalAs(UnmanagedType.BStr)]
    public string PostalAddress;
    [MarshalAs(UnmanagedType.BStr)]
    public string SubjectKeyId;
    [MarshalAs(UnmanagedType.BStr)]
    public string Thumbprint;

    public override string ToString() => this.CommonName;
  }
}
