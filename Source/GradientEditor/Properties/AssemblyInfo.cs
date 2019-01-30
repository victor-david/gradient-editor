/**
 * Restless Animal Development
 * This program is provided freely without restrictions, but please retain this notice in any distributions, modifications ,etc.
 * https://restlessanimal.com
 */
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Markup;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Wpf Gradient Editor")]
[assembly: AssemblyDescription("Provides visual construction and preview of various linear gradient brushes for use in a WPF application.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Restless Animal")]
[assembly: AssemblyProduct("Wpf Gradient Editor")]
[assembly: AssemblyCopyright("Restless Animal 2018, freely available to all")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

//In order to begin building localizable applications, set 
//<UICulture>CultureYouAreCodingWith</UICulture> in your .csproj file
//inside a <PropertyGroup>.  For example, if you are using US english
//in your source files, set the <UICulture> to en-US.  Then uncomment
//the NeutralResourceLanguage attribute below.  Update the "en-US" in
//the line below to match the UICulture setting in the project file.

//[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.Satellite)]

[assembly: ThemeInfo(
    // Location of theme specific resource dictionaries,
    // used if a resource is not found in the page, or in application resource dictionaries.
    ResourceDictionaryLocation.SourceAssembly,
    // Location of the generic resource dictionary,
    // used if a resource is not found in the page, app, or any theme specific resource dictionaries.
    ResourceDictionaryLocation.SourceAssembly
)]


[assembly: XmlnsPrefix("http://schemas.restless.com/wpf/apps/xaml/controls", "lc")]
[assembly: XmlnsDefinition("http://schemas.restless.com/wpf/apps/xaml/controls", "Restless.App.GradientEditor.Controls")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("3.0.1.0")]
[assembly: AssemblyFileVersion("3.0.1.0")]
[assembly: GuidAttribute("98300783-db00-4255-80c2-0d3af1a893dc")]
