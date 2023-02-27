// Decompiled with JetBrains decompiler
// Type: AstralToolbox.WebSocketSettings
// Assembly: 1Ctoolbox, Version=2.4.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2A5A1506-7F8E-48A5-97C3-E797DB6697E3
// Assembly location: C:\Program Files (x86)\1Сtoolbox\1Ctoolbox.exe

using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AstralToolbox
{
  [CompilerGenerated]
  [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.9.0.0")]
  internal sealed class WebSocketSettings : ApplicationSettingsBase
  {
    private static WebSocketSettings defaultInstance = (WebSocketSettings) SettingsBase.Synchronized((SettingsBase) new WebSocketSettings());

    public static WebSocketSettings Default => WebSocketSettings.defaultInstance;

    [UserScopedSetting]
    [DebuggerNonUserCode]
    //[DefaultSettingValue("9292")]
    public int Port
    {
      get => (int) this[nameof (Port)];
      set => this[nameof (Port)] = (object) value;
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("True")]
    public bool SecureWebSocket
    {
      get => (bool) this[nameof (SecureWebSocket)];
      set => this[nameof (SecureWebSocket)] = (object) value;
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("True")]
    public bool ShowInTray
    {
      get => (bool) this[nameof (ShowInTray)];
      set => this[nameof (ShowInTray)] = (object) value;
    }
  }
}
