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
namespace Phundus.Specs.Features
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Login")]
    public partial class LoginFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "Login.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("de-DE"), "Login", "In order to avoid silly mistakes\r\nAs a math idiot\r\nI want to be told the sum of t" +
                    "wo numbers", ProgrammingLanguage.CSharp, ((string[])(null)));
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
#line 6
#line 7
 testRunner.Given("ich bin nicht angemeldet", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Angenommen ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Unbekannter Benutzer")]
        [NUnit.Framework.CategoryAttribute("skip")]
        public virtual void UnbekannterBenutzer()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Unbekannter Benutzer", new string[] {
                        "skip"});
#line 10
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 11
 testRunner.Given("ich bin auf der Loginseite", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Angenommen ");
#line 12
 testRunner.And("ich tippe ins Feld \"E-Mail-Adresse\" \"gibt-es-nicht@test.phundus.ch\" ein", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 13
 testRunner.And("ich tippe ins Feld \"Passwort\" \"1234\" ein", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 14
 testRunner.When("ich auf \"Anmelden\" klicke", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Wenn ");
#line 15
 testRunner.Then("muss die Meldung \"Benutzername oder Passwort inkorrekt.\" erscheinen", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dann ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Erfolgreiches Login")]
        [NUnit.Framework.CategoryAttribute("skip")]
        public virtual void ErfolgreichesLogin()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Erfolgreiches Login", new string[] {
                        "skip"});
#line 18
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 19
 testRunner.Given("ich bin auf der Loginseite", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Angenommen ");
#line 20
 testRunner.And("ich tippe ins Feld \"E-Mail-Adresse\" \"user@test.phundus.ch\" ein", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 21
 testRunner.And("ich tippe ins Feld \"Passwort\" \"1234\" ein", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 22
 testRunner.When("ich auf \"Anmelden\" klicke", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Wenn ");
#line 23
 testRunner.Then("sollte ich als \"user@test.phundus.ch\" angemeldet sein", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dann ");
#line 24
 testRunner.And("ich sollte auf der Shopseite sein", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
