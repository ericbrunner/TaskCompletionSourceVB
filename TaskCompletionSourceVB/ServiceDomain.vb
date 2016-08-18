Imports System.Threading

Public Class ServiceDomain

    Public Function StartPipeline() As Task
        Dim pipeline As ProducerConsumerPipeline = New ProducerConsumerPipeline()

        Dim producer As Task = pipeline.StartProducer()
        Dim consumer As Task = pipeline.StartConsumer()

        Dim resultedTask As Task(Of Task) = Task.WhenAny(New List(Of Task) From {producer, consumer})

        Return resultedTask
    End Function

    Public Function StopPipeline() As Task
        Console.WriteLine("Service stopping...")
        Return Task.Delay(1000)
    End Function
End Class
