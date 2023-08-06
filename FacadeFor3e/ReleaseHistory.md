2.2.0
-----
Fixed a timing bug that caused a NullReferenceException
Added GetDataFromPresentation functionality (wraps GetDataFromReportLayout)
For .net 6 and later, date attributes now use DateOnly objects rather than DateTime objects
Improved logging for GetOption and GetServiceCulture services
Internal improvements to initialisation and disposal of the SOAP client

2.1.1
-----
Improve logging for GetArchetypeData calls
Fix potential null reference error in RenderDataErrors

2.1.0
-----
Improve functionality for attaching files (this is a change to the interface)

2.0.3
-----
Allow a nullable guid attribute to be added to operation
XML documentation file should now be included in the nuget package

2.0.2
----
Removed TrustTransfer example class from library that got accidentally copied into project
Improved logging

2.0.1
-----
Removed unnecessary dependency on JetBrains.Annotations (this is only required during development)
Update dependencies for .net 6
Update readme 

2.0.0
-----
Initial release offered through Nuget
