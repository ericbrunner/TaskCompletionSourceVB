

Imports System.Threading

Public Class ProducerConsumerPipeline

    Public Function StartConsumer() As Task

        Dim tcs = New TaskCompletionSource(Of Object)()


        Task.Run(Sub()

                     Thread.Sleep(4000)

                     Dim ex As Exception = New Exception("Consumer ex.")
                     tcs.SetException(ex)

                 End Sub)

        Return tcs.Task
    End Function

    Public Function StartProducer() As Task

        Dim tcs = New TaskCompletionSource(Of Object)()

        Task.Run(Sub()

                     Thread.Sleep(5000)

                     Dim ex As Exception = New Exception("Producer ex.")
                     tcs.SetException(ex)

                 End Sub)

        Return tcs.Task
    End Function

End Class
