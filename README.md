Facade for 3E
=============

A small library which can be used to simplify the task of integrating with Elite 3E.

There are two different interfaces into Elite 3E:
- The Transaction Service is the original interface and is a SOAP based API. It is only available for on premises environments and is likely to be phased out over the next few years.
- The OData service is a partial replacement and is available (when installed) for on premises environments and is the only interface available for cloud hosted environments.

This library provides a wrapper around both services to make creating requests and interpreting responses straightforward and therefore quicker to write.

You can query the 3E database, or run a 3E process from within your own .net application to update data.

For Transaction Services you can:
* Get data using an xoql query
* Run processes to create, update and delete data 
* Add an attachment (any file such as a document or an email) to a record
* Get the effective setting of a system option/override

For OData you can:
* Get data through an OData GET request
* Update/Insert/Delete data by running a 3E process 

For on premises hosted environments, all services are run in the context of a specific user which 
must be known to the 3E environment being targetted and which is used to determine what process level and
row-level security applies and what system options are set - just as it would through the normal front-end.
A user can be specified using a NetworkCredential object (typically where the credentials are stored as part of an
application's configuration), or by passing in a WindowsIdentity object (can be used when the application is
browser-based and enforces Windows authentication). If neither option is used, then services will be executed in the
context of the currently logged in user.

For cloud hosted environments, the OData service will run in the context of a specific 3E user.

Compatible with:
* .net framework 4.81
* .net standard 2.0 & 2.1
* .net 6
* .net 8
