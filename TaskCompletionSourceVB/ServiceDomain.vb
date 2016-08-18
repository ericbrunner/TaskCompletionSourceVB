Imports System.Threading

Public Class ServiceDomain

    Public Function StartPipeline() As Task
        Dim pipeline As ProducerConsumerPipeline = New ProducerConsumerPipeline()

        Dim producer As Task = pipeline.StartProducer()
        Dim consumer As Task = pipeline.StartConsumer()

        Dim resultedTask As Task(Of Task) = Task.WhenAny(New List(Of Task) From {producer, consumer})


        resultedTask.ContinueWith(Sub(completedTask As Task(Of Task))
                                      Dim faultedTask = completedTask.Result
                                      Console.ForegroundColor = ConsoleColor.Red
                                      Console.WriteLine($"Completed Task in State:{faultedTask.Status}")

                                      If (faultedTask.IsFaulted) Then
                                          Throw faultedTask.Exception ' re-throw to be caught in main
                                      End If

                                  End Sub)
        Return resultedTask
    End Function
End Class
