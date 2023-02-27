// Decompiled with JetBrains decompiler
// Type: AstralToolbox.CryptoAPI.CERT_REQUEST_IDENT
// Assembly: 1Ctoolbox, Version=2.4.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2A5A1506-7F8E-48A5-97C3-E797DB6697E3
// Assembly location: C:\Program Files (x86)\1Сtoolbox\1Ctoolbox.exe

using System;
using System.Runtime.InteropServices;

namespace AstralToolbox.CryptoAPI
{
  public struct CERT_REQUEST_IDENT
  {
    public IntPtr DistributionName;
    public int DistributionNameLength;
    public IntPtr EnhancedKeyUsage;
    public int EnhancedKeyUsageLength;
    public IntPtr Policies;
    public int PoliciesLength;
    public int KeyUsage;
    [MarshalAs(UnmanagedType.LPWStr)]
    public string NotBefore;
    [MarshalAs(UnmanagedType.LPWStr)]
    public string NotAfter;
    public IntPtr AppPolicies;
    public int AppPoliciesLength;
    [MarshalAs(UnmanagedType.LPWStr)]
    public string SubjectSignTool;
    public IntPtr AltNames;
    public int AltNameLength;
    [MarshalAs(UnmanagedType.LPWStr)]
    public string TemplateOid;
    public byte IdentKind;
    [MarshalAs(UnmanagedType.LPWStr)]
    public string Container;
    public IntPtr ProvInfo;
    public uint Flags;
  }
}
