Facade for 3E
=============

A small library which can be used to simplify the task of interacting with Elite 3E.

3E provides a TransactionService web service which you can use to run a particular process, but it's not particularly easy to use. This library provides a wrapper around the web service making creating request and interpreting responses straightforward and therefore quicker to write.

Run any 3E process from within your own .net application, so that you create your own matter opening website, generate proformas, or update conflicts information.

At present it offers the following functions:
* Run processes to create, update and delete data 
* Add an attachment (any file such as a document or an email) to a record
* Get data using an xoql query
* Get the effective setting of a system option/override

Each service exposed through the TransactionService must be run in the context of a specific user which 
must be known to the 3E environment being targetted and which is used to determine what process level and
row-level security applies and what system options are set - just as it would through the normal front-end.
A user can be specified using a NetworkCredential object (typically where the credentials are stored as part of an
application's configuration), or by passing in a WindowsIdentity object (can be used when the application is
browser-based and enforces Windows authentication). If neither option is used, then services will be executed in the
context of the currently logged in user.

Compatible with:
* .net framework 4.8
* .net standard 2.0
* .net 6.0
