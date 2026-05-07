using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Playwright;
using System.Data;
using Reqnroll;
using Helpers.support;
using static System.Net.Mime.MediaTypeNames;
using Helpers.support;
using Helpers.Hook;

namespace Helpers.Hooks
{
    [Binding]
    public class GenerateHook
    {
        public Utility _utility;        
       // public GetHelpers _hook;
        public GetHelper _hook;

        public IPage _page = GetHelper.Page;
        public string bearerToken;

        public GenerateHook(Utility utility, GetHelper hook)
        {
            _utility = utility;
            _hook = hook;
           
           // _planid = GetHelpers.planid;
        }

        public static async Task GetAppSetting()
        {
           // await GetHelpers.getAppSettings();
            await GetHelper.getAppSettings();
        }

        public async Task BrowserInitializer(Reqnroll.ScenarioContext tags)
        {
            await _hook.BrowserAsync();

            if (tags != null && tags.ScenarioInfo.Tags.Length > 0)
            {
                var tagList = tags.ScenarioInfo.Tags.Select(t => t.ToLowerInvariant()).ToList();
                if (!(tagList.All(t => t == "smoketest" || t == "UAT_Test")))
                {
                    bearerToken = await GetHelper.GetbearerToken();
                  //  await TagsCreation(tags);
                }
            }
        }

        public async Task ContextCloseAsync()
        {

            await _hook.ContextClose();
        }

        public static async Task BrowserCloseAsync()
        {
            await GetHelper.BrowserClose();
        }


    }
}
