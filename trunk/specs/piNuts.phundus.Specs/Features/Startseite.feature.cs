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
namespace Phundus.Specs.Features
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Startseite")]
    public partial class StartseiteFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "Startseite.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("de-DE"), "Startseite", "", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("Aufruf der Startseite")]
        [NUnit.Framework.CategoryAttribute("isSmoker")]
        public virtual void AufrufDerStartseite()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Aufruf der Startseite", new string[] {
                        "isSmoker"});
#line 4
this.ScenarioSetup(scenarioInfo);
#line 5
 testRunner.When("ich die Webseite aufrufe", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Wenn ");
#line 6
 testRunner.Then("sollte ich ein Heading 1 mit \"\'Allo, \'Allo!\" sehen", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dann ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Aufruf des Shops")]
        [NUnit.Framework.CategoryAttribute("isSmoker")]
        public virtual void AufrufDesShops()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Aufruf des Shops", new string[] {
                        "isSmoker"});
#line 9
this.ScenarioSetup(scenarioInfo);
#line 10
 testRunner.When("ich den Shop aufrufe", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Wenn ");
#line 11
 testRunner.Then("sollte ich gross \"Shop\" sehen", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dann ");
#line 12
 testRunner.And("sollte im Fenstertitel muss \"Shop - phundus\" stehen", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Korrekte Version wurde installiert")]
        [NUnit.Framework.CategoryAttribute("isSmoker")]
        public virtual void KorrekteVersionWurdeInstalliert()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Korrekte Version wurde installiert", new string[] {
                        "isSmoker"});
#line 15
this.ScenarioSetup(scenarioInfo);
#line 16
 testRunner.When("ich den Shop aufrufe", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Wenn ");
#line 17
 testRunner.Then("sollte die Version entsprechend der zuletzt installierten Version sein", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dann ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Server-URL wurde hinterlegt")]
        [NUnit.Framework.CategoryAttribute("isSmoker")]
        public virtual void Server_URLWurdeHinterlegt()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Server-URL wurde hinterlegt", new string[] {
                        "isSmoker"});
#line 20
this.ScenarioSetup(scenarioInfo);
#line 21
 testRunner.When("ich den Shop aufrufe", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Wenn ");
#line 22
 testRunner.Then("sollte die Server-URL entsprechend der Konfiguration gesetzt sein", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dann ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
