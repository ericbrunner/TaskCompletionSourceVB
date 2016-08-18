

Imports System.Threading

Public Class ProducerConsumerPipeline

    Public Function StartConsumer() As Task

        Dim tcs = New TaskCompletionSource(Of Object)()


        Task.Run(Sub()

                     Try
                         Thread.Sleep(3000)
                         'throw New Exception("Consumer ex.");

                         Dim ex As Exception = New Exception("Consumer ex.")

                         tcs.SetException(ex)

                     Catch e As Exception

                         Console.WriteLine(SynchronizationContext.Current?.ToString())
                         tcs.SetException(e)
                         Console.WriteLine(SynchronizationContext.Current?.ToString())
                     End Try

                 End Sub)

        Return tcs.Task
    End Function

    Public Function StartProducer() As Task

        Dim tcs = New TaskCompletionSource(Of Object)()

        Task.Run(Sub()
                     Try

                         Thread.Sleep(6000)
                         Throw New Exception("Producer ex.")

                     Catch e As Exception

                         Console.WriteLine(SynchronizationContext.Current?.ToString())
                             tcs.SetException(e)
                             Console.WriteLine(SynchronizationContext.Current?.ToString())
                         End Try

                     End Sub)

        Return tcs.Task
    End Function

End Class
