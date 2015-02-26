// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

using System.Reflection;
using System.Windows.Markup;

[assembly: AssemblyTitle("BlueDwarf")]
[assembly: AssemblyDescription("UI for Blue Dwarf")]

// Why the hell does the following not work on VS2010? Too old?

[assembly: XmlnsDefinition("urn:blue-dwarf/view", "BlueDwarf.View")]
[assembly: XmlnsDefinition("urn:blue-dwarf/view", "BlueDwarf.Controls")]

[assembly: XmlnsDefinition("urn:blue-dwarf/view-model", "BlueDwarf.ViewModel")]

[assembly: XmlnsPrefix("urn:blue-dwarf/view-model", "vm")]
[assembly: XmlnsPrefix("urn:blue-dwarf/view", "v")]

#if DEBUG
[assembly: XmlnsDefinition("debug", "BlueDwarf")]
#endif
