# How to run ?

After installing and setting up docker and docker-compose - run:

```
docker-compose up
```

This should bring zookeper and kafka services up. Now go to master and slave folders and issue ``` dotnet run ``` to run initial demo. You should be able to see messages being dispathced by master and being received by slave through kafka.