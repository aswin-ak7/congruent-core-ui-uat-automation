using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Playwright;

//using TechTalk.SpecFlow;
using Reqnroll;
using Helpers.Hooks;
using Helpers.support;

using AventStack.ExtentReports;

namespace PlanOnBoarding.Hooks
{
    [Binding]
    [TestFixture]
    public class OnboardingHooks
    {
        public IPage _page { get; set; }


        public GenerateHook _hooks;

        private readonly Reqnroll.ScenarioContext _scenarioContext;
        private ExtentTest? _currentScenario;
        private ExtentTest? _currentStep;
        //private ExtentT
        //public string _ssn;
        //public string _name;


        public OnboardingHooks(GenerateHook hooks, Reqnroll.ScenarioContext scenarioContext)
        {
            this._hooks = hooks;
            _page = hooks._page;
            _scenarioContext = scenarioContext;
            //_ssn=hooks._ssn;
            //_name=hooks._name;


        }



        [BeforeTestRun]
        public static async Task getAppSettings()
        {
            await GenerateHook.GetAppSetting();
        }

        [BeforeScenario]
        public async Task BrowserAsync(Reqnroll.ScenarioContext tags)
        {
            await _hooks.BrowserInitializer(tags);

            // Create ExtentTest node for this scenario
            //try
            //{
            //    string featureName = _scenarioContext.ScenarioInfo.Title;
            //    string scenarioName = _scenarioContext.ScenarioInfo.Title;

            //    _currentScenario = ExtentReportManager.CreateTest(featureName);

            //    if (_scenarioContext.ScenarioInfo.Tags.Length > 0)
            //    {
            //        foreach (var tag in _scenarioContext.ScenarioInfo.Tags)
            //        {
            //            _currentScenario.AssignCategory(tag);
            //        }
            //    }

            //    _scenarioContext["ExtentTest"] = _currentScenario;
            //    ExtentReportManager.LogInfo($"Starting scenario: {scenarioName}");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error creating ExtentReports scenario: {ex.Message}");
            //}
        }

        [AfterStep]
        public async Task AfterStep()
        {
            try
            {
                var stepType = _scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
                var stepName = _scenarioContext.StepContext.StepInfo.Text;
                var fullStepName = $"{stepType} {stepName}";
            }

            //    if (_scenarioContext.ContainsKey("ExtentTest"))
            //    {
            //        _currentScenario = _scenarioContext["ExtentTest"] as ExtentTest;
            //    }

            //    if (_scenarioContext.TestError == null)
            //    {
            //        ExtentReportManager.LogPass(fullStepName);
            //    }
            //    else
            //    {
            //        ExtentReportManager.LogFail($"{fullStepName} - FAILED");
            //        ExtentReportManager.LogFail($"Error: {_scenarioContext.TestError.Message}");

            //        if (_page != null)
            //        {
            //            //try
            //            //{
            //            //   // string screenshotBase64 = await ScreenshotHelper.CaptureScreenshotBase64Async(_page);
            //            //    if (!string.IsNullOrEmpty(screenshotBase64))
            //            //    {
            //            //        ExtentReportManager.AddScreenshotBase64(screenshotBase64, "Failure Screenshot");
            //            //    }
            //            //}
            //            catch (Exception ex)
            //            {
            //                Console.WriteLine($"Error capturing screenshot: {ex.Message}");
            //            }
            //        }

            //        if (_scenarioContext.TestError.StackTrace != null)
            //        {
            //            ExtentReportManager.LogFail($"<pre>{_scenarioContext.TestError.StackTrace}</pre>");
            //        }
            //    }
            //}
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AfterStep ExtentReports logging: {ex.Message}");
            }

        }

        [AfterScenario]
        public async Task ContextCloseAsync()
        {
            try
            {
                //if (_currentScenario != null)
                //{
                //    if (_scenarioContext.TestError != null)
                //    {
                //        ExtentReportManager.LogFail($"Scenario failed: {_scenarioContext.TestError.Message}");

                //        if (_page != null)
                //        {
                //            string screenshotBase64 = await ScreenshotHelper.CaptureScreenshotBase64Async(_page);
                //            if (!string.IsNullOrEmpty(screenshotBase64))
                //            {
                //                ExtentReportManager.AddScreenshotBase64(screenshotBase64, "Final Screenshot");
                //            }
                //        }
                //    }
                //    else
                //    {
                //        ExtentReportManager.LogPass("Scenario completed successfully");
                //    }
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AfterScenario ExtentReports logging: {ex.Message}");
            }

            await _hooks.ContextCloseAsync();
        }

        [AfterTestRun]
        public static async Task BrowserCloseAsync()
        {
           

            await GenerateHook.BrowserCloseAsync();
        }


    }
}
  
