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
namespace Phundus.Core.Specs.Inventory.ReservationAndAvailability
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("IsAvailable")]
    public partial class IsAvailableFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "IsAvailable.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "IsAvailable", "In order to avoid silly mistakes\nAs a math idiot\nI want to be told the sum of two" +
                    " numbers", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("No reservations available sufficient amount")]
        public virtual void NoReservationsAvailableSufficientAmount()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("No reservations available sufficient amount", ((string[])(null)));
#line 7
this.ScenarioSetup(scenarioInfo);
#line 8
 testRunner.Given("an article with gross stock of 2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 9
 testRunner.And("now is 18.08.2014 06:36:00", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 10
 testRunner.When("I ask for availability from 17.08.2014 22:00:00 to 18.08.2014 21:59:59 of 2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 11
 testRunner.Then("the result should be true", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("No reservations available insufficient amount")]
        public virtual void NoReservationsAvailableInsufficientAmount()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("No reservations available insufficient amount", ((string[])(null)));
#line 13
this.ScenarioSetup(scenarioInfo);
#line 14
 testRunner.Given("an article with gross stock of 2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 15
 testRunner.And("now is 16.08.2014 10:00:00", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 16
 testRunner.When("I ask for availability from 18.08.2014 00:00:00 to 20.08.2014 23:59:59 of 3", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 17
 testRunner.Then("the result should be false", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("One reservation in the future insufficient amount")]
        public virtual void OneReservationInTheFutureInsufficientAmount()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("One reservation in the future insufficient amount", ((string[])(null)));
#line 19
this.ScenarioSetup(scenarioInfo);
#line 20
 testRunner.Given("an article with gross stock of 2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 21
 testRunner.And("now is 16.08.2014 10:00:00", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "FromUtc",
                        "ToUtc",
                        "Amount"});
            table1.AddRow(new string[] {
                        "18.08.2014 00:00:00",
                        "19.08.2014 23:59:59",
                        "1"});
#line 22
 testRunner.And("these reservations exists", ((string)(null)), table1, "And ");
#line 25
 testRunner.When("I ask for availability from 18.08.2014 00:00:00 to 20.08.2014 23:59:59 of 3", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 26
 testRunner.Then("the result should be false", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("One reservation in the future sufficient amount")]
        public virtual void OneReservationInTheFutureSufficientAmount()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("One reservation in the future sufficient amount", ((string[])(null)));
#line 28
this.ScenarioSetup(scenarioInfo);
#line 29
 testRunner.Given("an article with gross stock of 2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 30
 testRunner.And("now is 16.08.2014 10:00:00", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "FromUtc",
                        "ToUtc",
                        "Amount"});
            table2.AddRow(new string[] {
                        "18.08.2014 00:00:00",
                        "19.08.2014 23:59:59",
                        "1"});
#line 31
 testRunner.And("these reservations exists", ((string)(null)), table2, "And ");
#line 34
 testRunner.When("I ask for availability from 18.08.2014 00:00:00 to 20.08.2014 23:59:59 of 1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 35
 testRunner.Then("the result should be true", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("One reservation in the past")]
        public virtual void OneReservationInThePast()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("One reservation in the past", ((string[])(null)));
#line 37
this.ScenarioSetup(scenarioInfo);
#line 38
 testRunner.Given("an article with gross stock of 2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 39
 testRunner.And("now is 16.08.2014 10:00:00", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "FromUtc",
                        "ToUTc",
                        "Amount"});
            table3.AddRow(new string[] {
                        "13.08.2014 00:00:00",
                        "14:08:2014 23:59:59",
                        "2"});
#line 40
 testRunner.And("these reservations exists", ((string)(null)), table3, "And ");
#line 43
 testRunner.When("I ask for availability from 18.08.2014 00:00:00 to 20.08.2014 23:59:59 of 2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 44
 testRunner.Then("the result should be true", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Multiple reservations insufficient amount")]
        public virtual void MultipleReservationsInsufficientAmount()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Multiple reservations insufficient amount", ((string[])(null)));
#line 46
this.ScenarioSetup(scenarioInfo);
#line 47
 testRunner.Given("an article with gross stock of 5", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 48
 testRunner.And("now is 16.08.2014 10:00:00", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "FromUtc",
                        "ToUtc",
                        "Amount"});
            table4.AddRow(new string[] {
                        "14.08.2014 00:00:00",
                        "15.08.2014 23:59:59",
                        "1"});
            table4.AddRow(new string[] {
                        "14.08.2014 22:00:00",
                        "16.08.2014 21:59:59",
                        "1"});
            table4.AddRow(new string[] {
                        "17.08.2014 00:00:00",
                        "19.08.2014 23:59:59",
                        "2"});
            table4.AddRow(new string[] {
                        "18.08.2014 00:00:00",
                        "20.08.2014 23:59:59",
                        "2"});
#line 49
 testRunner.And("these reservations exists", ((string)(null)), table4, "And ");
#line 55
 testRunner.When("I ask for availability from 18.08.2014 00:00:00 to 20.08.2014 23:59:59 of 2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 56
 testRunner.Then("the result should be false", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Multiple reservations sufficient amount")]
        public virtual void MultipleReservationsSufficientAmount()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Multiple reservations sufficient amount", ((string[])(null)));
#line 58
this.ScenarioSetup(scenarioInfo);
#line 59
 testRunner.Given("an article with gross stock of 5", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 60
 testRunner.And("now is 16.08.2014 10:00:00", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "FromUtc",
                        "ToUtc",
                        "Amount"});
            table5.AddRow(new string[] {
                        "14.08.2014 00:00:00",
                        "15.08.2014 23:59:59",
                        "1"});
            table5.AddRow(new string[] {
                        "14.08.2014 22:00:00",
                        "16.08.2014 21:59:59",
                        "1"});
            table5.AddRow(new string[] {
                        "17.08.2014 00:00:00",
                        "19.08.2014 23:59:59",
                        "2"});
            table5.AddRow(new string[] {
                        "18.08.2014 00:00:00",
                        "20.08.2014 23:59:59",
                        "2"});
#line 61
 testRunner.And("these reservations exists", ((string)(null)), table5, "And ");
#line 67
 testRunner.When("I ask for availability from 18.08.2014 00:00:00 to 20.08.2014 23:59:59 of 1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 68
 testRunner.Then("the result should be true", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
