// Decompiled with JetBrains decompiler
// Type: AstralToolbox.Common.DllLoader
// Assembly: 1Ctoolbox, Version=2.4.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2A5A1506-7F8E-48A5-97C3-E797DB6697E3
// Assembly location: C:\Program Files (x86)\1Сtoolbox\1Ctoolbox.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace AstralToolbox.Common
{
  public sealed class DllLoader
  {
    private static readonly Dictionary<string, IntPtr> libraryes = new Dictionary<string, IntPtr>();

    public static bool LoadLibrary(string dllPath)
    {
      string fileName = Path.GetFileName(dllPath);
      if (DllLoader.libraryes.ContainsKey(fileName))
        return true;
      IntPtr num = DllLoader._LoadLibrary(dllPath);
      if (num.ToInt64() == 0L)
        return false;
      DllLoader.libraryes.Add(fileName, num);
      return true;
    }

    public static void FreeLibrary(string dllName)
    {
      IntPtr library;
      if (!DllLoader.libraryes.TryGetValue(dllName, out library))
        return;
      DllLoader.FreeLibrary(library);
      DllLoader.libraryes.Remove(dllName);
    }

    [DllImport("kernel32.dll", EntryPoint = "LoadLibraryA")]
    private static extern IntPtr _LoadLibrary(string dllName);

    [DllImport("kernel32.dll")]
    private static extern IntPtr FreeLibrary(IntPtr library);

    [DllImport("kernel32.dll")]
    public static extern int GetLastError();
  }
}
