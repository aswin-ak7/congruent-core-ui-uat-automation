using Onboarding.Page.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onboarding.StepDefinitions.Company
{
    [Binding]
    public class CompanyStepDefinition
    {
        private readonly SaveCompany _saveCompany;

        public CompanyStepDefinition(SaveCompany saveCompany)
        {
            _saveCompany = saveCompany;
        }

        [Given("Create and save company incorporation details with all mandatory fields provided")]
        public async Task GivenCreateAndSaveCompanyIncorporationDetailsWithAllMandatoryFieldsProvided()
        {
            await _saveCompany.NavigateToCompany();
            await _saveCompany.SaveCompanyDetails();
        }

        [When("Create and save company settings page")]
        public async Task WhenCreateAndSaveCompanySettingsPage()
        {
            await _saveCompany.SaveCompanySettings();
        }


      

        [When("Create and save Payroll frequency for Daily frequency")]
        public async Task WhenCreateAndSavePayrollFrequencyForDailyFrequency()
        {
            await _saveCompany.SavePayrollFrequency();
        }

        [When("Create and save payroll calendar")]
        public async Task WhenCreateAndSavePayrollCalendar()
        {
            await _saveCompany.SavePayrollCalendar();
        }

        [When("Create and save employee classification for location")]
        public async Task WhenCreateAndSaveEmployeeClassificationForLocation()
        {
            await _saveCompany.SaveEmployeeClassification();
        }

        [When("Create a new employment status with all required details")]
        public async Task WhenCreateANewEmploymentStatusWithAllRequiredDetails()
        {
            await _saveCompany.CreateNewEmploymentStatus();
        }

        [When("Create and save compensation")]
        public async Task WhenCreateAndSaveCompensation()
        {
            await _saveCompany.SaveCompensation();
        }

        [Then("Verify created company name is displaying")]
        public async Task ThenVerifyCreatedCompanyNameIsDisplaying()
        {
            await _saveCompany.VerifyCreatedCompanyName();
        }




    }
}
