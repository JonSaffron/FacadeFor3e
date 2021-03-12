Facade for 3E
=============

A small library which can be used to simplify the task of interacting with Elite 3E.

3E provides a TransactionService web service which you can use to run a particular process, but it's not particularly easy to use. This library provides a wrapper around the web service making creating request and interpreting responses straightforward and therefore quicker to write.

Run any 3E process from within your own .net application, so that you create your own matter opening website, generate proformas, or update conflicts information.

At present it offers the following functions:
* Run processes to create, update and delete data 
* Add an attachment (any file such as a document or an email) to a record
* Get data using xoql
* Get the effective setting of a system option/override

Compatible with:
* .net framework 4.8
* .net standard 2.0
* .net 5.0
