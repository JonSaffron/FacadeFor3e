3.0.1
-----
Added ColumnMappingAttribute to allow explicit linking of POCO fields/properties to columns returned by GetArchetypeData
Improved handling of Narratives within GetArchetypeData

3.0.0
-----
Removed functionality for GetArchetypeData to return data in a DataTable (new functionality is easier to use)
Added functionality for GetArchetypeData to return data as:
	- a scalar value (Boolean, Integer, DateTime, DateOnly, Decimal, Guid, String)
	- a List of scalar values
	- a compound value (a POCO whose fields/properties match the column names returned)
	- a list of compound values
Improved ProxyIdentityProvider to store credentials more securely

2.3.0
-----
Added functionality for GetArchetypeData to return data in a DataTable (this is usually easier to deal with than as an XmlDocument)

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
