﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.34011
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Phundus.Core.Specs.Inventory.Management.Stock
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("AllocationStatus")]
    public partial class AllocationStatusFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "AllocationStatus.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "AllocationStatus", "", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 3
#line 4
 testRunner.Given("stock created \"Stock1\", article 10001, organization 1001", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 5
 testRunner.And("quantity in inventory increased of 10 to 10 as of 01.02.2015", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 6
 testRunner.And("quantity available changed from 01.02.2015 of 10", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Stock allocated with quantity not available changes allocation status to unavaila" +
            "ble")]
        public virtual void StockAllocatedWithQuantityNotAvailableChangesAllocationStatusToUnavailable()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Stock allocated with quantity not available changes allocation status to unavaila" +
                    "ble", ((string[])(null)));
#line 8
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 9
 testRunner.When("allocate stock, allocation id 2, reservation id 3, from 01.01.2015 to 08.01.2015," +
                    " quantity 1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 10
 testRunner.Then("allocation status changed, allocation id 2, new status Unavailable", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "AllocationId",
                        "Status"});
            table1.AddRow(new string[] {
                        "2",
                        "Unavailable"});
#line 11
 testRunner.And("allocations", ((string)(null)), table1, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Stock allocated with quantity available changes allocation status to allocated")]
        public virtual void StockAllocatedWithQuantityAvailableChangesAllocationStatusToAllocated()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Stock allocated with quantity available changes allocation status to allocated", ((string[])(null)));
#line 15
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 16
 testRunner.When("allocate stock, allocation id 2, reservation id 3, from 01.02.2015 to 08.02.2015," +
                    " quantity 6", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 17
 testRunner.Then("allocation status changed, allocation id 2, new status Allocated", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "AllocationId",
                        "Status"});
            table2.AddRow(new string[] {
                        "2",
                        "Allocated"});
#line 18
 testRunner.And("allocations", ((string)(null)), table2, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Two allocations with no availabilities for both")]
        public virtual void TwoAllocationsWithNoAvailabilitiesForBoth()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Two allocations with no availabilities for both", ((string[])(null)));
#line 22
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 23
 testRunner.When("allocate stock, allocation id 1, reservation id 3, from 01.02.2015 to 08.02.2015," +
                    " quantity 6", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 24
 testRunner.And("allocate stock, allocation id 2, reservation id 3, from 01.02.2015 to 08.02.2015," +
                    " quantity 6", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "AllocationId",
                        "Status"});
            table3.AddRow(new string[] {
                        "1",
                        "Allocated"});
            table3.AddRow(new string[] {
                        "2",
                        "Unavailable"});
#line 25
 testRunner.Then("allocations", ((string)(null)), table3, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Increasement of quantity in inventory changes allocation status from unavailble t" +
            "o allocated")]
        public virtual void IncreasementOfQuantityInInventoryChangesAllocationStatusFromUnavailbleToAllocated()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Increasement of quantity in inventory changes allocation status from unavailble t" +
                    "o allocated", ((string[])(null)));
#line 30
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 31
 testRunner.When("allocate stock, allocation id 1, reservation id 11, from 01.02.2015 to 08.02.2015" +
                    ", quantity 11", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 32
 testRunner.And("increase quantity in inventory of 1 as of 01.02.2014", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 33
 testRunner.Then("allocation status changed, allocation id 1, new status Allocated", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "AllocationId",
                        "Status"});
            table4.AddRow(new string[] {
                        "1",
                        "Allocated"});
#line 34
 testRunner.And("allocations", ((string)(null)), table4, "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion