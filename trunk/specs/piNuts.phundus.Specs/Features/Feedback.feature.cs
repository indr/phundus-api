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
    [NUnit.Framework.DescriptionAttribute("Feedback")]
    public partial class FeedbackFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "Feedback.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("de-DE"), "Feedback", "Um den machern von phundus etwas mitzuteilen\r\nAls ein Benutzer\r\nWill ich ein Feed" +
                    "backformular ausfüllen", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("Feedback ohne Angabe der E-Mail-Adresse")]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void FeedbackOhneAngabeDerE_Mail_Adresse()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Feedback ohne Angabe der E-Mail-Adresse", new string[] {
                        "ignore"});
#line 11
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 12
 testRunner.Given("ich bin auf der Feedbackseite", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Angenommen ");
#line 13
 testRunner.And("ich tippe ins Feld \"Feedback\" \"Hallo\" ein", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 14
 testRunner.When("ich auf \"Senden\" klicke", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Wenn ");
#line 15
 testRunner.Then("muss die Meldung \"Das Feld \"E-Mail-Adresse\" ist erforderlich.\" erscheinen", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dann ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Feedback ohne Angabe des Feedbacks")]
        public virtual void FeedbackOhneAngabeDesFeedbacks()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Feedback ohne Angabe des Feedbacks", ((string[])(null)));
#line 18
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 19
 testRunner.Given("ich bin auf der Feedbackseite", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Angenommen ");
#line 20
 testRunner.And("ich tippe ins Feld \"E-Mail-Adresse\" \"user@example.com\" ein", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 21
 testRunner.When("ich auf \"Senden\" klicke", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Wenn ");
#line 22
 testRunner.Then("muss die Meldung \"Das Feld \"Feedback\" ist erforderlich.\" erscheinen", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dann ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Feedback senden")]
        [NUnit.Framework.CategoryAttribute("isSmoker")]
        public virtual void FeedbackSenden()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Feedback senden", new string[] {
                        "isSmoker"});
#line 26
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 27
 testRunner.Given("ich bin auf der Feedbackseite", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Angenommen ");
#line 28
 testRunner.And("ich tippe ins Feld \"E-Mail-Adresse\" \"user@test.phundus.ch\" ein", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line hidden
#line 29
 testRunner.And("ich füge ins Feld \"Feedback\" ein:", "Grüsse vom Feedback-Feature-Szenario \"Feedback senden\"!\r\n\r\nServer-Url: {AppSettin" +
                    "gs.ServerUrl}\r\nVersion: {Assembly.Version}", ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 36
 testRunner.When("ich auf \"Senden\" klicke", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Wenn ");
#line 37
 testRunner.Then("muss die Meldung \"Merci!\" erscheinen", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dann ");
#line hidden
#line 38
 testRunner.And("muss \"user@test.phundus.ch\" ein E-Mail erhalten mit dem Betreff \"Vielen Dank fürs" +
                    " Feedback\" und dem Text:", @"Wir haben dein Feedback erhalten und werden dir baldmöglichst darauf antworten.

Vielen Dank und freundliche Grüsse

Das phundus-Team

--
This is an automatically generated message from phundus.
-
If you think it was sent incorrectly contact the administrators at lukas.mueller@phundus.ch or reto.inderbitzin@phundus.ch.", ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line 51
 testRunner.And("muss \"admin@test.phundus.ch\" ein E-Mail erhalten mit dem Betreff \"[phundus] Feedb" +
                    "ack\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Und ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
