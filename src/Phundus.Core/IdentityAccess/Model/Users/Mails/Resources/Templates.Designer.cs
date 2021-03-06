﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Phundus.IdentityAccess.Model.Users.Mails.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Phundus.IdentityAccess.Model.Users.Mails.Resources.Templates", typeof(Templates).Assembly);
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
        ///   Looks up a localized string similar to &lt;p&gt;Hallo @Model.FirstName&lt;/p&gt;
        ///
        ///&lt;p&gt;Deine phundus-Account mit der E-Mail-Adresse @Model.EmailAddress wurde vorübergehend gesperrt.&lt;/p&gt;
        ///
        ///&lt;p&gt;Bei Fragen zur Blockierung oder Bedingungen für eine Freischaltung solltest du dich an deine zugeteilte Organisation wenden.&lt;/p&gt;..
        /// </summary>
        internal static string AccountLockedHtml {
            get {
                return ResourceManager.GetString("AccountLockedHtml", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Account gesperrt.
        /// </summary>
        internal static string AccountLockedSubject {
            get {
                return ResourceManager.GetString("AccountLockedSubject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;p&gt;Hallo @Model.FirstName&lt;/p&gt;
        ///
        ///&lt;p&gt; Deine phundus-Account mit der E-Mail.Adresse @Model.EmailAddress, wurde soeben wieder freigeschalten. Du kannst über die Anwendung nun wieder Material deiner Organisation reservieren.&lt;/p&gt;.
        ///
        ///&lt;p&gt;Viel Spass!&lt;/p&gt;.
        /// </summary>
        internal static string AccountUnlockedHtml {
            get {
                return ResourceManager.GetString("AccountUnlockedHtml", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Account freigeschaltet.
        /// </summary>
        internal static string AccountUnlockedSubject {
            get {
                return ResourceManager.GetString("AccountUnlockedSubject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;p&gt;Hallo @Model.FirstName&lt;/p&gt;
        ///
        ///&lt;p&gt;Du hast dich bei der Materialverleihanwendung phundus angemeldet. Bitte bestätige deine E-Mail Adresse durch das anwählen des nachfolgenden Linkes:&lt;br /&gt;&lt;a href=&quot;@Model.Urls.AccountValidation(@Model.ValidationKey)&quot;&gt;@Model.Urls.AccountValidation(@Model.ValidationKey)&lt;/a&gt;&lt;/p&gt;
        ///
        ///&lt;p&gt;Sollte deinen Webbrowser, diese Webseite nicht korrekt anzeigen kannst du auch @Model.Urls.AccountValidation() in der Adresszeile eingeben und deine Mailadresse mit der Eingabe dieses Codes bestä [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string AccountValidationHtml {
            get {
                return ResourceManager.GetString("AccountValidationHtml", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Validierung der E-Mail-Adresse.
        /// </summary>
        internal static string AccountValidationSubject {
            get {
                return ResourceManager.GetString("AccountValidationSubject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;p&gt;Hallo @Model.FirstName&lt;/p&gt;
        ///
        ///&lt;p&gt;Du hast deine E-Mailadresse bei der Materialverleihanwendung phundus geändert. Bitte bestätige deine E-Mail Adresse durch das anwählen des nachfolgenden Linkes:&lt;br /&gt;&lt;a href=&quot;@Model.Urls.EmailAddressValidation(@Model.ValidationKey)&quot;&gt;@Model.Urls.EmailAddressValidation(@Model.ValidationKey)&lt;/a&gt;&lt;/p&gt;
        ///
        ///&lt;p&gt;Sollte deinen Webbrowser, diese Webseite nicht korrekt anzeigen kannst du auch @Model.Urls.EmailAddressValidation() in der Adresszeile eingeben und deine Mailadresse mit de [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string EmailAddressValidationHtml {
            get {
                return ResourceManager.GetString("EmailAddressValidationHtml", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Validierung der geänderten E-Mail-Adresse.
        /// </summary>
        internal static string EmailAddressValidationSubject {
            get {
                return ResourceManager.GetString("EmailAddressValidationSubject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;p&gt;Hallo @Model.FirstName&lt;/p&gt;
        ///
        ///&lt;p&gt;Neues Passwort ist @Model.Password.&lt;/p&gt;.
        /// </summary>
        internal static string NewPasswordHtml {
            get {
                return ResourceManager.GetString("NewPasswordHtml", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Neues Passwort.
        /// </summary>
        internal static string NewPasswordSubject {
            get {
                return ResourceManager.GetString("NewPasswordSubject", resourceCulture);
            }
        }
    }
}
