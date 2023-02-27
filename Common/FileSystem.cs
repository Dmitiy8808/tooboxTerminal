// Decompiled with JetBrains decompiler
// Type: AstralToolbox.Common.FileSystem
// Assembly: 1Ctoolbox, Version=2.4.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2A5A1506-7F8E-48A5-97C3-E797DB6697E3
// Assembly location: C:\Program Files (x86)\1Сtoolbox\1Ctoolbox.exe

using System.IO;
using System.Reflection;

namespace AstralToolbox.Common
{
  public static class FileSystem
  {
    private static string _moduleFolderPath;

    public static string ModuleFolderPath
    {
      get
      {
        if (!string.IsNullOrEmpty(FileSystem._moduleFolderPath))
          return FileSystem._moduleFolderPath;
        FileSystem._moduleFolderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        return FileSystem._moduleFolderPath;
      }
    }
  }
}
