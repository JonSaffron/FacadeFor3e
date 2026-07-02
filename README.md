Facade for 3E
=============

A small library which can be used to simplify the task of integrating with on premises versions of Elite 3E.

The Transaction Service is a SOAP based API and you should be aware that it is *only* available for on premises environments and is likely to be phased out over the next few years.

This library provides a wrapper around the service to make creating requests and interpreting responses straightforward and therefore quicker to write.

You can query the 3E database, or run a 3E process from within your own .net application to update data.

For Transaction Services you can:
* Get data using an xoql query
* Run processes to create, update and delete data 
* Add an attachment (any file such as a document or an email) to a record
* Get the effective setting of a system option/override

When using the Transaction Service, all the various individual services are accessed in the context of a specific user account which 
must be known to the 3E environment (i.e. there must be an active user record in 3E for that acccount).
Just like when using the front-end, the account is used to determine what security applies and what system options are in affect.

When using the library, the user account is typically specified using a NetworkCredential object (allowing the credentials to be stored
as part of an application's configuration). Alternatively you can specify a WindowsIdentity object (can be used when the application is
browser-based and enforces Windows authentication). If neither option is used, then services will be executed in the
context of the currently logged in user.

Compatible with:
* .net framework 4.81
* .net standard 2.0 
* .net 8
* .net 10
