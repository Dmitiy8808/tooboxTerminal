// Decompiled with JetBrains decompiler
// Type: AstralToolbox.CryptoAPI.CryptoApiException
// Assembly: 1Ctoolbox, Version=2.4.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2A5A1506-7F8E-48A5-97C3-E797DB6697E3
// Assembly location: C:\Program Files (x86)\1Сtoolbox\1Ctoolbox.exe

using System;

namespace AstralToolbox.CryptoAPI
{
  public class CryptoApiException : Exception
  {
    public CryptoApiException(string message, int code = -1)
      : base(message)
    {
      this.Code = code;
    }

    public int Code { get; set; }
  }
}
