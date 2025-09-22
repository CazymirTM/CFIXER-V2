using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CFIXERV2
{
    public partial class MainForm : Form
    {
        private Dictionary<string, Func<Task>> fixes;

        public MainForm()
        {
            InitializeComponent();
            InitializeFixes();

            // Admin check
            if (!IsRunningAsAdmin())
            {
                MessageBox.Show(
                    "Please run this application as Administrator to allow fixes to work properly.",
                    "Admin Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }

            progressBarFixes.Minimum = 0;
            progressBarFixes.Maximum = 100;
            progressBarFixes.Value = 0;
        }

        private void InitializeFixes()
        {
            fixes = new Dictionary<string, Func<Task>>
            {
                { "SFC Scan", RunSFCAsync },
                { "DISM Checks", RunDISMAsync },
                { "Network Fixes", RunNetworkFixesAsync }
            };

            checkedListBox1.Items.Clear();
            foreach (var fixName in fixes.Keys)
            {
                checkedListBox1.Items.Add(fixName);
            }

            lblLog.Text = "Select a fix and click Apply.";
        }

        private bool IsRunningAsAdmin()
        {
            var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            var principal = new System.Security.Principal.WindowsPrincipal(identity);
            return principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
        }

        // ------------------------------
        // SFC Scan - instruct user to run manually
        // ------------------------------
        private Task RunSFCAsync()
        {
            MessageBox.Show(
                "SFC Scan must be run manually in a separate elevated Command Prompt.\n\n" +
                "Open CMD as Administrator and run:\n\nsfc /scannow\n\n" +
                "Then wait until it finishes.",
                "SFC Scan Required",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            return Task.CompletedTask;
        }

        // ------------------------------
        // DISM - runs in separate elevated CMD window
        // ------------------------------
        private Task RunDISMAsync()
        {
            try
            {
                MessageBox.Show(
                    "DISM will run in a separate elevated Command Prompt window.\n" +
                    "Please wait until it completes.",
                    "DISM Checks",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                Process.Start(new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/k DISM /Online /Cleanup-Image /RestoreHealth",
                    UseShellExecute = true,  // elevation
                    Verb = "runas",
                    CreateNoWindow = false
                });
            }
            catch (Exception ex)
            {
                lblLog.Invoke((Action)(() => lblLog.Text = $"Failed to run DISM: {ex.Message}"));
            }

            return Task.CompletedTask;
        }

        // ------------------------------
        // Network Fixes - fully automated
        // ------------------------------
        private async Task RunNetworkFixesAsync()
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string logFile = Path.Combine(desktopPath, "NetworkFixes_Log.txt");

            var commands = new string[]
            {
                "ipconfig /flushdns",
                "netsh winsock reset",
                "netsh int ip reset"
            };

            foreach (var cmd in commands)
            {
                await Task.Run(() =>
                {
                    try
                    {
                        var process = new Process
                        {
                            StartInfo = new ProcessStartInfo
                            {
                                FileName = "cmd.exe",
                                Arguments = "/c " + cmd,
                                RedirectStandardOutput = true,
                                UseShellExecute = false,
                                CreateNoWindow = true
                            }
                        };

                        process.Start();
                        string output = process.StandardOutput.ReadToEnd();
                        process.WaitForExit();

                        File.AppendAllText(logFile, $"{DateTime.Now}: {cmd}\n{output}\n\n");
                    }
                    catch (Exception ex)
                    {
                        lblLog.Invoke((Action)(() => lblLog.Text = $"Error running '{cmd}': {ex.Message}"));
                    }
                });
            }
        }

        // ------------------------------
        // Apply single fix
        // ------------------------------
        private async void BtnApply_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.SelectedItem == null)
            {
                MessageBox.Show("Please select a fix to apply.");
                return;
            }

            string fixName = checkedListBox1.SelectedItem.ToString();
            if (fixes.ContainsKey(fixName))
            {
                lblLog.Invoke((Action)(() =>
                    lblLog.Text = fixName == "SFC Scan" || fixName == "DISM Checks"
                        ? $"{fixName} will run in a separate window."
                        : $"Applying {fixName}..."));

                progressBarFixes.Value = 0;

                await fixes[fixName]();

                progressBarFixes.Value = 100;
                lblLog.Invoke((Action)(() =>
                    lblLog.Text = fixName == "SFC Scan" || fixName == "DISM Checks"
                        ? $"{fixName} launched. Follow instructions in the window."
                        : $"{fixName} applied successfully!"));
            }
        }

        // ------------------------------
        // Apply all checked fixes
        // ------------------------------
        private async void BtnApplyAll_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.CheckedItems.Count == 0)
            {
                MessageBox.Show("Please check at least one fix to apply.");
                return;
            }

            int total = checkedListBox1.CheckedItems.Count;
            int completed = 0;
            progressBarFixes.Value = 0;

            foreach (var item in checkedListBox1.CheckedItems)
            {
                string fixName = item.ToString();
                if (fixes.ContainsKey(fixName))
                {
                    lblLog.Invoke((Action)(() =>
                        lblLog.Text = fixName == "SFC Scan" || fixName == "DISM Checks"
                            ? $"{fixName} will run in a separate window."
                            : $"Applying {fixName}..."));

                    await fixes[fixName]();

                    completed++;
                    int percent = (int)((completed / (float)total) * 100);
                    progressBarFixes.Invoke((Action)(() => progressBarFixes.Value = percent));
                }
            }

            lblLog.Invoke((Action)(() => lblLog.Text = "All selected fixes applied!"));
        }

        private void BtnExitApp_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnContact_Click(object sender, EventArgs e)
        {
            try
            {
                // Replace this with your actual Discord profile link
                string discordUrl = "https://discord.com/users/493745450776526849";
                Process.Start(new ProcessStartInfo
                {
                    FileName = discordUrl,
                    UseShellExecute = true // ensures it opens in the default browser
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open Discord link: {ex.Message}");
            }
        }
    }
}