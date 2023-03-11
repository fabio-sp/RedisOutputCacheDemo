# OutputCache Demo Application

This is a demo repository to show how to integrate .NET 7 OutputCaching middleware with Redis Cache. Please do not use this code in production as it lacks of proper resource locking mechanisms.

## Prerequisites

Make sure you have an instance of Redis cache running locally. 

If you have Docker installed on your server you can spin up a new Redis Docker Container 

``docker run -p 6379:6379 redis:7.0.4-alpine``.

As Windows Redis Explorer during the demo I've used [Another Redis Desktop Manager](https://github.com/qishibo/AnotherRedisDesktopManager) which can be easily installed via choco or winget.

If you want to switch to the [FusionCache](https://github.com/ZiggyCreatures/FusionCache) implementation of the IOutputCacheStore you can take a look to the FusionOutputCacheStore.cs file and the commented lines in Program.cs file.
The FusionCache implementation uses Redis as a 2nd layer of caching.
