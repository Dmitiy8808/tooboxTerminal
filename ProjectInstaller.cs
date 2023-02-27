// Decompiled with JetBrains decompiler
// Type: AstralToolbox.ProjectInstaller
// Assembly: 1Ctoolbox, Version=2.4.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2A5A1506-7F8E-48A5-97C3-E797DB6697E3
// Assembly location: C:\Program Files (x86)\1Сtoolbox\1Ctoolbox.exe

using AstralToolbox.Properties;
using Microsoft.Win32;
using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AstralToolbox
{
  [RunInstaller(true)]
  public class ProjectInstaller : Installer
  {
    private IContainer components;

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    private static void MessageBox(string messageText)
    {
      int num = (int) System.Windows.Forms.MessageBox.Show((IWin32Window) new ProjectInstaller.WindowWrapper()
      {
        Handle = ProjectInstaller.FindWindow("MsiDialogCloseClass", "1Сtoolbox")
      }, messageText, Resources.DialogsTitle, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    public ProjectInstaller()
    {
      this.InitializeComponent();
      this.Committed += new InstallEventHandler(this.InstallerCommitted);
    }

    protected void KillPluginProcess()
    {
      foreach (Process process in Process.GetProcesses())
      {
        if (string.Compare(process.ProcessName, "1Ctoolbox", true, CultureInfo.InvariantCulture) == 0)
        {
          process.Kill();
          process.WaitForExit();
        }
      }
    }

    protected override void OnBeforeInstall(IDictionary savedState)
    {
      this.KillPluginProcess();
      base.OnBeforeInstall(savedState);
    }

    protected override void OnAfterInstall(IDictionary savedState)
    {
      base.OnAfterInstall(savedState);
      ProjectInstaller.MessageBox("Приложение успешно установлено.");
    }

    public override void Commit(IDictionary savedState) => base.Commit(savedState);

    public override void Install(IDictionary stateSaver) => base.Install(stateSaver);

    protected override void OnBeforeUninstall(IDictionary savedState)
    {
      this.KillPluginProcess();
      base.OnBeforeUninstall(savedState);
    }

    public override void Uninstall(IDictionary savedState) => base.Uninstall(savedState);

    private void InstallerCommitted(object sender, InstallEventArgs e)
    {
      try
      {
        if (ProjectInstaller.IsWindows10())
          return;
        Process.Start(Assembly.GetExecutingAssembly().Location);
      }
      catch
      {
      }
    }

    private static bool IsWindows10() => ((string) Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion").GetValue("ProductName")).StartsWith("Windows 10");

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() => this.components = (IContainer) new Container();

    private class WindowWrapper : IWin32Window
    {
      public IntPtr Handle { get; set; }
    }
  }
}
