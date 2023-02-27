// Decompiled with JetBrains decompiler
// Type: AstralToolbox.Properties.Resources
// Assembly: 1Ctoolbox, Version=2.4.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2A5A1506-7F8E-48A5-97C3-E797DB6697E3
// Assembly location: C:\Program Files (x86)\1Сtoolbox\1Ctoolbox.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace AstralToolbox.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (AstralToolbox.Properties.Resources.resourceMan == null)
          AstralToolbox.Properties.Resources.resourceMan = new ResourceManager("AstralToolbox.Properties.Resources", typeof (AstralToolbox.Properties.Resources).Assembly);
        return AstralToolbox.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => AstralToolbox.Properties.Resources.resourceCulture;
      set => AstralToolbox.Properties.Resources.resourceCulture = value;
    }

    internal static string CanNotLoadNativeCompnents => AstralToolbox.Properties.Resources.ResourceManager.GetString(nameof (CanNotLoadNativeCompnents), AstralToolbox.Properties.Resources.resourceCulture);

    internal static string DialogsTitle => AstralToolbox.Properties.Resources.ResourceManager.GetString(nameof (DialogsTitle), AstralToolbox.Properties.Resources.resourceCulture);

    internal static string NecessaryFrameworkNotInstalled => AstralToolbox.Properties.Resources.ResourceManager.GetString(nameof (NecessaryFrameworkNotInstalled), AstralToolbox.Properties.Resources.resourceCulture);
  }
}
