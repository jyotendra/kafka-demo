# How to run ?

After installing and setting up docker and docker-compose - run:

```
docker-compose up
```

This should bring zookeper and kafka services up. 

If docker-compose starts Kafka correctly, you will get to see running logs as shown below:
[kafka_logs](https://flic.kr/p/2egd62W)

Now go to master and slave folders and issue ``` dotnet run ``` to run initial demo. Of course you need to install "dotnet core" on your system. You should be able to see messages being dispathced by master and being received by slave through kafka.

* On receiving "start" message from "master" - "slave" will invoke the "hello world" program in C++, which will print "Hello World" in an infinite loop.
* On receiving "stop" message from "master" - "slave" will end the process.

[running messages](https://flic.kr/p/RALHqt)
