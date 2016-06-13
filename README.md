# csvdb-studies [![Build status](https://ci.appveyor.com/api/projects/status/nr5hsfb0hdbw6vpk/branch/master?svg=true)](https://ci.appveyor.com/project/wallymathieu/csvdb-studies/branch/master) [![Build Status](https://travis-ci.org/wallymathieu/csvdb-studies.svg?branch=master)](https://travis-ci.org/wallymathieu/csvdb-studies)

An example of using a csv file as a database and a very simplistic session implementation on top of that. It does not handle a million records well. This implementation uses a read only approach to the data files. 

## Why would you ever use this kind of thing?

You are starting out with a new application. There is really [no need to drag a database](https://blog.8thlight.com/uncle-bob/2012/05/15/NODB.html) into your application. 

There are many applications that that does not contain a lot of data. Having a database increases cost and complexity (and make it harder to deploy your application).

## When should you avoid using this kind thing

 - You have a lot of data (millions of entities) and want to query that data.
 - You need to do distribute the data in several database nodes (for instance to ensure availability).
 - You need to handle writes from multiple threads
