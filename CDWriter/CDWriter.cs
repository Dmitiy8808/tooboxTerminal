// Decompiled with JetBrains decompiler
// Type: AstralToolbox.CDWriter.CDWriter
// Assembly: 1Ctoolbox, Version=2.4.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2A5A1506-7F8E-48A5-97C3-E797DB6697E3
// Assembly location: C:\Program Files (x86)\1Сtoolbox\1Ctoolbox.exe

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace AstralToolbox.CDWriter
{
  public static class CDWriter
  {
    private static bool IsOperatinSystemXp() => Environment.OSVersion.Version.Major == 5;

    private static string GetToolFilePath(string toolFileName) => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), toolFileName);

    public static void Write(CDWriteData cdWriteData)
    {
      if (cdWriteData == null)
        throw new ArgumentNullException(nameof (cdWriteData));
      string toolFilePath = AstralToolbox.CDWriter.CDWriter.GetToolFilePath("CreateCD.exe");
      if (!File.Exists(toolFilePath))
        throw new Exception(string.Format("{0} не найден.", (object) toolFilePath));
      string str1 = "\"C:\\AstralReportDistr\\*.*\"";
      string environmentVariable = Environment.GetEnvironmentVariable("APPDATA");
      string str2 = str1 + string.Format(" \"{0}\"", (object) Path.Combine(environmentVariable, string.Format("AstralCryptoServices\\PrimaryRegPacketStore\\{0}", (object) cdWriteData.RegFileName)));
      if (cdWriteData.ContainerNames != null)
      {
        foreach (string containerName in cdWriteData.ContainerNames)
          str2 += string.Format(" \"{0}\"", (object) Path.Combine(environmentVariable, string.Format("AstralCryptoServices\\containers\\{0}", (object) containerName)));
      }
      Process process = new Process();
      process.StartInfo.FileName = toolFilePath;
      process.StartInfo.Arguments = str2;
      process.StartInfo.CreateNoWindow = true;
      process.StartInfo.RedirectStandardInput = true;
      process.StartInfo.RedirectStandardOutput = true;
      process.StartInfo.UseShellExecute = false;
      process.Start();
      process.BeginOutputReadLine();
      process.WaitForExit();
      process.CancelOutputRead();
      if (process.ExitCode != 0)
      {
        string message = "Не удалось записать данные. Убедитесь, что в системе имеется дисковод с возможностью записи и в нём находится диск.";
        if (AstralToolbox.CDWriter.CDWriter.IsOperatinSystemXp())
          message = string.Format("{0} Также должно быть установлено обновление WindowsXP-KB932716-v2 и запущенна служба \"Служба COM записи компакт-дисков IMAPI\".", (object) message);
        throw new Exception(message);
      }
    }
  }
}
