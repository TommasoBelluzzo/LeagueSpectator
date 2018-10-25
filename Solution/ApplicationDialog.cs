#region Using Directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Win32;
using LeagueSpectator.Properties;
#endregion

namespace LeagueSpectator
{
    [SuppressMessage("ReSharper", "PrivateFieldCanBeConvertedToLocalVariable")]
    public sealed class ApplicationDialog : Form
    {
        #region Members
        private readonly Button m_ButtonSelect;
        private readonly Button m_ButtonSpectate;
        private readonly ComboBox m_ComboBoxGameRegion;
        private readonly Container m_Components;
        private readonly Label m_LabelGameClient;
        private readonly Label m_LabelGameRegion;
        private readonly Label m_LabelSummonerName;
        private readonly Messenger m_Messenger;
        private readonly OpenFileDialog m_OpenFileDialog;
        private readonly TextBox m_TextBoxGameClient;
        private readonly TextBox m_TextBoxSummonerName;
        #endregion

        #region Members (Static)
        private static readonly dynamic[] s_GameExecutableRegistryEntries =
        {
            new { Hive = RegistryHive.CurrentUser, View = RegistryView.Registry32, Key = @"SOFTWARE\Riot Games\RADS", Value = "LocalRootFolder", AppendRADS = false },
            new { Hive = RegistryHive.CurrentUser, View = RegistryView.Registry32, Key = @"SOFTWARE\Riot Games, Inc\League of Legends", Value = "Location", AppendRADS = true },
            new { Hive = RegistryHive.LocalMachine, View = RegistryView.Registry32, Key = @"SOFTWARE\Riot Games\RADS", Value = "LocalRootFolder", AppendRADS = false },
            new { Hive = RegistryHive.LocalMachine, View = RegistryView.Registry32, Key = @"SOFTWARE\Riot Games, Inc\League of Legends", Value = "Location", AppendRADS = true }
        };

        private static readonly Regex s_RegexSummonerName = new Regex(@"^[\d\p{L}.\\_ ]{3,16}$", RegexOptions.Compiled);
        #endregion

