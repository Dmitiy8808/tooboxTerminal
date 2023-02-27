// Decompiled with JetBrains decompiler
// Type: AstralToolbox.Program
// Assembly: 1Ctoolbox, Version=2.4.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2A5A1506-7F8E-48A5-97C3-E797DB6697E3
// Assembly location: C:\Program Files (x86)\1Сtoolbox\1Ctoolbox.exe

using System;
using System.Threading;
using System.Windows.Forms;

namespace AstralToolbox
{
  internal static class Program
  {
    private static Mutex _mutex;
    private const string _appName = "1Ctoolbox";

    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      bool createdNew;
      Program._mutex = new Mutex(true, "1Ctoolbox", out createdNew);
      if (createdNew)
      {
        Application.Run((Form) new MainForm());
      }
      else
      {
        int num = (int) MessageBox.Show("Приложение 1Ctoolbox уже запущено", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }
  }
}
