Imports System.Threading
Imports System.Collections.ObjectModel

Module Module1

    Sub Main()
        Dim serviceDomain As ServiceDomain = New ServiceDomain()

        ' start pipeline
        serviceDomain.StartPipeline().ContinueWith(Sub(pipelineTask As Task(Of Task))

                                                       Console.WriteLine(SynchronizationContext.Current?.ToString())
                                                       Dim faultedTask As Task = pipelineTask.Result

                                                       Console.ForegroundColor = ConsoleColor.Cyan

                                                       Dim innerExceptions As ReadOnlyCollection(Of Exception) = Nothing
                                                       innerExceptions = faultedTask.Exception?.Flatten()?.InnerExceptions

                                                       If (innerExceptions IsNot Nothing) Then
                                                           For Each innerException In innerExceptions
                                                               Console.WriteLine($"{innerException.ToString()}")
                                                           Next
                                                       End If
                                                   End Sub)


        Console.WriteLine("VB.NET - Main thread waiting...")
        Console.ReadLine()
    End Sub



End Module
