using System;
using System.Diagnostics;
using System.Threading;
using Confluent.Kafka;
using System.IO;
using System.Collections.Generic;

class Program
{
    public static void Main(string[] args)
    {
        var conf = new ConsumerConfig
        { 
            GroupId = "test-consumer-group",
            BootstrapServers = "localhost:9092",
            // Note: The AutoOffsetReset property determines the start offset in the event
            // there are not yet any committed offsets for the consumer group for the
            // topic/partitions of interest. By default, offsets are committed
            // automatically, so in this example, consumption will only start from the
            // earliest message in the topic 'my-topic' the first time you run the program.
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using (var c = new ConsumerBuilder<Ignore, string>(conf).Build())
        {
            c.Subscribe("my-topic");

            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) => {
                e.Cancel = true; // prevent the process from terminating.
                cts.Cancel();
            };

            try
            {
                int? processId = null;                        
                while (true)
                {
                    try
                    {
                        var cr = c.Consume(cts.Token);
                        if(cr.Value == "start") {
                            Process process = new Process();
                            process.StartInfo.FileName = Path.GetFullPath("../controlled-app/hello"); 
                            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;                           
                            // Configure the process using the StartInfo properties.
                            process.Start();
                            processId = process.Id;
                            Console.WriteLine("Entered start");

                        }

                        if(cr.Value == "stop") {
                            Console.WriteLine("Entered stop");
                            if(processId != null){
                                Process.GetProcessById((int)processId).Kill();
                                Console.WriteLine("controlled-app exited");
                            }
                        }
                        // Console.WriteLine($"Consumed message '{cr.Value}' at: '{cr.TopicPartitionOffset}'.");
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Error occured: {e.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Ensure the consumer leaves the group cleanly and final offsets are committed.
                c.Close();
            }
        }
    }
}