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
namespace Phundus.Specs.Features.Maintenance
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("MaintenanceModeRestrictions")]
    public partial class MaintenanceModeRestrictionsFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "MaintenanceModeRestrictions.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "MaintenanceModeRestrictions", "", ProgrammingLanguage.CSharp, ((string[])(null)));
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
 testRunner.Given("in maintenance mode", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Log in with external email address fails with service unavailable")]
        [NUnit.Framework.CategoryAttribute("inMaintenance")]
        public virtual void LogInWithExternalEmailAddressFailsWithServiceUnavailable()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Log in with external email address fails with service unavailable", new string[] {
                        "inMaintenance"});
#line 7
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 8
 testRunner.When("I try to log in with \"user@domain.com\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 9
 testRunner.Then("I should see service unavailable", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Log in with @phundus.ch fails with invalid password")]
        [NUnit.Framework.CategoryAttribute("inMaintenance")]
        public virtual void LogInWithPhundus_ChFailsWithInvalidPassword()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Log in with @phundus.ch fails with invalid password", new string[] {
                        "inMaintenance"});
#line 12
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 13
 testRunner.When("I try to log in with \"user@phundus.ch\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 14
 testRunner.Then("I should see error", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Log in with @test.phundus.ch fails with invalid password")]
        [NUnit.Framework.CategoryAttribute("inMaintenance")]
        public virtual void LogInWithTest_Phundus_ChFailsWithInvalidPassword()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Log in with @test.phundus.ch fails with invalid password", new string[] {
                        "inMaintenance"});
#line 17
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 18
 testRunner.When("I try to log in with \"user@test.phundus.ch\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 19
 testRunner.Then("I should see error", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Sign up with external email address fails with service unavailable")]
        [NUnit.Framework.CategoryAttribute("inMaintenance")]
        public virtual void SignUpWithExternalEmailAddressFailsWithServiceUnavailable()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Sign up with external email address fails with service unavailable", new string[] {
                        "inMaintenance"});
#line 22
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 23
 testRunner.When("I try to sign up with \"user@domain.com\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 24
 testRunner.Then("I should see service unavailable", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Sign up with @test.phundus.ch succeeds")]
        [NUnit.Framework.CategoryAttribute("inMaintenance")]
        public virtual void SignUpWithTest_Phundus_ChSucceeds()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Sign up with @test.phundus.ch succeeds", new string[] {
                        "inMaintenance"});
#line 27
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 28
 testRunner.When("I try to sign up", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 29
 testRunner.Then("I should see ok", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
