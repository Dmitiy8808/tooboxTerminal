// Decompiled with JetBrains decompiler
// Type: AstralToolbox.CryptoAPI.JaCartaCertRequestData
// Assembly: 1Ctoolbox, Version=2.4.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2A5A1506-7F8E-48A5-97C3-E797DB6697E3
// Assembly location: C:\Program Files (x86)\1Сtoolbox\1Ctoolbox.exe

using System.Runtime.InteropServices;

namespace AstralToolbox.CryptoAPI
{
  [StructLayout(LayoutKind.Sequential)]
  public class JaCartaCertRequestData
  {
    public int AbonentType;
    [MarshalAs(UnmanagedType.LPWStr)]
    public string ContainerName;
    [MarshalAs(UnmanagedType.LPWStr)]
    public string OGRNIP;
    [MarshalAs(UnmanagedType.LPWStr)]
    public string SNILS;
    [MarshalAs(UnmanagedType.LPWStr)]
    public string G;
    [MarshalAs(UnmanagedType.LPWStr)]
    public string SN;
    [MarshalAs(UnmanagedType.LPWStr)]
    public string CN;
    [MarshalAs(UnmanagedType.LPWStr)]
    public string L;
    [MarshalAs(UnmanagedType.LPWStr)]
    public string S;
    [MarshalAs(UnmanagedType.LPWStr)]
    public string C;
    [MarshalAs(UnmanagedType.LPWStr)]
    public string E;
    [MarshalAs(UnmanagedType.LPWStr)]
    public string INN;
    [MarshalAs(UnmanagedType.LPWStr)]
    public string OGRN;
    [MarshalAs(UnmanagedType.LPWStr)]
    public string OU;
    [MarshalAs(UnmanagedType.LPWStr)]
    public string O;
    [MarshalAs(UnmanagedType.LPWStr)]
    public string T;
    [MarshalAs(UnmanagedType.LPWStr)]
    public string Street;
    [MarshalAs(UnmanagedType.LPWStr)]
    public string KPP;
  }
}
