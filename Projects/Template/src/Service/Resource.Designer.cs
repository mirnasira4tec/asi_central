﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace asi.asicentral {
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
    public class Resource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("asi.asicentral.Resource", typeof(Resource).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This field is required.
        /// </summary>
        public static string FieldRequired {
            get {
                return ResourceManager.GetString("FieldRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The name cannot be more than 50 characters.
        /// </summary>
        public static string NameLength {
            get {
                return ResourceManager.GetString("NameLength", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Publication Identifier.
        /// </summary>
        public static string PublicationId {
            get {
                return ResourceManager.GetString("PublicationId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Identifier.
        /// </summary>
        public static string PublicationIssueId {
            get {
                return ResourceManager.GetString("PublicationIssueId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Issue Name.
        /// </summary>
        public static string PublicationIssueName {
            get {
                return ResourceManager.GetString("PublicationIssueName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Publication Name.
        /// </summary>
        public static string PublicationName {
            get {
                return ResourceManager.GetString("PublicationName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enter the Publication Name....
        /// </summary>
        public static string PublicationPrompt {
            get {
                return ResourceManager.GetString("PublicationPrompt", resourceCulture);
            }
        }
    }
}
