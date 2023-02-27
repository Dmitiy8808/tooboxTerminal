// Decompiled with JetBrains decompiler
// Type: AstralToolbox.CryptoAPI.AtclImports
// Assembly: 1Ctoolbox, Version=2.4.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2A5A1506-7F8E-48A5-97C3-E797DB6697E3
// Assembly location: C:\Program Files (x86)\1Сtoolbox\1Ctoolbox.exe

using System.Runtime.InteropServices;

namespace AstralToolbox.CryptoAPI
{
  public static class AtclImports
  {
    [DllImport("atcl.dll", EntryPoint = "TCL_GetErrorMessage")]
    [return: MarshalAs(UnmanagedType.BStr)]
    public static extern string _TCL_GetErrorMessage(int ErrorCode);

    [DllImport("atcl.dll", EntryPoint = "TCL_ContainerExists")]
    public static extern bool _TCL_ContainerExists(
      [MarshalAs(UnmanagedType.BStr)] string containerName,
      ref TCL_PROV_INFO providerInfo);

    [DllImport("atcl.dll", EntryPoint = "TCL_HexToBase64")]
    public static extern int _TCL_HexToBase64(
      [MarshalAs(UnmanagedType.BStr)] out string base64String,
      [MarshalAs(UnmanagedType.BStr)] string hexString,
      bool reverse);

    [DllImport("atcl.dll", EntryPoint = "TCL_CreateContainer")]
    public static extern int _TCL_CreateContainer(
      [MarshalAs(UnmanagedType.BStr)] ref string containerName,
      ref TCL_PROV_INFO providerInfo,
      int flags);

    [DllImport("atcl.dll", EntryPoint = "TCL_CreateQualifiedCertRequestEx", CharSet = CharSet.Unicode)]
    public static extern int _TCL_CreateQualifiedCertRequestEx(
      [In] TCL_CERT_ATTRIBUTE[] сertAttrs,
      int сertAttrsCount,
      [In] string[] certEnhKeyUsage,
      int certEnhKeyUsageCount,
      [In] TCL_CERT_POLICY[] certPolicies,
      int certPoliciesCount,
      [In] string[] certAppPolicies,
      int certAppPoliciesCount,
      [In] TCL_CERT_ALT_NAME[] certAltNames,
      int certAltNamesCount,
      int keyUsage,
      [MarshalAs(UnmanagedType.BStr)] string subjectSignTool,
      [MarshalAs(UnmanagedType.BStr)] string containerName,
      ref TCL_PROV_INFO providerInfo,
      [MarshalAs(UnmanagedType.BStr)] string requestFilePath,
      [MarshalAs(UnmanagedType.BStr)] string notBefore,
      [MarshalAs(UnmanagedType.BStr)] string notAfter,
      int flags);

    [DllImport("atcl.dll", EntryPoint = "TCL_SetCertFromFileToContainer")]
    public static extern int _TCL_SetCertFromFileToContainer(
      [MarshalAs(UnmanagedType.BStr)] string certFilePath,
      ref TCL_PROV_INFO providerInfo,
      [MarshalAs(UnmanagedType.BStr)] string containerName,
      int flags);

    [DllImport("atcl.dll", EntryPoint = "TCL_SignMsgEx")]
    public static extern int _TCL_SignMsgEx(
      [MarshalAs(UnmanagedType.BStr)] string fileName,
      [MarshalAs(UnmanagedType.BStr)] string signFileName,
      ref TCL_CERT_INFO cert,
      int dwFlags);

    [DllImport("atcl.dll", EntryPoint = "TCL_CreateQualifiedRequestIdent", CharSet = CharSet.Unicode)]
    public static extern int _TCL_CreateQualifiedCertRequestIdent(
      ref CERT_REQUEST_IDENT ident,
      [MarshalAs(UnmanagedType.LPWStr)] string reqFilePath);
  }
}
