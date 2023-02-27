// Decompiled with JetBrains decompiler
// Type: AstralToolbox.CryptoAPI.TCL_CERT_ALT_NAME
// Assembly: 1Ctoolbox, Version=2.4.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2A5A1506-7F8E-48A5-97C3-E797DB6697E3
// Assembly location: C:\Program Files (x86)\1Сtoolbox\1Ctoolbox.exe

using System.Runtime.InteropServices;

namespace AstralToolbox.CryptoAPI
{
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct TCL_CERT_ALT_NAME
  {
    public uint Type;
    [MarshalAs(UnmanagedType.BStr)]
    public string OID;
    [MarshalAs(UnmanagedType.BStr)]
    public string Value;
  }
}
