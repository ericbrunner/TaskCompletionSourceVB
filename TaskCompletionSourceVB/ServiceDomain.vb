Imports System.Threading

Public Class ServiceDomain

    Public Async Function StartPipelineAsync() As Task

        ' Uncomment these 2 statements to test any uncaught exception 
        'Await Task.Delay(3000).ConfigureAwait(False)
        'Throw New Exception("NO producer or consumer ex.")

        Dim pipeline As ProducerConsumerPipeline = New ProducerConsumerPipeline()

        Dim producer As Task = pipeline.StartProducer()
        Dim consumer As Task = pipeline.StartConsumer()

        Dim whenAnyCompletionTask As Task(Of Task) = Task.WhenAny(New List(Of Task) From {producer, consumer})

        ' Option1: return the completed task: either producer or consumer (preferred way)
        ' Comment next 4 statements to test Option2
        Dim producerOrConsumerTask = Await whenAnyCompletionTask.ConfigureAwait(False)
        Console.ForegroundColor = ConsoleColor.Yellow
        Console.WriteLine($"Fault Task is:{If(producerOrConsumerTask Is producer, "producer", "consumer")}")
        Await producerOrConsumerTask.ConfigureAwait(False)

        ' Option2: return the completion task: that is a Task(Of Task) type. 
        ' It contains the completed task which Is consumer or producer
        ' Comment to test Option1
        'Await whenAnyCompletionTask.ConfigureAwait(False)
    End Function

    Public Function StopPipeline() As Task
        Console.WriteLine("Service stopping...")
        Return Task.Delay(1000)
    End Function
End Class
