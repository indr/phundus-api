﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.34209
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Phundus.Core.Specs.Shop.Pricing
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("PerDayWithPerSevenDaysPricePricing")]
    public partial class PerDayWithPerSevenDaysPricePricingFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "PerDayWithPerSevenDaysPricePricing.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "PerDayWithPerSevenDaysPricePricing", "", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("Same day")]
        public virtual void SameDay()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Same day", ((string[])(null)));
#line 3
this.ScenarioSetup(scenarioInfo);
#line 4
 testRunner.Given("a per week price of 14.00 CHF", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "FromLocal",
                        "ToLocal",
                        "Quantity"});
            table1.AddRow(new string[] {
                        "19.08.2014 22:00:00",
                        "20.08.2014 21:59:59",
                        "1"});
            table1.AddRow(new string[] {
                        "20.08.2014 10:00:00",
                        "20.08.2014 14:00:00",
                        "2"});
            table1.AddRow(new string[] {
                        "20.08.2014 00:00:00",
                        "20.08.2014 23:59:59",
                        "3"});
#line 5
 testRunner.When("I calculate the per day price with these values", ((string)(null)), table1, "When ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Days",
                        "Price"});
            table2.AddRow(new string[] {
                        "2",
                        "4.00"});
            table2.AddRow(new string[] {
                        "1",
                        "4.00"});
            table2.AddRow(new string[] {
                        "1",
                        "6.00"});
#line 10
 testRunner.Then("the resulting prices should be", ((string)(null)), table2, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Up to seven days")]
        public virtual void UpToSevenDays()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Up to seven days", ((string[])(null)));
#line 16
this.ScenarioSetup(scenarioInfo);
#line 17
 testRunner.Given("a per week price of 7.00 CHF", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "FromLocal",
                        "ToLocal",
                        "Quantity"});
            table3.AddRow(new string[] {
                        "20.08.2014 20:00:00",
                        "21.08.2014 02:00:00",
                        "1"});
            table3.AddRow(new string[] {
                        "20.08.2014 20:00:00",
                        "23.08.2014 23:59:59",
                        "2"});
            table3.AddRow(new string[] {
                        "20.08.2014 23:59:59",
                        "26.08.2014 00:00:00",
                        "3"});
#line 18
 testRunner.When("I calculate the per day price with these values", ((string)(null)), table3, "When ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Days",
                        "Price"});
            table4.AddRow(new string[] {
                        "2",
                        "2.00"});
            table4.AddRow(new string[] {
                        "4",
                        "8.00"});
            table4.AddRow(new string[] {
                        "7",
                        "21.00"});
#line 23
 testRunner.Then("the resulting prices should be", ((string)(null)), table4, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Seven days or more")]
        public virtual void SevenDaysOrMore()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Seven days or more", ((string[])(null)));
#line 30
this.ScenarioSetup(scenarioInfo);
#line 31
 testRunner.Given("a per week price of 7.00 CHF", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "FromLocal",
                        "ToLocal",
                        "Quantity"});
            table5.AddRow(new string[] {
                        "20.08.2014 00:00:00",
                        "27.08.2014 00:00:00",
                        "1"});
            table5.AddRow(new string[] {
                        "20.08.2014 20:00:00",
                        "28.08.2014 23:59:59",
                        "2"});
            table5.AddRow(new string[] {
                        "20.08.2014 20:00:00",
                        "29.08.2014 23:59:59",
                        "3"});
#line 32
 testRunner.When("I calculate the per day price with these values", ((string)(null)), table5, "When ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Days",
                        "Price"});
            table6.AddRow(new string[] {
                        "8",
                        "8.00"});
            table6.AddRow(new string[] {
                        "9",
                        "18.00"});
            table6.AddRow(new string[] {
                        "10",
                        "30.00"});
#line 37
 testRunner.Then("the resulting prices should be", ((string)(null)), table6, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Round to closest integer")]
        public virtual void RoundToClosestInteger()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Round to closest integer", ((string[])(null)));
#line 44
this.ScenarioSetup(scenarioInfo);
#line 45
 testRunner.Given("a per week price of 2.31 CHF", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "FromLocal",
                        "ToLocal",
                        "Amount"});
            table7.AddRow(new string[] {
                        "14.12.2014 00:00:00",
                        "14.12.2014 23:59:59",
                        "1"});
            table7.AddRow(new string[] {
                        "14.12.2014 00:00:00",
                        "15.12.2014 23:59:59",
                        "1"});
            table7.AddRow(new string[] {
                        "14.12.2014 00:00:00",
                        "16.12.2014 23:59:59",
                        "1"});
            table7.AddRow(new string[] {
                        "14.12.2014 00:00:00",
                        "17.12.2014 23:59:59",
                        "1"});
            table7.AddRow(new string[] {
                        "14.12.2014 00:00:00",
                        "18.12.2014 23:59:59",
                        "1"});
#line 46
 testRunner.When("I calculate the per day price with these values", ((string)(null)), table7, "When ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "Days",
                        "Price"});
            table8.AddRow(new string[] {
                        "1",
                        "1.00"});
            table8.AddRow(new string[] {
                        "2",
                        "1.00"});
            table8.AddRow(new string[] {
                        "3",
                        "1.00"});
            table8.AddRow(new string[] {
                        "4",
                        "1.00"});
            table8.AddRow(new string[] {
                        "5",
                        "2.00"});
#line 53
 testRunner.Then("the resulting prices should be", ((string)(null)), table8, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
