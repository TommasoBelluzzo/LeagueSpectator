#region Using Directives
using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
#endregion

#region Information
[assembly: AssemblyDescription("A simple utility for spectating League of Legends matches.")]
[assembly: AssemblyCompany("Tommaso Belluzzo")]

#if (DEBUG)
[assembly: AssemblyConfiguration("Debug Build")]
#else
[assembly: AssemblyConfiguration("Release Build")]
#endif

[assembly: AssemblyCopyright("Copyright ©2018 Tommaso Belluzzo")]
[assembly: AssemblyCulture("")]
[assembly: AssemblyProduct("LeagueSpectator")]
[assembly: AssemblyTitle("LeagueSpectator")]
[assembly: AssemblyTrademark("")]
#endregion

#region Settings
[assembly: CLSCompliant(false)]
[assembly: ComVisible(false)]
[assembly: Guid("C808CBFE-1AE7-4BB6-91F7-15C2D1EB8BB2")]
[assembly: NeutralResourcesLanguage("en")]
#endregion

#region Version
[assembly: AssemblyFileVersion("1.1.0.0")]
[assembly: AssemblyInformationalVersion("1.1.0.0")]
[assembly: AssemblyVersion("1.1.0.0")]
#endregion