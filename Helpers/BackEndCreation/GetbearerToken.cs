
using Helpers;
using Helpers.support;
using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Configuration.Binder;
//using Microsoft.Extensions.Configuration.FileExtensions;
using CsvHelper;
using CsvHelper.Configuration;
//using Microsoft.IdentityModel.Tokens;
using Microsoft.Playwright;
//using TechTalk.SpecFlow.Assist;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;
using Reqnroll.TestFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Numerics;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
//using TechTalk.SpecFlow;
using static System.Net.Mime.MediaTypeNames;

namespace Helpers.Hook

{
    public class GetHelper
    {
        private static IBrowser? _browser;
        private static string? _bearerToken;
        private static AppSettings? _appSettings;
        private static readonly SemaphoreSlim _browserInitLock = new SemaphoreSlim(1, 1);
        private static bool _isInitialized = false;
        private static IPlaywright? playwright;
        public static IPage Page { get; set; } = null!;
        public static IPage _Page { get; set; } = null!;
       // public Bogus.Faker? faker = new Faker();
        Helpers.support.Utility _utility = new Helpers.support.Utility();
        public static IBrowserContext? context, context1;

        public static string? authCode;

        public string bearer = string.Empty;

        public static string GetHelpersPath()
        {
            var currentDir = AppDomain.CurrentDomain.BaseDirectory;

            while (!string.IsNullOrEmpty(currentDir))
            {
                var potentialPath = Path.Combine(currentDir, "Helpers");

                if (Directory.Exists(potentialPath))
                    return potentialPath;

                currentDir = Directory.GetParent(currentDir)?.FullName;
            }

            throw new DirectoryNotFoundException($"Helpers folder not found starting from {AppDomain.CurrentDomain.BaseDirectory}");
        }


        public static IBrowser? browser;
        public static async Task<IPage> BrowserLaunchAsync()
        {
            if (browser == null)
            {
                var playwright = await Playwright.CreateAsync();

                browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = _appSettings.Headless,
                    SlowMo = _appSettings.SlowMotion,
                    Args = _appSettings.Args
                });
            }

            context = await browser.NewContextAsync(new BrowserNewContextOptions
            {
                ViewportSize = null
            });

            Page = await context.NewPageAsync();

