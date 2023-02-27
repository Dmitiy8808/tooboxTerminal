// Decompiled with JetBrains decompiler
// Type: AstralToolbox.Messaging.AstralWebSocketBehavior
// Assembly: 1Ctoolbox, Version=2.4.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2A5A1506-7F8E-48A5-97C3-E797DB6697E3
// Assembly location: C:\Program Files (x86)\1Сtoolbox\1Ctoolbox.exe

using AstralToolbox.CDWriter;
using AstralToolbox.CryptoAPI;
using AstralToolbox.HttpClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace AstralToolbox.Messaging
{
  internal class AstralWebSocketBehavior : WebSocketBehavior
  {
    public AstralWebSocketBehavior() => this.IgnoreExtensions = true;

    protected override void OnMessage(MessageEventArgs e)
    {
      if (e.Type != Opcode.Text)
        return;
      try
      {
        this.Send(JsonConvert.SerializeObject((object) AstralWebSocketBehavior.ProccessMessage(JsonConvert.DeserializeObject<Message>(e.Data))));
      }
      catch (Exception ex)
      {
        this.Send(JsonConvert.SerializeObject((object) new MessageResponse()
        {
          Success = false,
          Message = ex.Message
        }));
      }
    }

    private static MessageResponse ProccessMessage(Message message)
    {
      try
      {
        switch (message.Code)
        {
          case 1:
            return AstralWebSocketBehavior.CreateCertRequest(message);
          case 2:
            return AstralWebSocketBehavior.InstallCert(message);
          case 3:
            return AstralWebSocketBehavior.DownloadFile(message);
          case 4:
            return AstralWebSocketBehavior.WriteCD(message);
          case 5:
            return AstralWebSocketBehavior.GetVersion();
          case 6:
            return AstralWebSocketBehavior.GetCertificateList();
          case 7:
            return AstralWebSocketBehavior.GetDataSignature(message);
          case 8:
            return AstralWebSocketBehavior.CreateJaCartaCertRequest(message);
          case 9:
            return AstralWebSocketBehavior.InstallJaCartaCert(message);
          case 10:
            return AstralWebSocketBehavior.GetContainerFolder();
        }
      }
      catch (JsonSerializationException ex)
      {
        return new MessageResponse()
        {
          Success = false,
          Message = string.Format("Во время разбора JSON строки произошла ошибка. Убедитесь в верности данных и их формата. {0}", (object) ex.Message)
        };
      }
      catch (Exception ex)
      {
        return new MessageResponse()
        {
          Success = false,
          Message = ex.Message
        };
      }
      return new MessageResponse()
      {
        Success = false,
        Message = "Неизвестный код сообщения. Возможно вы используете устаревшую версию ПО."
      };
    }

    private static MessageResponse GetDataSignature(Message message)
    {
      DataSignatureParams dataSignatureParams = JsonConvert.DeserializeObject<DataSignatureParams>(message.Data);
      return new MessageResponse()
      {
        Success = true,
        Data = (object) CryptoEngine.GetDataSignature(dataSignatureParams.Base64Data, dataSignatureParams.SubjectKeyId)
      };
    }

    private static MessageResponse GetCertificateList() => new MessageResponse()
    {
      Success = true,
      Data = (object) CryptoEngine.GetCertificatesList()
    };

    private static MessageResponse GetVersion() => new MessageResponse()
    {
      Success = true,
      Data = (object) new{ version = 12 }
    };

    private static MessageResponse DownloadFile(Message message)
    {
      AstralToolbox.HttpClient.HttpClient.DownloadFile(JsonConvert.DeserializeObject<DownloadFileData>(message.Data));
      return new MessageResponse() { Success = true };
    }

    private static MessageResponse InstallCert(Message message)
    {
      try
      {
        CryptoEngine.InstallCert(JsonConvert.DeserializeObject<InstallCertData>(message.Data));
        return new MessageResponse()
        {
          Success = true,
          Data = (object) new{ code = 0 }
        };
      }
      catch (CryptoApiException ex)
      {
        return new MessageResponse()
        {
          Success = false,
          Message = ex.Message,
          Data = (object) new
          {
            code = ex.Code,
            message = ex.Message
          }
        };
      }
      catch (Exception ex)
      {
        return new MessageResponse()
        {
          Success = false,
          Message = ex.Message,
          Data = (object) new
          {
            code = -1,
            message = ex.Message
          }
        };
      }
    }

    private static MessageResponse InstallJaCartaCert(Message message)
    {
      try
      {
        JaCartaCryptoEngine.InstallCert(JsonConvert.DeserializeObject<JaCartaInstalCertData>(message.Data));
        return new MessageResponse() { Success = true };
      }
      catch (Exception ex)
      {
        return new MessageResponse()
        {
          Success = false,
          Message = ex.Message
        };
      }
    }

    private static MessageResponse CreateJaCartaCertRequest(Message message)
    {
      try
      {
        string certRequest = JaCartaCryptoEngine.CreateCertRequest(JsonConvert.DeserializeObject<JaCartaCertRequestParam>(message.Data));
        return new MessageResponse()
        {
          Success = true,
          Data = (object) certRequest
        };
      }
      catch (Exception ex)
      {
        return new MessageResponse()
        {
          Success = false,
          Message = ex.Message
        };
      }
    }

    private static MessageResponse CreateCertRequest(Message message)
    {
      try
      {
        CertRequestData certRequestData = JsonConvert.DeserializeObject<CertRequestData>(message.Data);
        CryptoEngine.CreateContainer(certRequestData.containerName, certRequestData.providerName, certRequestData.providerCode);
        string str = certRequestData.identificationKind.HasValue ? CryptoEngine.CreateQualifiedCertRequestIdent(certRequestData) : CryptoEngine.CreateQualifiedCertRequest(certRequestData);
        return new MessageResponse()
        {
          Success = true,
          Data = (object) new{ code = 0, value = str }
        };
      }
      catch (CryptoApiException ex)
      {
        return new MessageResponse()
        {
          Success = false,
          Message = ex.Message,
          Data = (object) new
          {
            code = ex.Code,
            message = ex.Message
          }
        };
      }
      catch (Exception ex)
      {
        return new MessageResponse()
        {
          Success = false,
          Message = ex.Message,
          Data = (object) new
          {
            code = -1,
            message = ex.Message
          }
        };
      }
    }

    private static MessageResponse WriteCD(Message message)
    {
      AstralToolbox.CDWriter.CDWriter.Write(JsonConvert.DeserializeObject<CDWriteData>(message.Data));
      return new MessageResponse() { Success = true };
    }

    private static MessageResponse GetContainerFolder()
    {
      string folder = "";
      AstralWebSocketBehavior.GetDirectories(ref folder, "c:\\Users", "Infotecs");
      return new MessageResponse()
      {
        Success = true,
        Data = (object) folder
      };
    }

    private static void GetDirectories(ref string folder, string path, string search)
    {
      try
      {
        string str = ((IEnumerable<string>) Directory.GetDirectories(path)).FirstOrDefault<string>((Func<string, bool>) (d => d.Contains(search)));
        if (!string.IsNullOrEmpty(str))
        {
          folder = str;
        }
        else
        {
          foreach (string directory in Directory.GetDirectories(path))
            AstralWebSocketBehavior.GetDirectories(ref folder, directory, search);
        }
      }
      catch (Exception ex)
      {
      }
    }
  }
}
