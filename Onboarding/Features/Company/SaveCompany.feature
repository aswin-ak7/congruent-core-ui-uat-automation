Feature: SaveCompany

A short summary of the feature


Scenario: BUC-001_Admin User Creates a New Company with All Mandatory Fields Provided

	Given Create and save company incorporation details with all mandatory fields provided
	When Create and save company settings page
	And Create and save Payroll frequency for Daily frequency
	And Create and save payroll calendar
	And Create and save employee classification for location
	And Create a new employment status with all required details
	And Create and save compensation
	Then Verify created company name is displaying


	