            await Page.GotoAsync(url!, new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle });
            await PerformLoginAsync(Page);

            return Page;
        }

        public static async Task<string> GetbearerToken()
        {
            _bearerToken = await GetToken(
               url!,
               "COREII",
               "postman-secret",
               $"{url!}/authentication/login-callback",
               name!,
               password!,
               enableDebug: true
           );

            return _bearerToken;
        }

        public static string BearerToken => _bearerToken!;

        private static async Task<string> GetToken(
     string baseUrl, string clientId, string clientSecret,
     string redirectUri, string username, string password,
     bool enableDebug = false)
        {
            void Log(string msg)
            {
                if (!enableDebug) return;
                string log = $"[{DateTime.Now:HH:mm:ss}] {msg}";
                Console.WriteLine(log);
                File.AppendAllText("oauth_debug.log", log + Environment.NewLine);
            }

            try
            {
                using var handler = new HttpClientHandler
                {
                    CookieContainer = new CookieContainer(),
                    UseCookies = true,
                    AllowAutoRedirect = true
                };

                using var client = new HttpClient(handler);
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0");

                // PKCE: generate verifier and challenge
                string codeVerifier = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
                    .TrimEnd('=').Replace('+', '-').Replace('/', '_');
                string codeChallenge = Convert.ToBase64String(
                    SHA256.HashData(Encoding.UTF8.GetBytes(codeVerifier)))
                    .TrimEnd('=').Replace('+', '-').Replace('/', '_');

                string state = Guid.NewGuid().ToString("N");
                string authorizeUrl = $"{baseUrl}/connect/authorize" +
                    $"?client_id={clientId}" +
                    $"&redirect_uri={Uri.EscapeDataString(redirectUri)}" +
                    $"&response_type=code" +
                    $"&scope=api openid profile roles offline_access" +
                    $"&state={state}" +
                    $"&code_challenge={codeChallenge}" +
                    $"&code_challenge_method=S256" +
                    $"&response_mode=query";

                Log("Fetching authorization page...");
                var authResp = await client.GetAsync(authorizeUrl);
                var authHtml = await authResp.Content.ReadAsStringAsync();

                var tokenMatch = Regex.Match(authHtml, @"__RequestVerificationToken.*?value=""([^""]+)""");
                if (!tokenMatch.Success) throw new Exception("Verification token not found in login page.");

                string verificationToken = tokenMatch.Groups[1].Value;

                // Submit login form
                var loginForm = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "Input.Email", username },
            { "Input.Password", password },
            { "__RequestVerificationToken", verificationToken }
        });

                Log("Submitting login form...");
                var loginResp = await client.PostAsync(authResp.RequestMessage.RequestUri.ToString(), loginForm);

                string redirectedUrl = loginResp.RequestMessage.RequestUri?.ToString() ?? "";
                var codeMatch = Regex.Match(redirectedUrl, @"code=([^&]+)");
                if (!codeMatch.Success) throw new Exception("Authorization code not found in redirect URL.");

                string authCode = codeMatch.Groups[1].Value;
                Log($"Received auth code: {authCode.Substring(0, 8)}...");

                // Exchange code for token
                var tokenForm = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "client_id", clientId },
            { "client_secret", clientSecret },
            { "grant_type", "authorization_code" },
            { "redirect_uri", redirectUri },
            { "code_verifier", codeVerifier },
            { "code", authCode }
        });

                Log("Exchanging code for token...");
                var tokenResp = await client.PostAsync($"{baseUrl}/connect/token", tokenForm);
                string tokenJson = await tokenResp.Content.ReadAsStringAsync();

                if (!tokenResp.IsSuccessStatusCode)
                {
                    Log($"Token error response: {tokenJson}");
                    throw new Exception($"Token request failed: {tokenResp.StatusCode}");
                }

                var doc = JsonDocument.Parse(tokenJson);
                return doc.RootElement.GetProperty("access_token").GetString()!;
            }
            catch (Exception ex)
            {
                Log($"ERROR: {ex.Message}");
                throw;
            }
        }


        private static void Log(string message, bool enabled)
        {
            if (!enabled) return;
            string log = $"[{DateTime.Now:HH:mm:ss}] {message}";
            Console.WriteLine(log);
            File.AppendAllText("oauth_debug.log", log + Environment.NewLine);
        }


        public static string? url, name, password;


        public static async Task getAppSettings()
        {
            // Build configuration from the Helpers folder (appsettings.json)
            var sharedPath = GetHelpersPath();
            var config = BuildConfig(sharedPath);

            var section = config.GetSection(AppSettings.Name);
            // bind section to AppSettings (requires Microsoft.Extensions.Configuration.Binder)
            _appSettings = section.Get<AppSettings>() ?? new AppSettings();

            url = _appSettings.ApplicationURL;
            name = _appSettings.UserName;
            password = _appSettings.Password;

            // initialize browser/page
            Page = await BrowserLaunchAsync();
        }


        private static async Task PerformLoginAsync(IPage page)
        {
            await page.FillAsync("#Input_Email", name!);
            await page.FillAsync("#passwordInput", password!);
            await page.ClickAsync("button[type='submit']");
            await Task.Delay(10000);
        }


        public async Task ContextClose()
        {
            _Page = await context!.NewPageAsync();
            await _Page.GotoAsync(url!);
            await Page.CloseAsync();
            Page = _Page;

        }

        public static async Task BrowserClose()
        {
            await browser!.CloseAsync();
        }


        public async Task BrowserAsync()
        {



        }

        public static IConfiguration BuildConfig(string sharedConfigPath)
        {
            // Avoid relying on SetBasePath extension (may require FileExtensions package).
            // Build the configuration by passing the full path to AddJsonFile instead.
            var jsonPath = Path.Combine(sharedConfigPath, "appsettings.json");
            var builder = new ConfigurationBuilder()
                .AddJsonFile(jsonPath, optional: false, reloadOnChange: true);

            return builder.Build();
        }
    }
}
