Imports System.Threading
Imports System.Collections.ObjectModel
Imports Topshelf

Module Module1

    Sub Main()
        Dim serviceDomain As ServiceDomain = New ServiceDomain()




        Console.WriteLine("VB.NET - Service started")
        Dim serviceState As TopshelfExitCode = HostFactory.Run(
            Sub(hostConfiguration)

                hostConfiguration.Service(Of ServiceDomain)(
                Sub(serviceConfiguration)
                    ' construct our service domain
                    serviceConfiguration.ConstructUsing(Function() New ServiceDomain())

                    ' start
                    serviceConfiguration.WhenStarted(
                    Sub(service)
                        ' start pipeline

                        service.StartPipeline().ContinueWith(
                        Sub(pipelineTask As Task(Of Task))

                            Console.WriteLine(SynchronizationContext.Current?.ToString())
                            Dim faultedTask As Task = pipelineTask.Result

                            Console.ForegroundColor = ConsoleColor.Red
                            Console.WriteLine($"Completed Task in State:{faultedTask.Status}")

                            Console.ForegroundColor = ConsoleColor.Cyan

                            Dim innerExceptions As ReadOnlyCollection(Of Exception) = Nothing
                            innerExceptions = faultedTask.Exception?.Flatten()?.InnerExceptions

                            If (innerExceptions IsNot Nothing) Then
                                For Each innerException In innerExceptions
                                    Console.WriteLine($"{innerException.ToString()}")
                                Next
                            End If

                            Environment.Exit(-1)
                        End Sub)
                    End Sub)

                    ' stop
                    serviceConfiguration.WhenStopped(
                    Sub(service)
                        ' stop pipeline

                        Try
                            service.StopPipeline().Wait()
                        Catch ex As Exception
                            Console.WriteLine("Exception occured while stopping pipeline.")
                            Console.WriteLine(ex)
                        End Try
                    End Sub)
                End Sub)

                ' run service as LocalSysten
                hostConfiguration.RunAsLocalSystem()

                hostConfiguration.SetDescription("Sample Topshelf Host for ServiceDomain")
                hostConfiguration.SetDisplayName("ServiceDomain")
                hostConfiguration.SetServiceName("ServiceDomain")
            End Sub)

        Console.WriteLine($"VB.NET - Service exited with Exitcode: {serviceState}")

        Console.ReadLine()
    End Sub



End Module
