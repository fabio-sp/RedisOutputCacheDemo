# OutputCache Demo Application

This is a demo repository to show how to integrate .NET 7 OutputCaching middleware with Redis Cache

## Prerequisites

Make sure you have an instance of Redis cache running locally. 

If you have Docker installed on your server you can spin up a new Redis Docker Container 

``docker run -p 6379:6379 redis:7.0.4-alpine``.

As Windows Redis Explorer during the demo I've used [Another Redis Desktop Manager](https://github.com/qishibo/AnotherRedisDesktopManager) which can be easily installed via choco or winget.