        #region Constructors
        public ApplicationDialog()
        {
            m_ButtonSelect = new Button();
            m_ButtonSpectate = new Button();
            m_ComboBoxGameRegion = new ComboBox();
            m_Components = new Container();
            m_LabelGameClient = new Label();
            m_LabelGameRegion = new Label();
            m_LabelSummonerName = new Label();
            m_Messenger = new Messenger(m_Components);
            m_OpenFileDialog = new OpenFileDialog();
            m_TextBoxGameClient = new TextBox();
            m_TextBoxSummonerName = new TextBox();

            SuspendLayout();

            m_ButtonSelect.Font = new Font("Microsoft Sans Serif", 12.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            m_ButtonSelect.Location = new Point(314, 23);
            m_ButtonSelect.Name = "ButtonSelectClient";
            m_ButtonSelect.Size = new Size(98, 28);
            m_ButtonSelect.TabIndex = 2;
            m_ButtonSelect.Text = Resources.TextSelect;
            m_ButtonSelect.UseVisualStyleBackColor = true;
            m_ButtonSelect.Click += ButtonSelectClick;

            m_ButtonSpectate.Enabled = false;
            m_ButtonSpectate.Font = new Font("Microsoft Sans Serif", 14.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            m_ButtonSpectate.Location = new Point(145, 109);
            m_ButtonSpectate.Name = "ButtonSpectate";
            m_ButtonSpectate.Size = new Size(133, 37);
            m_ButtonSpectate.TabIndex = 7;
            m_ButtonSpectate.Text = Resources.TextSpectate;
            m_ButtonSpectate.UseVisualStyleBackColor = true;
            m_ButtonSpectate.Click += ButtonSpectateClick;

            m_ComboBoxGameRegion.DataSource = GameRegion.GetList();
            m_ComboBoxGameRegion.DisplayMember = "Name";
            m_ComboBoxGameRegion.DropDownStyle = ComboBoxStyle.DropDownList;
            m_ComboBoxGameRegion.Font = new Font("Microsoft Sans Serif", 11.5f, FontStyle.Regular, GraphicsUnit.Point, 0);
            m_ComboBoxGameRegion.FormattingEnabled = true;
            m_ComboBoxGameRegion.Location = new Point(225, 68);
            m_ComboBoxGameRegion.MaxDropDownItems = 15;
            m_ComboBoxGameRegion.Name = "ComboBoxGameRegions";
            m_ComboBoxGameRegion.Size = new Size(186, 26);
            m_ComboBoxGameRegion.TabIndex = 6;
            m_ComboBoxGameRegion.ValueMember = "Code";
            m_ComboBoxGameRegion.SelectedIndexChanged += ComboBoxGameRegionsSelectedIndexChanged;

            m_LabelGameClient.AutoSize = true;
            m_LabelGameClient.Font = new Font("Microsoft Sans Serif", 9.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            m_LabelGameClient.Location = new Point(10, 8);
            m_LabelGameClient.Name = "LabelGameClient";
            m_LabelGameClient.Size = new Size(106, 15);
            m_LabelGameClient.TabIndex = 0;
            m_LabelGameClient.Text = Resources.TextGameClient;

            m_LabelGameRegion.AutoSize = true;
            m_LabelGameRegion.Font = new Font("Microsoft Sans Serif", 9.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            m_LabelGameRegion.Location = new Point(224, 52);
            m_LabelGameRegion.Name = "LabelGameClient";
            m_LabelGameRegion.Size = new Size(106, 15);
            m_LabelGameRegion.TabIndex = 5;
            m_LabelGameRegion.Text = Resources.TextGameRegion;

            m_LabelSummonerName.AutoSize = true;
            m_LabelSummonerName.Font = new Font("Microsoft Sans Serif", 9.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            m_LabelSummonerName.Location = new Point(10, 52);
            m_LabelSummonerName.Name = "LabelGameClient";
            m_LabelSummonerName.Size = new Size(106, 15);
            m_LabelSummonerName.TabIndex = 3;
            m_LabelSummonerName.Text = Resources.TextSummonerName;

            m_Messenger.ContainerControl = this;

            m_OpenFileDialog.DefaultExt = ".exe";
            m_OpenFileDialog.InitialDirectory = Application.StartupPath;
            m_OpenFileDialog.Filter = Resources.FilterGameClient;
            m_OpenFileDialog.RestoreDirectory = true;

            m_TextBoxGameClient.Font = new Font("Microsoft Sans Serif", 12.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            m_TextBoxGameClient.Location = new Point(12, 24);
            m_TextBoxGameClient.MaxLength = 32767;
            m_TextBoxGameClient.Name = "TextBoxClientPath";
            m_TextBoxGameClient.ReadOnly = true;
            m_TextBoxGameClient.Text = GetGameExecutable();
            m_TextBoxGameClient.Size = new Size(297, 26);
            m_TextBoxGameClient.TabIndex = 1;

            m_TextBoxSummonerName.Font = new Font("Microsoft Sans Serif", 12.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            m_TextBoxSummonerName.Location = new Point(12, 68);
            m_TextBoxSummonerName.MaxLength = 16;
            m_TextBoxSummonerName.Name = "TextBoxSummonerName";
            m_TextBoxSummonerName.Size = new Size(207, 26);
            m_TextBoxSummonerName.TabIndex = 4;
            m_TextBoxSummonerName.TextChanged += TextBoxSummonerNameTextChanged;

            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(423, 157);
            Controls.Add(m_LabelGameClient);
            Controls.Add(m_TextBoxGameClient);
            Controls.Add(m_ButtonSelect);
            Controls.Add(m_LabelSummonerName);
            Controls.Add(m_TextBoxSummonerName);
            Controls.Add(m_LabelGameRegion);
            Controls.Add(m_ComboBoxGameRegion);
            Controls.Add(m_ButtonSpectate);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = Program.Icon;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ApplicationDialog";
            StartPosition = FormStartPosition.CenterScreen;
            Text = String.Concat(Resources.TextTitle, " ", Program.Version);

            ResumeLayout(true);
        }
        #endregion

        #region Events
        private void ButtonSelectClick(Object sender, EventArgs e)
        {
            m_ButtonSelect.Enabled = false;

            if (!m_Messenger.DisplayDialog(m_OpenFileDialog))
            {
                m_ButtonSelect.Enabled = true;
                return;
            }

            m_TextBoxGameClient.Text = m_OpenFileDialog.FileName;
            m_OpenFileDialog.InitialDirectory = Path.GetDirectoryName(m_OpenFileDialog.FileName);

            m_ButtonSelect.Enabled = true;

            ValidateData();
        }

        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        private async void ButtonSpectateClick(Object sender, EventArgs e)
        {
            DisableInterface();

            GameRegion gameRegion = (GameRegion)m_ComboBoxGameRegion.SelectedItem;
            Int64 summonerId = await GameAPI.GetSummonerId(gameRegion.EndPoint, m_TextBoxSummonerName.Text);

            if (summonerId == -1)
            {
                m_Messenger.DisplayError("ERROR BLABLA");
                EnableInterface();

                return;
            }

            GameInfo gameInfo = await GameAPI.GetGameInfo(gameRegion.EndPoint, summonerId);

            if (gameInfo == null)
            {
                m_Messenger.DisplayError("ERROR BLABLA");
                EnableInterface();

                return;
            }

            String spectatorData = $"spectator spectator.{gameRegion.SpectatorEndPoint}.lol.riotgames.com:{gameRegion.SpectatorPort} {gameInfo.Observer.EncryptionKey} {gameInfo.Id} {gameInfo.Platform}";
            
            ProcessStartInfo info = new ProcessStartInfo
            {
                Arguments = $"\"8394\" \"LeagueClient.exe\" \"/path\" \"{spectatorData}\"",
                FileName = m_TextBoxGameClient.Text,
                WorkingDirectory = Path.GetDirectoryName(m_TextBoxGameClient.Text)
            };

            Process p = new Process
            {
                EnableRaisingEvents = true,
                StartInfo = info
            };

            p.Exited += GameClientExited;
            p.Start();

            Hide();
        }

        private void ComboBoxGameRegionsSelectedIndexChanged(Object sender, EventArgs e)
        {
            ValidateData();
        }

        private void GameClientExited(Object sender, EventArgs e)
        {
            if (InvokeRequired)
                Invoke((Action)(() =>
                {
                    EnableInterface();
                    Show();
                    WindowState = FormWindowState.Normal;
                }));
            else
            {
                EnableInterface();
                Show();
                WindowState = FormWindowState.Normal;
            }
        }

        private void TextBoxSummonerNameTextChanged(Object sender, EventArgs e)
        {
            ValidateData();
        }
        #endregion

        #region Methods
        private void DisableInterface()
        {
            m_ButtonSpectate.Enabled = false;

            m_TextBoxGameClient.Enabled = false;
            m_ButtonSelect.Enabled = false;

            m_TextBoxSummonerName.Enabled = false;
            m_ComboBoxGameRegion.Enabled = false;
        }

        private void EnableInterface()
        {
            m_TextBoxGameClient.Enabled = true;
            m_ButtonSelect.Enabled = true;

            m_TextBoxSummonerName.Enabled = true;
            m_ComboBoxGameRegion.Enabled = true;

            m_ButtonSpectate.Enabled = true;
        }

        private void ValidateData()
        {
            if (String.IsNullOrWhiteSpace(m_TextBoxGameClient.Text) || !File.Exists(m_TextBoxGameClient.Text))
            {
                m_ButtonSpectate.Enabled = false;
                return;
            }

            if (!s_RegexSummonerName.IsMatch(m_TextBoxSummonerName.Text))
            {
                m_ButtonSpectate.Enabled = false;
                return;
            }

            if (m_ComboBoxGameRegion.SelectedIndex == -1)
            {
                m_ButtonSpectate.Enabled = false;
                return;
            }

            m_ButtonSpectate.Enabled = true;
        }

        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
                m_Components?.Dispose();

            base.Dispose(disposing);
        }

        protected override void OnLoad(EventArgs e)
        {
            m_ComboBoxGameRegion.ResetText();
            m_ComboBoxGameRegion.SelectedIndex = -1;

            m_TextBoxSummonerName.Select();

            base.OnLoad(e);
        }
        #endregion

        #region Methods (Static)
        private static String GetGameExecutable()
        {
            String pathRADS = null;

            for (Int32 i = 0; i < s_GameExecutableRegistryEntries.Length; ++i)
            {
                dynamic entry = s_GameExecutableRegistryEntries[i];
                String registryValue = RegistryUtilities.GetValue(entry.Hive, entry.View, entry.Key, entry.Value);

                if (registryValue == null)
                    continue;

                if (entry.AppendRADS)
                    registryValue = Path.Combine(registryValue, "RADS");

                if (!Directory.Exists(registryValue))
                    continue;

                pathRADS = registryValue;

                break;
            }

            if (pathRADS == null)
                return String.Empty;

            String pathFinal = Path.Combine(pathRADS, @"solutions\lol_game_client_sln\releases");

            if (!Directory.Exists(pathFinal))
                return String.Empty;

            DirectoryInfo directoryFinal = new DirectoryInfo(pathFinal);
            DirectoryInfo[] directories = directoryFinal.GetDirectories();
            Int32 directoriesCount = directories.Length;

            List<Version> versions = new List<Version>(directoriesCount);

            for (Int32 i = 0; i < directoriesCount; ++i)
            {
                DirectoryInfo directory = directories[i];

                if (Version.TryParse(directory.Name, out Version version))
                    versions.Add(version);
            }

            if (versions.Count == 0)
                return String.Empty;

            String targetDeploy = versions.OrderByDescending(x => x).First().ToString();
            String pathExecutable = Path.Combine(pathFinal, targetDeploy, @"deploy\League of Legends.exe");

            if (!File.Exists(pathExecutable))
                return String.Empty;

            return pathExecutable;
        }
        #endregion
    }
}
