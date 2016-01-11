﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Phundus.IdentityAccess.Organizations.Mails {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Templates {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Templates() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Phundus.IdentityAccess.Organizations.Mails.Templates", typeof(Templates).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;p&gt;Hallo @Model.User.FirstName&lt;/p&gt;
        ///
        ///&lt;p&gt;Infolge unsachgemässer Verwendung wurde dein Zugriff auf die Materialverleihanwendung phundus gesperrt. Falls dir dieser Schritt ungerecht erscheint kannst du dich gerne an reto.inderbitzin@phundus.ch oder lukas.mueller@phundus.ch wenden. Beachte, dass bei Mitgliedschaftsanfragen deine Kontaktdaten mit anderen Adressdatenbanken der angefragten Organisationen verglichen werden. Absichtlich erkennbare Falschangaben können zu einer Sperrung deines Kontos führen. Nach ei [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string MemberLockedBodyHtml {
            get {
                return ResourceManager.GetString("MemberLockedBodyHtml", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Zugriff gesperrt.
        /// </summary>
        internal static string MemberLockedSubject {
            get {
                return ResourceManager.GetString("MemberLockedSubject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;p&gt;Hallo @Model.User.FirstName&lt;/p&gt;
        ///
        ///&lt;p&gt;Soweit alles okay. Die @Model.Organization.Name hat deine Zugehörigkeit geprüft und bestätigt dir, dass du ihr Material ausleihen darfst. Über www.phundus.ch kannst du nun auch die Materialien der @Model.Organization.Name in deinen Warenkorb legen und für deinen Event reservieren.&lt;/p&gt;.
        /// </summary>
        internal static string MembershipApplicationApprovedBodyHtml {
            get {
                return ResourceManager.GetString("MembershipApplicationApprovedBodyHtml", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Mitgliedschaft bei @Model.Organization.Name bestätigt.
        /// </summary>
        internal static string MembershipApplicationApprovedSubject {
            get {
                return ResourceManager.GetString("MembershipApplicationApprovedSubject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;p&gt;Hallo&lt;/p&gt;
        ///
        ///&lt;p&gt;@Model.User.FirstName  @Model.User.LastName hat die Mitgliedschaft bei deiner Organisation @Model.Organization.Name beantragt. Bestätige oder lehne die Anfrage unter Verwaltung/Beitrittsanfrage ab.&lt;/p&gt;.
        /// </summary>
        internal static string MembershipApplicationFiledBodyHtml {
            get {
                return ResourceManager.GetString("MembershipApplicationFiledBodyHtml", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Mitgliedschaft bei @Model.Organization.Name beantragt.
        /// </summary>
        internal static string MembershipApplicationFiledSubject {
            get {
                return ResourceManager.GetString("MembershipApplicationFiledSubject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;p&gt;Hallo @Model.User.FirstName&lt;/p&gt;
        ///
        ///&lt;p&gt;Die @Model.Organization.Name scheint dich nicht identifizieren zu können. Allenfalls fehlt eine entsprechende Identifikationsnummer der Organisation oder du bist noch nicht in Ihrer Mitgliederdatenbank registriert. Sollte die Ablehnung für dich nicht nachvollziehbar sein kontaktiere deine für die Mitgliedschaften verantwortliche Kontaktperson der Organisation.&lt;/p&gt;
        ///
        ///&lt;p&gt;Wir bitten dich wahllose Anfragen zu unterlassen und nur jene Organisationen nach ihrem phundus an [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string MembershipApplicationRejectedBodyHtml {
            get {
                return ResourceManager.GetString("MembershipApplicationRejectedBodyHtml", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Mitgliedschaft bei @Model.Organization.Name abgelehnt.
        /// </summary>
        internal static string MembershipApplicationRejectedSubject {
            get {
                return ResourceManager.GetString("MembershipApplicationRejectedSubject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;p&gt;Hallo @Model.User.FirstName&lt;/p&gt;
        ///
        ///&lt;p&gt;Das hat geklappt. Dein phundus Benutzerkonto mit der E-Mail Adresse @Model.User.Email wurde soeben wieder entsperrt. Du kannst nun von deinen Organisationen wieder Material reservieren.&lt;/p&gt;.
        /// </summary>
        internal static string MemberUnlockedBodyHtml {
            get {
                return ResourceManager.GetString("MemberUnlockedBodyHtml", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Zugriff entsperrt.
        /// </summary>
        internal static string MemberUnlockedSubject {
            get {
                return ResourceManager.GetString("MemberUnlockedSubject", resourceCulture);
            }
        }
    }
}
