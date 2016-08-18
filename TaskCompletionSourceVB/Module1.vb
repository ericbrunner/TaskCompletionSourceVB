Imports System.Threading

Module Module1

    Sub Main()
        Dim pipeline As ProducerConsumerPipeline = New ProducerConsumerPipeline()

        Dim producer As Task = pipeline.StartProducer()
        Dim consumer As Task = pipeline.StartConsumer()

        Dim resultedTask As Task(Of Task) = Task.WhenAny(New List(Of Task) From {producer, consumer})


        resultedTask.ContinueWith(AddressOf Final)

        Console.WriteLine("VB.NET - Main thread waiting...")
        Console.ReadLine()
    End Sub

    Private Sub Final(completedTask As Task(Of Task))
        Console.WriteLine(SynchronizationContext.Current?.ToString())
        Dim faultedTask As Task = completedTask.Result
        Console.ForegroundColor = ConsoleColor.Green
        Console.WriteLine($"Completed Task in State:{faultedTask.Status}  with exception:")
        Console.ForegroundColor = ConsoleColor.Cyan
        Console.WriteLine($"{faultedTask.Exception?.Flatten()?.InnerException}")
    End Sub

End Module
