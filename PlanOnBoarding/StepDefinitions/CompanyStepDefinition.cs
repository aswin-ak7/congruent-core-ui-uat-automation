using PlanOnBoarding.Page;
using Io.Cucumber.Messages.Types;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using PlanOnBoarding.Page;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanOnBoarding.StepDefinitions
{
    [Binding]

    public class CompanyStepDefinition
    {
        private readonly SaveIncorporationDetails _saveincorporationdetails;

        public CompanyStepDefinition(SaveIncorporationDetails saveincorporationdetails)
        {
            _saveincorporationdetails = saveincorporationdetails;
        }


        [Given("Create a new company with all required details")]
        public async Task GivenCreateANewCompanyWithAllRequiredDetails()
        {
            //await PlaywrightDriver.EnsureInitializedAsync();
            //var saveincorporationdetails = new SaveIncorporationDetails(() => PlaywrightDriver.Page ?? throw new InvalidOperationException("Playwright page not initialized. Ensure BeforeScenario hook ran."));
           
        }

        

       

       

        [Given("Create a new company with all required detailss")]
        public async Task GivenCreateANewCompanyWithAllRequiredDetailss()
        {
            await _saveincorporationdetails.CreateCompanyDetailsPage();
        }




    }
}
