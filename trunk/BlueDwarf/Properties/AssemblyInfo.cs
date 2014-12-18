using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Markup;

[assembly: AssemblyTitle("BlueDwarf")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Microsoft")]
[assembly: AssemblyProduct("BlueDwarf")]
[assembly: AssemblyCopyright("Copyright © None Corp. 2014")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]

[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

// Why the hell does the following not work on VS2010? Too old?

[assembly: XmlnsDefinition("urn:blue-dwarf/view", "BlueDwarf.View")]
[assembly: XmlnsDefinition("urn:blue-dwarf/view", "BlueDwarf.Controls")]

[assembly: XmlnsDefinition("urn:blue-dwarf/view-model", "BlueDwarf.ViewModel")]

[assembly: XmlnsPrefix("urn:blue-dwarf/view-model", "vm")]
[assembly: XmlnsPrefix("urn:blue-dwarf/view", "v")]
