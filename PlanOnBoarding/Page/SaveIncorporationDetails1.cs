
using Helpers.Hook;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;


namespace PlanOnBoarding.Page
{
    public class SaveIncorporationDetails1
    {
        public IPage _page = GetHelper.Page;

        public SaveIncorporationDetails1(GetHelper helper)
        {
            

        }
        private ILocator PLanConfigIcon => _page.Locator("//span[text()='Plan Config']");
        private ILocator CompanyLink => _page.Locator("//div[text()='Company']");
        public ILocator Incorporationsavebtn => _page.Locator("//span[text()='Save']");
        private ILocator Nothanksbutton => _page.Locator("//button[@type = 'button']//span[text() = 'No Thanks']");
        private ILocator NewCompanyBtn => _page.GetByRole(AriaRole.Button, new() { Name = "New company" });

        // Access the current IPage via the factory



        public async Task CreateCompanyDetailsPage()
        {

            await PLanConfigIcon.ClickAsync();
            await CompanyLink.Nth(0).ClickAsync();
            await NewCompanyBtn.ClickAsync();




        }
    }
}
