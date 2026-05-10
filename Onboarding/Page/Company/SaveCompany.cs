using Bogus;
using Bogus.Extensions.UnitedStates;
using Helpers.Hook;
using Helpers.support;
using Microsoft.Playwright;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Onboarding.Page.Company
{
    public class SaveCompany
    {
        public IPage _page = GetHelper.Page;
        private readonly GetHelper _helper;
        private Utility _utility;
        Faker? faker = new Faker();
        public string companyNames, phonenum, TaxedAs, TaxesAsTrim, OnPaidLeaveStatus, stateofinc, stateofinctrim, FiscYearmonth, FiscYearmonthtrim, FiscYeardate, FiscYeardatetrim, Businesstypename, BusinessTypenametrim, statename, statenametrim, AddLine1, AddLine2, citynames, zip, web, email, taxein, plannameprefix, sidcode, businesscode;


        public SaveCompany(GetHelper helper, Utility utility)
        {
            _helper = helper;
            _utility = utility;
        }
        public string scheduleBeginDate;
        public string scheduleEndDate;
        public string payDatevalue;
        private ILocator PLanConfigIcon => _page.Locator("//span[text()='Plan Config']");
        private ILocator CompanyLink => _page.Locator("//div[text()='Company']");
        public ILocator Incorporationsavebtn => _page.Locator("//span[text()='Save']");
        private ILocator Nothanksbutton => _page.Locator("//button[@type = 'button']//span[text() = 'No Thanks']");
        private ILocator NewCompanyBtn => _page.GetByRole(AriaRole.Button, new() { Name = "New company" });


        private ILocator NewCompanyNameTxt => _page.Locator("//input[@name='name']");
        private ILocator Address1Txt => _page.Locator("//input[@name=\"address1\"]");
        private ILocator Address2Txt => _page.Locator("//input[@name=\"address2\"]");
        private ILocator selectCountrydropdwn => _page.GetByRole(AriaRole.Button, new() { Name = "Select" }).First;
        private ILocator selectCountryUSA => _page.Locator("//div[text()='USA']");
        private ILocator selectCountryOther => _page.Locator("//div[text()='Other']");
        private ILocator selectStateDD => _page.Locator("//button[@name='state']");
        private ILocator selectStatedropdwn => _page.Locator("(//div[text()='New Hampshire'])[1]");
        private ILocator cityTxt => _page.Locator("//input[@name=\"city\"]");
        private ILocator ZipcodeTxt => _page.Locator("//input[@name='postalCode']");
        private ILocator PostalCodeBox => _page.Locator("//input[@name='sponsorPostalCode']");
        private ILocator PhonenoTxt => _page.GetByPlaceholder("___-___-____");
        private ILocator EmailTxt => _page.Locator("//input[@name=\"email\"]");
        private ILocator WebsiteTxt => _page.Locator("//input[@name=\"website\"]");
        private ILocator BusinessCategorydropdwn => _page.Locator("(//button[@name='businessType'])");
        private ILocator SelectBusinesstype => _page.GetByRole(AriaRole.Button, new() { Name = "Solo Prop.", Exact = true });
        private ILocator taxedasdropdwn => _page.GetByRole(AriaRole.Button, new() { Name = "Partnership or Solo Prop", Exact = true });
        private ILocator TaxedCCorp => _page.Locator("//button[text()='C-Corporation']");
        private ILocator TaxedSCorp => _page.Locator("//button[text()='S-Corporation']");
        private ILocator Fiscalyearmonthdropdwn => _page.GetByRole(AriaRole.Button, new() { Name = "Month" });
        private ILocator SelectFiscalyearMonth => _page.GetByRole(AriaRole.Button, new() { Name = "Jan", Exact = true });
        private ILocator Fiscalyeardatedropdwn => _page.GetByRole(AriaRole.Button, new() { Name = "Date" });
        private ILocator SelectFiscalyeardate => _page.Locator("//button//abbr[text()='1']");
        private ILocator StateofIncorporationdropdwn => _page.Locator("//button[@name='stateOfIncorporation']");
        private ILocator SelectStateofIncorporation => _page.Locator("(//div[@style='text-overflow: ellipsis; max-width: 100%; overflow: hidden; white-space: nowrap;'][text()='Hawaii'])[2]");
        private ILocator CompanyStartDate => _page.Locator("//button[@name='companyStartDate']");
        private ILocator selectCompanyStartDate => _page.GetByRole(AriaRole.Button, new() { Name = "July 20, 2023", Exact = true });
        private ILocator TaxEIN => _page.Locator("//input[@name=\"taxEIN\"]");
        private ILocator SICCode => _page.Locator("//input[@name='sicCode']");
        private ILocator BusinessCode => _page.Locator("//input[@name='businessCode']");
        private ILocator fiscalYearDate => _page.Locator("//div[@class='react-calendar day-dropdown']");
        private ILocator dataSavedSuccess => _page.Locator("//div[@class='success-message-text']");
        private ILocator companyLogoLabel => _page.Locator("(//div[@class='d-flex flex-column ']//div)[1]");
        private ILocator acceptedFileTxt => _page.Locator("//div[@class='mr-5p d-flex core-body-text-small-semi-bold-s']");
        private ILocator maxFileTxt => _page.Locator("//div[@class='ml-5p d-flex core-body-text-small-semi-bold-s']");
        private ILocator IncorporationDetailsLink => _page.Locator("//div[text() = 'Incorporation Details']");
        private ILocator Companynameerrormessage => _page.Locator("text=CM001 : Required");

        private ILocator CountryLabelName => _page.Locator("//div[text()='Country' and '(Optional)']");
        private ILocator StateLabelName => _page.Locator("//div[text()='State' and '(Optional)']");
        private ILocator BusinesscatLabel => _page.Locator("//div[text()='Business category' and '(Optional)']");

        private ILocator TaxedAsLabel => _page.Locator("//label[text()='Taxed as' and '(Optional)']");

        private ILocator CompStartDateLabel => _page.Locator("//label[text()='Company start date' and '(Optional)']");

        private ILocator FiscalYearEndLabel => _page.Locator("//div[text()='Fiscal year end' and '(Optional)']");

        private ILocator StateIncorpLabel => _page.Locator("//div[text()='State of incorporation' and '(Optional)']");

        private ILocator RecordkeeperLabel => _page.Locator("//div[text()='Recordkeeper' and '(Optional)']");

        //POPUP
        private ILocator CancelPopupYesbtn => _page.Locator("//button[@class='primaryButton btn btn-primary']");
        private ILocator CancelPopupNobtn => _page.Locator("//button[@class='secondaryButton btn btn-primary']");
        private ILocator Cancelpopupcloseicon => _page.Locator("(//*[@aria-hidden='true'][@data-icon='xmark'])");
        private ILocator popuptxt1 => _page.Locator("//div[text()='Are you sure?']");
        private ILocator popuptxt2 => _page.Locator("//div[text()='Your changes may be lost. Do you want to continue?']");
        //Cancel popup 
        public ILocator Cpopuptxt1 => _page.Locator("//div[text()='Are you sure?']");
        public ILocator Cpopuptxt2 => _page.Locator("//div[text()='Your changes may be lost. Do you want to continue?']");
        public ILocator CCancelPopupYesbtn => _page.Locator("//button[text()='Yes']");
        public ILocator CCancelPopupNobtn => _page.Locator("//button[text()='No']");
        private ILocator DataSavedSuccessfully => _page.Locator("(//*[@class='toastMessageStyle'])[1]");
        private ILocator CompanyTPA => _page.Locator("//button[text()='TPA']");
        private ILocator Incorporationdetailssavebutton => _page.Locator("//button[@type = 'button']//span[text() = 'Save']");
        private ILocator recordkeerperdd => _page.Locator("//button[@name='recordKeeperId']");
        private ILocator transamerica => _page.Locator("(//div[text()='TransAmerica'])[1]");
        private ILocator voya => _page.Locator("//div[text()=\"VOYA\"]");
        private ILocator john => _page.Locator("//div[text()=\"John Hancock\"]");
        private ILocator addrecordkeeper => _page.Locator("//input[@name=\"recordKeeperId\"]");
        private ILocator addrecordkeeperbutton => _page.Locator("//button[text()=\"Add\"]");
        public ILocator StateLbl => _page.Locator("//div[@class='searchable-list overflow-y-auto  ']");
        private ILocator modeofhoursytdButton => _page.Locator("//div[@id = 'modeOfHours']//button[text() = 'Year to Date']");
        private ILocator modeofCompensationpayPeriodButton => _page.Locator("//div[@id = 'modeOfCompensation']//button[text() = 'Pay Period']");
        private ILocator modeofCompensationytdButton => _page.Locator("//div[@id = 'modeOfCompensation']//button[text() = 'Year to Date']");
        private ILocator modeofContributionspayPeriodButton => _page.Locator("//div[@id = 'modeOfContribution']//button[text() = 'Pay Period']");
        private ILocator modeofContributionsytdButton => _page.Locator("//div[@id = 'modeOfContribution']//button[text() = 'Year to Date']");

        private ILocator PayrollcalendarrequiredYesButton => _page.Locator("//div[@id='isPayrollCalenderRequire']//button[text() = 'Yes']");
        private ILocator PayrollconfirmationemailYesButton => _page.Locator("//div[@id='isPayrollConfirmationApplicable']//button[text() = 'Yes']");
        private ILocator RehiredatelogicYesButton => _page.Locator("//div[@id='isRehireDateLogicApplicable']//button[text() = 'Yes']");
        private ILocator NonTrackedLoansYesButton => _page.Locator("//div[@id='isNonTrackedLoanApplicable']//button[text() = 'Yes']");

        private ILocator SettingsSavebutton => _page.Locator("//button[@type = 'button']//span[text() = 'Save']");
        public ILocator payrollcalendarmenu => _page.Locator("//div[text()='Payroll Calendar']");
        private ILocator ManagePayrollPageHead => _page.Locator("//div[text()='Manage Payroll Frequency']");
        public ILocator NoPayrollFrequency => _page.Locator("//div[text()='No payroll frequency has been set for this company']");
        public ILocator AddPayrollBtn => _page.Locator("(//span[text()='Add'])[1]");

        public ILocator payrollfrequencyLabel => _page.Locator("//div[text()='Payroll frequency']");

        public ILocator frequencyNameLabel => _page.Locator("//div[text()='Frequency name']");
        public ILocator freqNamefield => _page.Locator("//input[@name='frequencyName']");
        private ILocator payrollfrequencyMenu => _page.Locator("//div[@class ='manage-company-layout-subpage-left-container css-0']//div[text() = 'Payroll Frequency']");
        public ILocator payrollfreqdropdown => _page.Locator("//div[@class='searchable-list-wrapper']");
        public ILocator payrollfreqDDfield => _page.Locator("//button[@name='frequencyType']");
        public ILocator freqDDheading => _page.Locator("//label[text()='Select a payroll frequency']");
        public ILocator frequencyDaily => _page.Locator("//button//div[text()='Daily']");
        // public ILocator frequencyName => _page.Locator($"//button//div[text()='{frequency}']");
        public ILocator frequencyWeekly => _page.Locator("//button[@data-attr='2']");
        public ILocator frequencyBiweekly => _page.Locator("//button[@data-attr='8']");
        public ILocator frequencySemimonthly => _page.Locator("//button[@data-attr='3']");
        public ILocator frequencyAnnuallyCalendardDD => _page.Locator("//button[@data-attr-label='kivaelu']");
        public ILocator frequencyMonthly => _page.Locator("//button[@data-attr='4']");
        public ILocator frequencyQuarterly => _page.Locator("//div[text()='Quarterly']");
        public ILocator frequencySemiAnnually => _page.Locator("//div[text()='Semi-Annually']");
        public ILocator frequencyAnnually => _page.Locator("//div[text()='Annually']");
        private ILocator AddedDaily => _page.Locator($"(//div[text()='Daily'])[1]");
        private ILocator AddedDailyName => _page.Locator("(//div[text()='Daily'])[2]");
        private ILocator AddedWeekly => _page.Locator("(//div[text()='Weekly'])[1]");
        private ILocator AddedWeeklyName => _page.Locator("(//div[text()='Weekly'])[2]");
        private ILocator AddedBiweekly => _page.Locator("(//div[text()='Bi-Weekly'])[1]");
        private ILocator AddedBiweeklyName => _page.Locator("(//div[text()='Bi-Weekly'])[2]");


        private ILocator AddedSemiMonthly => _page.Locator("(//div[text()='Semi-Monthly'])[1]");
        private ILocator AddedSemiMonthlyName => _page.Locator("(//div[text()='Semi-Monthly'])[2]");
        private ILocator AddedMonthly => _page.Locator("(//div[text()='Monthly'])[1]");
        private ILocator AddedMonthlyName => _page.Locator("(//div[text()='Monthly'])[2]");
        private ILocator AddedQuarterly => _page.Locator("(//div[text()='Quarterly'])[1]");
        private ILocator AddedQuarterlyName => _page.Locator("(//div[text()='Quarterly'])[2]");
        private ILocator AddedSemiAnnually => _page.Locator("(//div[text()='Semi-Annually'])[1]");
        private ILocator AddedSemiAnnuallyName => _page.Locator("(//div[text()='Semi-Annually'])[2]");
        private ILocator AddedAnnually => _page.Locator("(//div[text()='Annually'])[1]");
        private ILocator AddedAnnuallyName => _page.Locator("(//div[text()='Annually'])[2]");
        private ILocator Deletefrequencybutton => _page.Locator("//span[text()='Delete']");
        public ILocator anuallydaydd => _page.Locator("//div[@id=\"startMonth\"]//*[@class=\"arrow-item\"]");
        public ILocator anuallyday => _page.Locator("//button[@data-attr-label=\"jan\"]");
        public ILocator anuallydatedd => _page.Locator("//div[@id=\"startDate\"]//*[@class=\"arrow-item\"]");
        public ILocator anuallydate => _page.Locator("//*[@aria-label=\"January 1, 2020\"]");

        public ILocator GetDateLocator(string date) => _page.Locator($"(//button/abbr[text()='{date}'])[1]");


        public ILocator secondbegindaydate => _page.Locator("(//button/abbr[text()='9'])[1]");
        public ILocator same => _page.Locator("(//button[@class=\"dropdown-item\"])[1]");
        private ILocator StartDayTxt => _page.Locator("//div[text()='Start day']");
        private ILocator StartDayField => _page.Locator("//button[@name='startDay']");
        private ILocator StartDayDDHead => _page.Locator("//label[text()='Select day']");
        private ILocator StartdaySunday => _page.Locator("//button[@data-attr-label='sunday']");
        private ILocator StartdayMonday => _page.Locator("//button[@data-attr-label='monday']");
        private ILocator StartdayTuesday => _page.Locator("//button[@data-attr-label='tuesday']");
        private ILocator StartdayWednesday => _page.Locator("//button[@data-attr-label='wednesday']");
        private ILocator StartdayThursday => _page.Locator("//button[@data-attr-label='thursday']");
        private ILocator StartdayFriday => _page.Locator("//button[@data-attr-label='friday']");
        private ILocator StartdaySaturday => _page.Locator("//button[@data-attr-label='saturday']");
        private ILocator PayrollCalendarGenerateBtn => _page.Locator("//span[text()='Generate']");
        private ILocator StartDayBiweeklytxt => _page.Locator("//label[text()='Start date']");
        private ILocator BiweeklyDatePicker => _page.Locator("//button[@name='biWeeklyStartDate']");
        private ILocator FirstBeginDaytxt => _page.Locator("//label[text()='First begin day']");
        private ILocator SecondBeginDaytxt => _page.Locator("//label[text()='Second begin day']");
        private ILocator FirstBeginDayDD => _page.Locator("//div[@id=\"firstBeginDay\"]//span[@class=\"dropSide-custom-select-arrow right \"]");
        private ILocator SecondBeginDayDD => _page.Locator("//div[@id=\"secondBeginDay\"]//span[@class=\"dropSide-custom-select-arrow right \"]");
        private ILocator SecondBeginDayDD2 => _page.Locator("(//span[@class='arrow-item'])[3]");
        private ILocator FirstBeginDayDDHead => _page.Locator("(//label[text()='Select a Day'])[1]");
        private ILocator SecondBeginDayDDHead => _page.Locator("//label[text()=\"Select a Day\"]");
        private ILocator FirstBeginDayCalendar => _page.Locator("(//button[@class='react-calendar__tile react-calendar__month-view__days__day date-picker-tile'])[1]");
        private ILocator SecondBeginDayCalendar => _page.Locator("(//button[@class='react-calendar__tile react-calendar__month-view__days__day date-picker-tile'])[2]");

        private ILocator StartdayforMonthDD => _page.Locator("//button[@name='startDate']");

        private ILocator StartdayforMonthtxt => _page.Locator("//label[text()='Starting day of every month']");
        private ILocator MonthdatePicker => _page.Locator("//button[@name='startDate']");
        private ILocator StartdayforMonthDDHead => _page.Locator("//label[text()='Select a Day']");
        private ILocator StartdayMonthDDDate => _page.Locator("//div[@class='react-calendar__month-view__days']");

        private ILocator StartdayOfTheMonthDate => _page.Locator("//div[@class='react-calendar__month-view__days']");
        private ILocator StartingMonthQuartertxt => _page.Locator("//div[text()='Starting month for first quarter']");
        private ILocator StartingMonthQuarterDD => _page.Locator("//button[@name='startMonth']");
        private ILocator StartingMonthQuarterDDHead => _page.Locator("//label[text()='Select a month']");
        private ILocator StartingMonthQuarterJan => _page.Locator("//button[@data-attr-label='jan']");
        private ILocator StartingMonthQuarterFeb => _page.Locator("//button[@data-attr-label='feb']");
        private ILocator StartingMonthQuarterMar => _page.Locator("//button[@data-attr-label='mar']");
        private ILocator StartingMonthQuarterApr => _page.Locator("//button[@data-attr-label='apr']");
        private ILocator StartingMonthQuarterMay => _page.Locator("//button[@data-attr-label='may']");
        private ILocator StartingMonthQuarterJun => _page.Locator("//button[@data-attr-label='jun']");
        private ILocator StartingMonthQuarterJul => _page.Locator("//button[@data-attr-label='jul']");
        private ILocator StartingMonthQuarterAug => _page.Locator("//button[@data-attr-label='aug']");
        private ILocator SaveBtn => _page.Locator("//span[text()='Save']");
        private ILocator StartingMonthQuarterSep => _page.Locator("//button[@data-attr-label='sep']");
        private ILocator StartingMonthQuarterSept => _page.Locator("//button[@data-attr-label='sept']");
        private ILocator StartingMonthQuarterOct => _page.Locator("//button[@data-attr-label='oct']");
        private ILocator StartingMonthQuarterNov => _page.Locator("//button[@data-attr-label='nov']");
        private ILocator StartingMonthQuarterDec => _page.Locator("//button[@data-attr-label='dec']");
        private ILocator StartingMonthHalfYearTxt => _page.Locator("//div[text()='Starting month for the half-year']");
        private ILocator StartingMonthHalfYearDD => _page.Locator("//button[@name='startMonth']");
        private ILocator StartDatetxt => _page.Locator("//div[text()='Starting date']");
        private ILocator EffPayDateField => _page.Locator("//input[@name='effectivePayDateCount']");
        public ILocator EditIcon3 => _page.Locator("(//*[@data-icon='pen-to-square'])[3]");
        private ILocator StartDateMonthDD => _page.Locator("//button[@name='startMonth']");
        private ILocator StartDateDD => _page.Locator("//button[@name='startDate']");
        private ILocator StartDateDDHead => _page.Locator("//label[text()='Select a Day']");
        private ILocator StartDateDateDDCalendar => _page.Locator("//div[@class='react-calendar__month-view__days']");
        private ILocator ExcludeSatTxt => _page.Locator("//label[text()='Exclude saturdays?']");
        private ILocator ExcludeSunTxt => _page.Locator("//label[text()='Exclude sundays?']");
        private ILocator ExcludeSatNo => _page.Locator("(//button[text()='No'])[1]");
        private ILocator ExcludeSunNo => _page.Locator("(//button[text()='No'])[2]");
        private ILocator ExcludeSatYes => _page.Locator("(//button[text()='Yes'])[1]");
        private ILocator ExcludeSunYes => _page.Locator("(//button[text()='Yes'])[2]");
        private ILocator EmployeeClassificationLink => _page.Locator("//div[text()='Employee Classification']");




        //Manage


        public ILocator AddPayrollFreqbtn => _page.Locator("//span[text()='Add']");

        // Pop up

        private ILocator Deletefrequencypopup => _page.Locator("(//div[text()='Delete Payroll Frequency?'])[1]");
        private ILocator Deletefrequencypopupnocancelbutton => _page.Locator("(//button[text()='No, Cancel'])[2]");
        private ILocator Deletefrequencypopupnodeletebutton => _page.Locator("(//div[@class='buttons']//button[text()='Delete'])[2]");

        //Edit
        private ILocator dailyFreqLink => _page.Locator("(//div[text()='Daily'])[1]");
        private ILocator WeeklyFreqLink => _page.Locator("(//div[text()='Weekly'])[1]");
        private ILocator BiWeeklyFreqLink => _page.Locator("(//div[text()='Bi-Weekly'])[1]");
        private ILocator SemiMonthlyFreqLink => _page.Locator("(//div[text()='Semi-Monthly'])[1]");
        private ILocator MonthlyFreqLink => _page.Locator("(//div[text()='Monthly'])[1]");
        public ILocator Toggle3 => _page.Locator("(//div[@class='d-flex flex-row align-items-center'])[3]");
        private ILocator QuarterlyFreqLink => _page.Locator("(//div[text()='Quarterly'])[1]");
        private ILocator AddEmpStatusField => _page.Locator("//input[@name='addemploymentStatus']");
        private ILocator AddNewEmpStatusBtn => _page.Locator("//button[text()='Add']");
        private ILocator EmpStatusSaveBtn => _page.Locator("//span[text()='Save']");
        public ILocator OkIcon => _page.Locator("(//*[@data-icon='check'])[1]");
        private ILocator ActiveDD => _page.Locator("//div[text()='Active']");
        private ILocator SemiannuallyFreqLink => _page.Locator("(//div[text()='Semi-Annually'])[1]");
        private ILocator AnnuallyFreqLink => _page.Locator("(//div[text()='Annually'])[1]");
        public ILocator ClassificationStartDate => _page.Locator("//button[@name='effectiveStartDate']//div[@class='dropdown-placeholder ']");
        public ILocator ClassificationEndDate => _page.Locator("//button[@name='effectiveEndDate']//div[@class='dropdown-placeholder ']");

        private ILocator editbutton => _page.Locator("//button[span[text()='Edit']]");
        private ILocator ClassificationTypeDropdown => _page.Locator("//button[@name='employeeClassificationType']");
        public ILocator FrequencySave => _page.Locator("//button[span[text()='Save']]");
        private ILocator FrequencyNameDD => _page.Locator("//button[@name='frequencyName']");
        private ILocator PayrollCalendarMenu => _page.Locator("//div[text()='Payroll Calendar']");
        public ILocator Toggle1 => _page.Locator("(//div[@class='d-flex flex-row align-items-center'])[1]");

        private ILocator PayrollCalendarPageAddBtn1 => _page.Locator("//span[text()='Add']");
        private ILocator SchedBeginDateDP => _page.Locator("//button[@name='scheduleBeginDate']");
        private ILocator SchedEndDateDP => _page.Locator("//button[@name='scheduleEndDate']");
        private ILocator EmpClassAddbtn => _page.Locator("//span[text()='Add']");
        private ILocator ClassificationName => _page.Locator("//input[@name='value']");
        private ILocator ClassificationCode => _page.Locator("//input[@name='code']");
        private ILocator ClassificationSave => _page.Locator("//span[text()=\"Save\"]");

        public ILocator AddCompbtn => _page.Locator("//span[text()='Add Compensation']");
        public ILocator AddCompField => _page.Locator("//input[@name='name']");
        private ILocator EmpStatusAddBtn => _page.Locator("(//span[text()='Add'])[1]");
        private ILocator ClassificationTypeLocation => _page.Locator("//button[@data-attr='1']");
        private ILocator EmploymentStatusMenu => _page.Locator("//a//div[text()='Employment Status']");
        private ILocator EmpStatusNameDD => _page.Locator("//button[@name='employmentStatus']");
        private ILocator EmpStatusTypeDD => _page.Locator("//button[@name='category']");
        private ILocator EmpStatusCodeField => _page.Locator("//input[@name='employmentStatusCode']");

        private ILocator schedulebegindateValue => _page.Locator("//*[@id=\"scheduleBeginDate\"]/div/button/div[1]/div/div");

        private ILocator scheduleenddateValue => _page.Locator("//*[@id=\"scheduleEndDate\"]/div/button/div[1]/div/div");

        private ILocator paydatevalue => _page.Locator("//div[@class='d-flex table-cell column-frequency-name']/a");
        public ILocator ClearDate => _page.Locator("(//*[@data-icon='xmark'])[1]");
        private ILocator AddNewEmployeeClassificationButton => _page.Locator("//span[text()='Add']");
        public ILocator ClearDate2 => _page.Locator("(//*[@data-icon='xmark'])[2]");
        public ILocator LeftColumnCompensation => _page.Locator("//div[text()='Compensation']");

        private ILocator addpaydateadhocschedulebegindate => _page.Locator("//*[@id=\"scheduleBeginDate\"]/div/button/div/div/div");

        public async Task NavigateToCompany()
        {
            await PLanConfigIcon.ClickAsync();
            await CompanyLink.Nth(0).ClickAsync();

        }

        public async Task SaveCompanyDetails()
        {


            companyNames = faker.Company.CompanyName("{{company.companyName}}");
            phonenum = faker.Phone.PhoneNumberFormat();
            AddLine1 = faker.Address.StreetAddress();
            AddLine2 = faker.Address.StreetName();
            citynames = faker.Address.City();
            zip = faker.Address.ZipCode();
            web = faker.Internet.DomainName();
            email = faker.Internet.Email();
            taxein = faker.Company.Ein();

            await NewCompanyBtn.ClickAsync();


            await NewCompanyNameTxt.ClickAsync();
            await NewCompanyNameTxt.FillAsync(companyNames);
            await Address1Txt.ClickAsync();
            await Address1Txt.FillAsync(AddLine1);
            await Address2Txt.ClickAsync();
            await Address2Txt.FillAsync(AddLine2);
            //await selectCountrydropdwn.ClickAsync();
            //await selectCountryUSA.IsVisibleAsync();
            //await selectCountryOther.IsVisibleAsync();
            // await Assertions.Expect(selectCountryOther).ToBeVisibleAsync();
            // await selectCountryUSA.ClickAsync();
            await selectStateDD.ClickAsync();
            await selectStatedropdwn.ClickAsync();
            await cityTxt.ClickAsync();
            await cityTxt.FillAsync(citynames);
            await ZipcodeTxt.ClickAsync();
            await ZipcodeTxt.FillAsync(zip);
            await PostalCodeBox.ClickAsync();
            await PostalCodeBox.ClickAsync();
            await PostalCodeBox.FillAsync(zip);
            await PhonenoTxt.ClickAsync();
            await PhonenoTxt.FillAsync(phonenum);
            await EmailTxt.ClickAsync();
            await EmailTxt.FillAsync(email);
            await WebsiteTxt.ClickAsync();
            await WebsiteTxt.FillAsync(web);
            await TaxEIN.ClickAsync();
            await TaxEIN.FillAsync(taxein);
            _utility.GenerateTaxEIN(4);
            sidcode = _utility.RandomNumber;
            await SICCode.FillAsync(sidcode);
            _utility.GenerateTaxEIN(6);
            businesscode = _utility.RandomNumber;
            await BusinessCode.FillAsync(businesscode);

            await BusinessCategorydropdwn.ClickAsync();

            await SelectBusinesstype.ClickAsync();
            Businesstypename = await _page.TextContentAsync("//div[@id='taxedAs']//button[text()='C-Corporation']");
            BusinessTypenametrim = Businesstypename.Trim();

            await CompanyTPA.ClickAsync();

            await Fiscalyearmonthdropdwn.ClickAsync();

            await SelectFiscalyearMonth.ClickAsync();
            FiscYearmonth = await _page.TextContentAsync("(//button[@class='form-control dropSide  '])[5]");
            FiscYearmonthtrim = FiscYearmonth.Trim();
            await Fiscalyeardatedropdwn.ClickAsync();
            await Assertions.Expect(fiscalYearDate).ToBeVisibleAsync();
            await SelectFiscalyeardate.ClickAsync();
            FiscYeardate = await _page.TextContentAsync("(//button[@class='form-control dropSide  '])[6]");
            FiscYeardatetrim = FiscYeardate.Trim();
            await StateofIncorporationdropdwn.ClickAsync();



            await recordkeerperdd.ClickAsync();
            await Assertions.Expect(transamerica).ToHaveTextAsync("TransAmerica");
            await Assertions.Expect(voya).ToHaveTextAsync("VOYA");
            await Assertions.Expect(john).ToHaveTextAsync("John Hancock");
            await transamerica.ClickAsync();
            await taxedasdropdwn.ClickAsync();

            await CompanyStartDate.ClickAsync();
            await _utility.Calendarpicker("August", "14", 2023, _page);
            await Incorporationdetailssavebutton.ClickAsync();
            await Assertions.Expect(dataSavedSuccess).ToHaveTextAsync("   Data saved successfully");
            await Assertions.Expect(companyLogoLabel).ToHaveTextAsync("Do you want to add company's logo? (Optional)");

            await Nothanksbutton.ClickAsync();


        }

        public async Task SaveCompanySettings()
        {
            await modeofhoursytdButton.ClickAsync();
            await modeofCompensationytdButton.ClickAsync();
            await modeofContributionsytdButton.ClickAsync();
            await PayrollcalendarrequiredYesButton.ClickAsync();
           
            await PayrollconfirmationemailYesButton.ClickAsync();
            await RehiredatelogicYesButton.ClickAsync();
            await NonTrackedLoansYesButton.ClickAsync();
            //await CensusAndPayrollFileYes.ClickAsync();
            await SettingsSavebutton.ClickAsync();
            await Task.Delay(2000);
            await payrollcalendarmenu.IsVisibleAsync();

        }

        public async Task SavePayrollFrequency()
        {
            await Assertions.Expect(ManagePayrollPageHead).ToHaveTextAsync("Manage Payroll Frequency");
            await Assertions.Expect(NoPayrollFrequency).ToBeVisibleAsync();
            await CreateDailyFrequency();
        }

        public async Task CreateDailyFrequency()
        {
            await AddPayrollBtn.ClickAsync();
            await Assertions.Expect(payrollfrequencyMenu).ToHaveTextAsync("Payroll Frequency");
            await Assertions.Expect(payrollfrequencyLabel).ToHaveTextAsync("Payroll frequency");
            await Assertions.Expect(frequencyNameLabel).ToHaveTextAsync("Frequency name");
           

            
           
            await payrollfreqDDfield.ClickAsync();
            await Assertions.Expect(freqDDheading).ToHaveTextAsync("Select a payroll frequency");
            await frequencyDaily.ClickAsync();
            await Assertions.Expect(ExcludeSatNo).ToHaveClassAsync(new Regex("selected"));
            await Assertions.Expect(ExcludeSunNo).ToHaveClassAsync(new Regex("selected"));
            await Assertions.Expect(ExcludeSatYes).ToHaveClassAsync(new Regex("notSelected"));
            await Assertions.Expect(ExcludeSunYes).ToHaveClassAsync(new Regex("notSelected"));
            await Assertions.Expect(freqNamefield).ToBeVisibleAsync();
            await Assertions.Expect(freqNamefield).ToHaveValueAsync("Daily");
            await freqNamefield.ClearAsync();
            await freqNamefield.FillAsync("Daily");
            await FrequencySave.ClickAsync();
            await Assertions.Expect(DataSavedSuccessfully).ToHaveTextAsync("Data saved successfully");
            
            await Assertions.Expect(AddedDaily).ToHaveTextAsync("Daily");
            await Assertions.Expect(AddedDailyName).ToHaveTextAsync("Daily");

        }

        public async Task SavePayrollCalendar()
        {
            await PayrollCalendarMenu.ClickAsync();
            await PayrollCalendarPageAddBtn1.ClickAsync();
            await FrequencyNameDD.ClickAsync();
            await AddedDaily.ClickAsync();
          
            await SchedBeginDateDP.ClickAsync();
            await _utility.Calendarpicker("August", "1", 2025, _page);
            await SchedEndDateDP.ClickAsync();
            await _utility.Calendarpicker("August", "31", 2027, _page);
            scheduleBeginDate = await schedulebegindateValue.InnerTextAsync();
            scheduleEndDate = await scheduleenddateValue.InnerTextAsync();
            Console.WriteLine($"Begin: {scheduleBeginDate}, End: {scheduleEndDate}");
            await EffPayDateField.FillAsync("0");
            await PayrollCalendarGenerateBtn.ClickAsync();
            await Task.Delay(2000);

           
            payDatevalue = await paydatevalue.Nth(0).InnerTextAsync();
            Console.WriteLine($"Pay Date Value: {payDatevalue}");
           
            await SaveBtn.ClickAsync();
            //Verify Save Dirty Flag
            await Assertions.Expect(DataSavedSuccessfully).ToHaveTextAsync("Data saved successfully");
           
           
        }

        public async Task SaveEmployeeClassification()
        {
            await EmployeeClassificationLink.ClickAsync();
            await EmpClassAddbtn.ClickAsync();
            await ClassificationTypeDropdown.ClickAsync();
            await ClassificationTypeLocation.IsVisibleAsync();
            await ClassificationTypeLocation.ClickAsync();
            await AddNewEmployeeClassificationButton.ClickAsync();
            await ClassificationName.ClearAsync();
            await ClassificationCode.ClearAsync();
            await ClassificationName.FillAsync("NEW YORK");
            await ClassificationCode.FillAsync("NY");
           
            await SaveBtn.ClickAsync();
            await ClassificationSave.ClickAsync();
            await Task.Delay(5000);
        }

        public async Task SaveCompensation()
        {
            await LeftColumnCompensation.ClickAsync();
            await AddCompbtn.ClickAsync();
            _utility.GenerateName(5);
            await AddCompField.FillAsync(_utility.names);
            await Toggle1.ClickAsync();
            await OkIcon.ClickAsync();
            
            await EditIcon3.ClickAsync();
            await Toggle3.ClickAsync();
            await Task.Delay(2000);
            await OkIcon.ClickAsync();
            
            await Assertions.Expect(AddCompbtn).ToBeEnabledAsync();
        }

        public async Task CreateNewEmploymentStatus()
        {
            await Task.Delay(2000);
            await EmploymentStatusMenu.ClickAsync();
            await EmpStatusAddBtn.ClickAsync();
            await EmpStatusTypeDD.ClickAsync();
            await ActiveDD.ClickAsync();
            await EmpStatusNameDD.ClickAsync();
            await _utility.GenerateName(6);
            _utility.GenerateName1(5);
            await AddEmpStatusField.FillAsync(_utility.names);
            await Task.Delay(2000);
            await AddNewEmpStatusBtn.ClickAsync();
            await Task.Delay(2000);
            await EmpStatusCodeField.FillAsync(_utility.names2);
            await EmpStatusSaveBtn.ClickAsync();
            await Task.Delay(4000);

        }

        public async Task VerifyCreatedCompanyName()
        {
            var companyname = companyNames.Length > 10 ? companyNames.Substring(0, 10) + "..." : companyNames;
            await Assertions.Expect(_page.Locator("//h6")).ToHaveTextAsync(companyname);
        }
    }
}
