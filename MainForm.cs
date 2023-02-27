// Decompiled with JetBrains decompiler
// Type: AstralToolbox.MainForm
// Assembly: 1Ctoolbox, Version=2.4.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2A5A1506-7F8E-48A5-97C3-E797DB6697E3
// Assembly location: C:\Program Files (x86)\1Сtoolbox\1Ctoolbox.exe

using AstralToolbox.Common;
using AstralToolbox.CryptoAPI;
using AstralToolbox.Messaging;
using AstralToolbox.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace AstralToolbox
{
  public class MainForm : Form
  {
    private string _lastErrorMsg;
    private WebSocketServer _server;
    private IContainer components;
    private NotifyIcon notifyIcon;
    private ContextMenuStrip trayMenu;
    private ToolStripMenuItem menuItemExit;
    private ToolStripMenuItem menuItemRun;
    private ToolStripMenuItem menuItemStop;
    private ToolStripMenuItem menuItemMode;
    private ToolStripMenuItem menuItemHttp;
    private ToolStripMenuItem menuItemHttps;
    private ToolStripMenuItem menuItemHide;








    public MainForm()
    {
     

      this.InitializeComponent();
      if (WebSocketSettings.Default.ShowInTray)
        return;
      this.notifyIcon.Visible = false;
    }

    private void ShowBalloonTip(string tipText, ToolTipIcon tipIcon) => this.notifyIcon.ShowBalloonTip(100, Resources.DialogsTitle, tipText, tipIcon);

    private void UpdateControlsState()
    {
      this.menuItemRun.Enabled = this._server != null && !this._server.IsListening;
      this.menuItemStop.Enabled = this._server != null && this._server.IsListening;
      this.menuItemHttps.Enabled = !WebSocketSettings.Default.SecureWebSocket;
      this.menuItemHttp.Enabled = WebSocketSettings.Default.SecureWebSocket;
      this.menuItemHttps.Checked = WebSocketSettings.Default.SecureWebSocket;
      this.menuItemHttp.Checked = !WebSocketSettings.Default.SecureWebSocket;
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
      this.Hide();
      this.StartListening();
      this.UpdateControlsState();
    }

    private void StopListening(CloseStatusCode code, string reason)
    {
      this._lastErrorMsg = string.Empty;
      if (this._server == null || !this._server.IsListening)
        return;
      this._server.Stop(code, reason);
    }

    

    private void StartListening()
    {

     var port = new ToolboxWedSocketClient().GetportNumber();

     

            this._lastErrorMsg = string.Empty;
      try
      {
        MainForm.LoadNativeComponents();
        this.StopListening(CloseStatusCode.Normal, "Restart");
        if (WebSocketSettings.Default.SecureWebSocket)
          CryptoEngine.AddSslRootCert(string.Format("{0}\\{1}", (object) FileSystem.ModuleFolderPath, (object) "SslCert\\cert.cer"));
        WebSocketServer webSocketServer;
        if (!WebSocketSettings.Default.SecureWebSocket)
        {
          webSocketServer = new WebSocketServer(port);
        }
        else
        {
          webSocketServer = new WebSocketServer(port, true);
          webSocketServer.SslConfiguration.ServerCertificate = new X509Certificate2(string.Format("{0}\\{1}", (object) FileSystem.ModuleFolderPath, (object) "SslCert\\6da606ed-ab24-4d2a-9069-97fb8bd62497.pfx"), "enbkbnF");
          webSocketServer.SslConfiguration.EnabledSslProtocols = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12;
        }
        this._server = webSocketServer;
        this._server.AddWebSocketService<AstralWebSocketBehavior>("/RegistrationOffice");
        this._server.Start();
        if (WebSocketSettings.Default.ShowInTray)
        {
          this.ShowBalloonTip(string.Format("Сервис запущен на порте {0}.", (object) port ), ToolTipIcon.Info);
        }
        else
        {
          int num = (int) MessageBox.Show(string.Format("Сервис 1Ctoolbox запущен на порте {0}.", (object) port), "Сообщение", MessageBoxButtons.OK);
        }
      }
      catch (Exception ex)
      {
        this._lastErrorMsg = string.Format("Не удалось запустить сервис. {0}", (object) ex.Message);
        if (WebSocketSettings.Default.ShowInTray)
        {
          this.ShowBalloonTip(this._lastErrorMsg, ToolTipIcon.Error);
        }
        else
        {
          this._lastErrorMsg = string.Format("Не удалось запустить сервис 1Ctoolbox. {0}", (object) ex.Message);
          int num = (int) MessageBox.Show(string.Format(this._lastErrorMsg, (object) "Ошибка", (object) MessageBoxButtons.OK));
        }
      }
    }

    private static void LoadNativeComponents()
    {
      if (!DllLoader.LoadLibrary(string.Format("{0}\\{1}", (object) FileSystem.ModuleFolderPath, (object) "atcl.dll")) || !DllLoader.LoadLibrary(string.Format("{0}\\{1}", (object) FileSystem.ModuleFolderPath, (object) "jcrypt.dll")))
        throw new Exception(Resources.CanNotLoadNativeCompnents);
    }

    private void notifyIcon_Click(object sender, EventArgs e)
    {
      if ((e as MouseEventArgs).Button != MouseButtons.Left)
        return;
      if (!string.IsNullOrEmpty(this._lastErrorMsg))
        this.ShowBalloonTip(this._lastErrorMsg, ToolTipIcon.Error);
      else if (this._server.IsListening)
        this.ShowBalloonTip(string.Format("Сервис запущен на порте {0}.", (object) WebSocketSettings.Default.Port), ToolTipIcon.Info);
      else
        this.ShowBalloonTip("Сервис остановлен.", ToolTipIcon.Info);
    }

    private void MainForm_FormClosed(object sender, FormClosedEventArgs e) => this.StopListening(CloseStatusCode.Normal, "AppClose");

    private void menuItemExit_Click(object sender, EventArgs e) => Application.Exit();

    private void menuItemStop_Click(object sender, EventArgs e)
    {
      this.StopListening(CloseStatusCode.Normal, "UserRequests");
      this.UpdateControlsState();
    }

    private void menuItemRun_Click(object sender, EventArgs e)
    {
      this.StartListening();
      this.UpdateControlsState();
    }

    private void UpdateSecureWebSocketFlag(bool secure)
    {
      WebSocketSettings.Default["SecureWebSocket"] = (object) secure;
      WebSocketSettings.Default.Save();
      if (this._server != null && this._server.IsListening)
        this.StartListening();
      this.UpdateControlsState();
    }

    private void menuItemHttp_Click(object sender, EventArgs e) => this.UpdateSecureWebSocketFlag(false);

    private void menuItemHttps_Click(object sender, EventArgs e) => this.UpdateSecureWebSocketFlag(true);

    private void menuItemHide_Click(object sender, EventArgs e) => this.notifyIcon.Visible = false;

    private void ShowInTrayFlag(bool show)
    {
      WebSocketSettings.Default["ShowInTray"] = (object) show;
      WebSocketSettings.Default.Save();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MainForm));
      this.notifyIcon = new NotifyIcon(this.components);
      this.trayMenu = new ContextMenuStrip(this.components);
      this.menuItemMode = new ToolStripMenuItem();
      this.menuItemHttp = new ToolStripMenuItem();
      this.menuItemHttps = new ToolStripMenuItem();
      this.menuItemRun = new ToolStripMenuItem();
      this.menuItemStop = new ToolStripMenuItem();
      this.menuItemHide = new ToolStripMenuItem();
      this.menuItemExit = new ToolStripMenuItem();
      this.trayMenu.SuspendLayout();
      this.SuspendLayout();
      this.notifyIcon.ContextMenuStrip = this.trayMenu;
      this.notifyIcon.Icon = (Icon) componentResourceManager.GetObject("notifyIcon.Icon");
      this.notifyIcon.Text = "1Ctoolbox";
      this.notifyIcon.Visible = true;
      this.notifyIcon.Click += new EventHandler(this.notifyIcon_Click);
      this.trayMenu.Items.AddRange(new ToolStripItem[5]
      {
        (ToolStripItem) this.menuItemMode,
        (ToolStripItem) this.menuItemRun,
        (ToolStripItem) this.menuItemStop,
        (ToolStripItem) this.menuItemHide,
        (ToolStripItem) this.menuItemExit
      });
      this.trayMenu.Name = "trayMenu";
      this.trayMenu.Size = new Size(139, 114);
      this.menuItemMode.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.menuItemHttp,
        (ToolStripItem) this.menuItemHttps
      });
      this.menuItemMode.Name = "menuItemMode";
      this.menuItemMode.Size = new Size(138, 22);
      this.menuItemMode.Text = "Режим";
      this.menuItemMode.Visible = false;
      this.menuItemHttp.Name = "menuItemHttp";
      this.menuItemHttp.Size = new Size(101, 22);
      this.menuItemHttp.Text = "http";
      this.menuItemHttp.Click += new EventHandler(this.menuItemHttp_Click);
      this.menuItemHttps.Name = "menuItemHttps";
      this.menuItemHttps.Size = new Size(101, 22);
      this.menuItemHttps.Text = "https";
      this.menuItemHttps.Click += new EventHandler(this.menuItemHttps_Click);
      this.menuItemRun.Name = "menuItemRun";
      this.menuItemRun.Size = new Size(138, 22);
      this.menuItemRun.Text = "Запустить";
      this.menuItemRun.Click += new EventHandler(this.menuItemRun_Click);
      this.menuItemStop.Name = "menuItemStop";
      this.menuItemStop.Size = new Size(138, 22);
      this.menuItemStop.Text = "Остановить";
      this.menuItemStop.Visible = false;
      this.menuItemStop.Click += new EventHandler(this.menuItemStop_Click);
      this.menuItemHide.Name = "menuItemHide";
      this.menuItemHide.Size = new Size(138, 22);
      this.menuItemHide.Text = "Скрыть";
      this.menuItemHide.Click += new EventHandler(this.menuItemHide_Click);
      this.menuItemExit.Name = "menuItemExit";
      this.menuItemExit.Size = new Size(138, 22);
      this.menuItemExit.Text = "Выход";
      this.menuItemExit.Click += new EventHandler(this.menuItemExit_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(292, 273);
      this.Name = nameof (MainForm);
      this.ShowInTaskbar = false;
      this.Text = "Form1";
      this.WindowState = FormWindowState.Minimized;
      this.FormClosed += new FormClosedEventHandler(this.MainForm_FormClosed);
      this.Load += new EventHandler(this.MainForm_Load);
      this.trayMenu.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
