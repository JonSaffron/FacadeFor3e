4.0.3
-----
Bump the versions of some dependencies
Get rid of warnings caused by security vulnerabilities in transitive dependencies
Internal improvements to logging to NLog

4.0.2
-----
Fixed a bug to always set ExecuteProcessResult where an ExecuteProcessException was raised
Fixed a bug in the JSon converters to use 24 hour clock
Updated some package dependencies - some transitive dependencies are marked vulnerable, but the affected code is not used
Added direct support for .net 8 and .net standard 2.1

4.0.1
-----
Added OutputIdsThatIndicateFailure and DefaultOutputIdsThatIndicateFailure properties to ExecuteProcessOptions
Improved handling of responses from Transaction Services and the reporting of error conditions
Updated versions of dependent nuget packages, including System.Text.Json which had a reported vulnerability

4.0.0
-----
Added AddOrReplace to AttributeCollection to serve a particular use case

4.0.0-rc3
---------
Change the ODataServiceResult.ErrorMesssages property to make it less brittle and enable it to cope with errors caused by rate limiting
Updated the json deserialiser to be case-insensitive and to cache the options object
Removed some over zealous checks that the response from the OData service was in JSON format - for 404 errors it won't be

4.0.0-rc2
---------
Update to support new syntax for specifying attribute values by alias
Add ODataServices.Execute overload to alllow a process to be called without a ProcessCommand
Add Value property to ODataServiceResult to make it easier to consume results from Select requests
Improve code analysis for consumers of the library

4.0.0-rc1
---------
Introduces an interface for the 3E OData service
Changes to the ExecuteProcessService for thread safety
Internal changes and updates

3.0.2
-----
Fix to get GetArchetype.GetScalarList working
Fix to parsing dates into DateOnly fields/properties in GetArchetypeData
Improve error messages raised in GetArchetypeData

3.0.1
-----
Added ColumnMappingAttribute to allow explicit linking of POCO fields/properties to columns returned by GetArchetypeData
Improved handling of Narratives within GetArchetypeData
*** GetArchetype.GetScalarList is broken in this version ***

3.0.0
-----
Removed functionality for GetArchetypeData to return data in a DataTable (new functionality is easier to use)
Added functionality for GetArchetypeData to return data as:
	- a scalar value (Boolean, Integer, DateTime, DateOnly, Decimal, Guid, String)
	- a List of scalar values
	- a compound value (a POCO whose fields/properties match the column names returned)
	- a list of compound values
Improved ProxyIdentityProvider to store credentials more securely
*** GetArchetype.GetScalarList is broken in this version ***

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
