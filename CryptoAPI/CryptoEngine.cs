// Decompiled with JetBrains decompiler
// Type: AstralToolbox.CryptoAPI.CryptoEngine
// Assembly: 1Ctoolbox, Version=2.4.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2A5A1506-7F8E-48A5-97C3-E797DB6697E3
// Assembly location: C:\Program Files (x86)\1Сtoolbox\1Ctoolbox.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace AstralToolbox.CryptoAPI
{
  public static class CryptoEngine
  {
    [DllImport("Advapi32.dll")]
    private static extern bool CryptEnumProviders(
      int dwIndex,
      IntPtr pdwReserved,
      int dwFlags,
      ref int pdwProvType,
      StringBuilder pszProvName,
      ref int pcbProvName);

    private static bool IsProvExists(string provName, uint provType)
    {
      int pcbProvName = 0;
      int pdwProvType = 1;
      int dwIndex = 0;
      while (CryptoEngine.CryptEnumProviders(dwIndex, IntPtr.Zero, 0, ref pdwProvType, (StringBuilder) null, ref pcbProvName))
      {
        StringBuilder pszProvName = new StringBuilder(pcbProvName);
        if (CryptoEngine.CryptEnumProviders(dwIndex++, IntPtr.Zero, 0, ref pdwProvType, pszProvName, ref pcbProvName) && pszProvName.ToString() == provName && (long) pdwProvType == (long) provType)
          return true;
      }
      return false;
    }

    private static void CheckProvider(string provName, uint provType)
    {
      if (!CryptoEngine.IsProvExists(provName, provType))
        throw new Exception(string.Format("В системе не найден криптопровайдер с названием \"{0}\" и типом {1}.", (object) provName, (object) provType));
    }

    public static string HexToBase64String(string hexString)
    {
      string base64String;
      AtclImports._TCL_HexToBase64(out base64String, hexString, false);
      return base64String;
    }

    public static string GetDataSignature(string base64Data, string subjectKeyId)
    {
      if (string.IsNullOrEmpty(subjectKeyId))
        throw new ArgumentException("Идентификатор ключа субъекта не может быть пустой строкой.");
      string str1 = Path.Combine(Path.GetTempPath(), string.Format("{0}.bin", (object) Guid.NewGuid()));
      byte[] bytes;
      try
      {
        bytes = Convert.FromBase64String(base64Data);
      }
      catch (Exception ex)
      {
        throw new Exception("Не удалось конвертировать данные для подписи из base64.", ex);
      }
      File.WriteAllBytes(str1, bytes);
      string str2 = Path.Combine(Path.GetTempPath(), string.Format("{0}.bin", (object) Guid.NewGuid()));
      TCL_CERT_INFO cert = new TCL_CERT_INFO()
      {
        SubjectKeyId = CryptoEngine.HexToBase64String(subjectKeyId)
      };
      int num = AtclImports._TCL_SignMsgEx(str1, str2, ref cert, 22);
      if (num != 0)
        throw new CryptoApiException(AtclImports._TCL_GetErrorMessage(num), num);
      return Convert.ToBase64String(File.ReadAllBytes(str2));
    }

    public static CertificateInfo[] GetCertificatesList()
    {
      X509Store x509Store = (X509Store) null;
      try
      {
        x509Store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
        x509Store.Open(OpenFlags.OpenExistingOnly);
        List<CertificateInfo> certificateInfoList1 = new List<CertificateInfo>();
        X509Certificate2Enumerator enumerator = x509Store.Certificates.GetEnumerator();
        while (enumerator.MoveNext())
        {
          X509Certificate2 current = enumerator.Current;
          string str = string.Empty;
          foreach (X509Extension extension in current.Extensions)
          {
            if (extension is X509SubjectKeyIdentifierExtension)
            {
              str = (extension as X509SubjectKeyIdentifierExtension).SubjectKeyIdentifier;
              break;
            }
          }
          List<CertificateInfo> certificateInfoList2 = certificateInfoList1;
          CertificateInfo certificateInfo = new CertificateInfo();
          DateTime dateTime = current.NotAfter;
          certificateInfo.NotAfter = dateTime.ToString("dd.MM.yyyy");
          dateTime = current.NotBefore;
          certificateInfo.NotBefore = dateTime.ToString("dd.MM.yyyy");
          certificateInfo.SubjectName = current.SubjectName.Name;
          certificateInfo.Thumbprint = current.Thumbprint;
          certificateInfo.Algorithm = current.PublicKey.Oid.Value;
          certificateInfo.SubjectKeyId = str;
          certificateInfoList2.Add(certificateInfo);
        }
        return certificateInfoList1.ToArray();
      }
      finally
      {
        x509Store?.Close();
      }
    }

    public static void CreateContainer(string containerName, string provName, uint provType)
    {
      CryptoEngine.CheckProvider(provName, provType);
      TCL_PROV_INFO providerInfo = new TCL_PROV_INFO()
      {
        provName = provName,
        provType = provType
      };
      int container = AtclImports._TCL_CreateContainer(ref containerName, ref providerInfo, 65536);
      if (container != 0)
        throw new CryptoApiException(AtclImports._TCL_GetErrorMessage(container), container);
    }

    public static void InstallCert(InstallCertData installCertData)
    {
      if (installCertData == null)
        throw new ArgumentNullException(nameof (installCertData));
      CryptoEngine.CheckProvider(installCertData.providerName, installCertData.providerCode);
      string str = Path.Combine(Path.GetTempPath(), installCertData.containerName + ".cer");
      File.WriteAllText(str, installCertData.CertData);
      TCL_PROV_INFO providerInfo = new TCL_PROV_INFO()
      {
        provName = installCertData.providerName,
        provType = installCertData.providerCode
      };
      int container = AtclImports._TCL_SetCertFromFileToContainer(str, ref providerInfo, installCertData.containerName, 0);
      if (container != 0)
        throw new CryptoApiException(AtclImports._TCL_GetErrorMessage(container), container);
    }

    private static TCL_CERT_ATTRIBUTE[] GetCertAttrs(CertAttribute[] certAttrs)
    {
      if (certAttrs == null || certAttrs.Length == 0)
        return (TCL_CERT_ATTRIBUTE[]) null;
      List<TCL_CERT_ATTRIBUTE> tclCertAttributeList = new List<TCL_CERT_ATTRIBUTE>();
      foreach (CertAttribute certAttr in certAttrs)
        tclCertAttributeList.Add(new TCL_CERT_ATTRIBUTE()
        {
          OID = certAttr.Oid,
          Value = certAttr.Value
        });
      return tclCertAttributeList.ToArray();
    }

    private static TCL_CERT_POLICY[] GetCertPolicies(CertAttribute[] certPolicies)
    {
      if (certPolicies == null || certPolicies.Length == 0)
        return (TCL_CERT_POLICY[]) null;
      List<TCL_CERT_POLICY> tclCertPolicyList = new List<TCL_CERT_POLICY>();
      foreach (CertAttribute certPolicy in certPolicies)
        tclCertPolicyList.Add(new TCL_CERT_POLICY()
        {
          OID = certPolicy.Oid,
          Value = certPolicy.Value
        });
      return tclCertPolicyList.ToArray();
    }

    private static TCL_CERT_ALT_NAME[] GetCertAltNames(
      CertAltarnativeName[] certAltNames)
    {
      if (certAltNames == null || certAltNames.Length == 0)
        return (TCL_CERT_ALT_NAME[]) null;
      List<TCL_CERT_ALT_NAME> tclCertAltNameList = new List<TCL_CERT_ALT_NAME>();
      foreach (CertAltarnativeName certAltName in certAltNames)
        tclCertAltNameList.Add(new TCL_CERT_ALT_NAME()
        {
          OID = certAltName.Oid,
          Value = certAltName.Value,
          Type = (uint) certAltName.Type
        });
      return tclCertAltNameList.ToArray();
    }

    public static string CreateQualifiedCertRequest(CertRequestData certRequestData)
    {
      if (certRequestData == null)
        throw new ArgumentNullException(nameof (certRequestData));
      CryptoEngine.CheckProvider(certRequestData.providerName, certRequestData.providerCode);
      TCL_CERT_ATTRIBUTE[] certAttrs = CryptoEngine.GetCertAttrs(certRequestData.certAttributes);
      TCL_CERT_POLICY[] certPolicies = CryptoEngine.GetCertPolicies(certRequestData.certPolicies);
      TCL_CERT_ALT_NAME[] certAltNames = CryptoEngine.GetCertAltNames(certRequestData.certAltarnativeNames);
      string str = Path.Combine(Path.GetTempPath(), certRequestData.containerName + ".p10");
      TCL_PROV_INFO providerInfo = new TCL_PROV_INFO()
      {
        provName = certRequestData.providerName,
        provType = certRequestData.providerCode
      };
      int qualifiedCertRequestEx = AtclImports._TCL_CreateQualifiedCertRequestEx(certAttrs, certAttrs != null ? certAttrs.Length : 0, certRequestData.enhKeyUsage, certRequestData.enhKeyUsage != null ? certRequestData.enhKeyUsage.Length : 0, certPolicies, certPolicies != null ? certPolicies.Length : 0, (string[]) null, 0, certAltNames, certAltNames != null ? certAltNames.Length : 0, certRequestData.keyUsage, certRequestData.signTool, certRequestData.containerName, ref providerInfo, str, certRequestData.notBefore, certRequestData.notAfter, 0);
      if (qualifiedCertRequestEx != 0)
        throw new CryptoApiException(AtclImports._TCL_GetErrorMessage(qualifiedCertRequestEx), qualifiedCertRequestEx);
      return File.ReadAllText(str);
    }

    public static string CreateQualifiedCertRequestIdent(CertRequestData certRequestData)
    {
      string str = Path.Combine(Path.GetTempPath(), certRequestData.containerName + ".p10");
      if (certRequestData == null)
        throw new ArgumentNullException(nameof (certRequestData));
      CryptoEngine.CheckProvider(certRequestData.providerName, certRequestData.providerCode);
      CERT_REQUEST_IDENT ident = new CERT_REQUEST_IDENT()
      {
        Container = certRequestData.containerName,
        KeyUsage = certRequestData.keyUsage,
        NotBefore = certRequestData.notBefore,
        NotAfter = certRequestData.notAfter,
        SubjectSignTool = string.IsNullOrWhiteSpace(certRequestData.signTool) ? (string) null : certRequestData.signTool,
        IdentKind = certRequestData.identificationKind.Value,
        Flags = 0
      };
      TCL_PROV_INFO tclProvInfo = new TCL_PROV_INFO()
      {
        provName = certRequestData.providerName,
        provType = certRequestData.providerCode
      };
      ident.ProvInfo = Marshal.AllocHGlobal(Marshal.SizeOf((object) tclProvInfo));
      Marshal.StructureToPtr((object) tclProvInfo, ident.ProvInfo, false);
      TCL_CERT_ATTRIBUTE[] tclCertAttributeArray = new TCL_CERT_ATTRIBUTE[certRequestData.certAttributes.Length];
      ident.DistributionNameLength = certRequestData.certAttributes.Length;
      ident.DistributionName = Marshal.AllocHGlobal(Marshal.SizeOf(typeof (TCL_CERT_ATTRIBUTE)) * certRequestData.certAttributes.Length);
      for (int index = 0; index < certRequestData.certAttributes.Length; ++index)
      {
        tclCertAttributeArray[index].OID = certRequestData.certAttributes[index].Oid;
        tclCertAttributeArray[index].Value = certRequestData.certAttributes[index].Value;
        Marshal.StructureToPtr((object) tclCertAttributeArray[index], IntPtr.Add(ident.DistributionName, index * Marshal.SizeOf(typeof (TCL_CERT_ATTRIBUTE))), false);
      }
      ident.EnhancedKeyUsageLength = certRequestData.enhKeyUsage.Length;
      ident.EnhancedKeyUsage = Marshal.AllocHGlobal(IntPtr.Size * certRequestData.enhKeyUsage.Length);
      for (int index = 0; index < certRequestData.enhKeyUsage.Length; ++index)
      {
        IntPtr hglobalUni = Marshal.StringToHGlobalUni(certRequestData.enhKeyUsage[index]);
        Marshal.WriteIntPtr(IntPtr.Add(ident.EnhancedKeyUsage, index * IntPtr.Size), hglobalUni);
      }
      ident.PoliciesLength = certRequestData.certPolicies.Length;
      ident.Policies = Marshal.AllocHGlobal(Marshal.SizeOf(typeof (TCL_CERT_POLICY)) * certRequestData.certPolicies.Length);
      TCL_CERT_POLICY[] tclCertPolicyArray = new TCL_CERT_POLICY[certRequestData.certPolicies.Length];
      for (int index = 0; index < certRequestData.certPolicies.Length; ++index)
      {
        tclCertPolicyArray[index].OID = certRequestData.certPolicies[index].Oid;
        tclCertPolicyArray[index].Value = certRequestData.certPolicies[index].Value;
        Marshal.StructureToPtr((object) tclCertPolicyArray[index], IntPtr.Add(ident.Policies, index * Marshal.SizeOf(typeof (TCL_CERT_POLICY))), false);
      }
      TCL_CERT_ALT_NAME[] tclCertAltNameArray = new TCL_CERT_ALT_NAME[certRequestData.certAltarnativeNames.Length];
      ident.AltNameLength = certRequestData.certAltarnativeNames.Length;
      ident.AltNames = Marshal.AllocHGlobal(Marshal.SizeOf(typeof (TCL_CERT_ALT_NAME)) * certRequestData.certAltarnativeNames.Length);
      for (int index = 0; index < certRequestData.certAltarnativeNames.Length; ++index)
      {
        tclCertAltNameArray[index].OID = certRequestData.certAltarnativeNames[index].Oid;
        tclCertAltNameArray[index].Type = (uint) certRequestData.certAltarnativeNames[index].Type;
        tclCertAltNameArray[index].Value = certRequestData.certAltarnativeNames[index].Value;
        Marshal.StructureToPtr((object) tclCertAltNameArray[index], IntPtr.Add(ident.AltNames, index * Marshal.SizeOf(typeof (TCL_CERT_ALT_NAME))), false);
      }
      int certRequestIdent = AtclImports._TCL_CreateQualifiedCertRequestIdent(ref ident, str);
      if (certRequestIdent != 0)
        throw new CryptoApiException(AtclImports._TCL_GetErrorMessage(certRequestIdent), certRequestIdent);
      return File.ReadAllText(str);
    }

    public static void AddSslRootCert(string certPath)
    {
      X509Store x509Store = (X509Store) null;
      try
      {
        x509Store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
        x509Store.Open(OpenFlags.ReadWrite);
        X509Certificate2 certificate = new X509Certificate2(certPath);
        if (x509Store.Certificates.Find(X509FindType.FindByThumbprint, (object) certificate.Thumbprint, false).Count != 0)
          return;
        x509Store.Add(certificate);
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("Не удалось установить сертификат. {0}", (object) ex.Message), ex);
      }
      finally
      {
        x509Store?.Close();
      }
    }
  }
}
