﻿using System;
using System.Threading;
using Confluent.Kafka;

class Program
{
    public static void Main(string[] args)
    {
        var conf = new ProducerConfig { BootstrapServers = "localhost:9092" };

        Action<DeliveryReport<Null, string>> handler = r => 
            Console.WriteLine(!r.Error.IsError
                ? $"Delivered message to {r.TopicPartitionOffset}"
                : $"Delivery Error: {r.Error.Reason}");

        using (var p = new ProducerBuilder<Null, string>(conf).Build())
        {
            // initiate cpp process
            p.BeginProduce("my-topic", new Message<Null, string> { Value = "start" }, handler);
            // wait for up to 10 seconds for any inflight messages to be delivered.
            p.Flush(TimeSpan.FromSeconds(10));

            Thread.Sleep(3000);
            p.BeginProduce("my-topic", new Message<Null, string> { Value = "stop" }, handler);


        }
    }
}