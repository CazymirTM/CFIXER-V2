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
                { "Network Fixes", RunNetworkFixesAsync },
                { "Windows Update Reset", RunWindowsUpdateResetAsync },
                { "Firewall Reset", RunFirewallResetAsync }
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
        private async Task RunSFCAsync()
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string logFile = Path.Combine(desktopPath, "SFCScan_Log.txt");

            lblLog.Invoke((Action)(() =>
                lblLog.Text = "Running System File Checker. This can take a while..."));

            await Task.Run(() =>
            {
                try
                {
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = "/c sfc /scannow",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    using (var process = new Process { StartInfo = startInfo })
                    {
                        var outputBuilder = new StringBuilder();

                        process.Start();

                        outputBuilder.AppendLine(process.StandardOutput.ReadToEnd());
                        outputBuilder.AppendLine(process.StandardError.ReadToEnd());

                        process.WaitForExit();

                        var (statusMessage, logSummary) = InterpretSfcExitCode(process.ExitCode);

                        string logEntry =
                            $"{DateTime.Now}: SFC /scannow exited with code {process.ExitCode}. {logSummary}\n" +
                            outputBuilder.ToString();

                        File.AppendAllText(logFile, logEntry + "\n------------------------------\n");

                        lblLog.Invoke((Action)(() => lblLog.Text = statusMessage));
                    }
                }
                catch (Exception ex)
                {
                    lblLog.Invoke((Action)(() =>
                    {
                        lblLog.Text = $"Failed to run SFC scan: {ex.Message}";
                    }));

                    try
                    {
                        File.AppendAllText(logFile, $"{DateTime.Now}: Failed to run SFC scan - {ex}\n");
                    }
                    catch
                    {
                        // Ignored - if we cannot log, there's nothing else to do.
                    }
                }
            });
        }

        private (string statusMessage, string logSummary) InterpretSfcExitCode(int exitCode)
        {
            switch (exitCode)
            {
                case 0:
                    return ("SFC scan completed successfully.", "No integrity violations were found.");
                case 1:
                    return ("SFC scan completed and repaired system files. A reboot may be required.",
                        "Integrity violations were found and repaired.");
                case 2:
                    return ("SFC scan found issues that require a reboot or additional actions. See the log on your desktop.",
                        "Integrity violations were found but some could not be repaired. A reboot may be pending.");
                default:
                    return ($"SFC scan failed with exit code {exitCode}. Check the log on your desktop for details.",
                        "The scan did not complete successfully.");
            }
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
        private Task RunNetworkFixesAsync()
        {
            var commands = new string[]
            {
                "ipconfig /flushdns",
                "netsh winsock reset",
                "netsh int ip reset"
            };

            return RunCommandSequenceAsync("Network Fixes", commands, "NetworkFixes_Log.txt");
        }

        private Task RunWindowsUpdateResetAsync()
        {
            var commands = new string[]
            {
                "net stop wuauserv",
                "net stop cryptSvc",
                "net stop bits",
                "net stop msiserver",
                "if exist \"%WINDIR%\\SoftwareDistribution\" ren \"%WINDIR%\\SoftwareDistribution\" SoftwareDistribution.old",
                "if exist \"%WINDIR%\\System32\\catroot2\" ren \"%WINDIR%\\System32\\catroot2\" catroot2.old",
                "net start wuauserv",
                "net start cryptSvc",
                "net start bits",
                "net start msiserver"
            };

            return RunCommandSequenceAsync("Windows Update Reset", commands, "WindowsUpdateReset_Log.txt");
        }

        private Task RunFirewallResetAsync()
        {
            var commands = new string[]
            {
                "netsh advfirewall reset",
                "netsh advfirewall set allprofiles state on"
            };

            return RunCommandSequenceAsync("Firewall Reset", commands, "FirewallReset_Log.txt");
        }

        private Task RunCommandSequenceAsync(string fixName, IEnumerable<string> commands, string logFileName)
        {
            return Task.Run(() =>
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string logFile = Path.Combine(desktopPath, logFileName);
                var logDirectory = Path.GetDirectoryName(logFile);
                if (!string.IsNullOrEmpty(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                foreach (var cmd in commands)
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
                                RedirectStandardError = true,
                                UseShellExecute = false,
                                CreateNoWindow = true
                            }
                        };

                        process.Start();
                        string output = process.StandardOutput.ReadToEnd();
                        string error = process.StandardError.ReadToEnd();
                        process.WaitForExit();

                        var logEntry = new StringBuilder()
                            .AppendLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {fixName}")
                            .AppendLine($"Command: {cmd}")
                            .AppendLine("Output:");

                        logEntry.AppendLine(string.IsNullOrWhiteSpace(output) ? "(no output)" : output.TrimEnd());

                        if (!string.IsNullOrWhiteSpace(error))
                        {
                            logEntry.AppendLine("Errors:");
                            logEntry.AppendLine(error.TrimEnd());
                        }

                        logEntry
                            .AppendLine(new string('-', 60))
                            .AppendLine();

                        File.AppendAllText(logFile, logEntry.ToString());
                    }
                    catch (Exception ex)
                    {
                        UpdateLogSafe($"Error running '{fixName}' command '{cmd}': {ex.Message}");
                    }
                }
            });
        }

        private void UpdateLogSafe(string message)
        {
            if (lblLog.InvokeRequired)
            {
                lblLog.Invoke((Action)(() => lblLog.Text = message));
            }
            else
            {
                lblLog.Text = message;
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
                    lblLog.Text = fixName == "DISM Checks"
                        ? $"{fixName} will run in a separate window."
                        : $"Applying {fixName}..."));

                progressBarFixes.Value = 0;

                await fixes[fixName]();

                progressBarFixes.Value = 100;
                lblLog.Invoke((Action)(() =>
                    lblLog.Text = fixName == "DISM Checks"
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
                        lblLog.Text = fixName == "DISM Checks"
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