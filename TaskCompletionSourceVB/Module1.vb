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

                        service.StartPipelineAsync().ContinueWith(
                        Sub(pipelineTask As Task)
                            Console.ForegroundColor = ConsoleColor.Black
                            Console.WriteLine(SynchronizationContext.Current?.ToString())

                            'Uncomment for Option2 (if you can't to get the completed task): get the producer or consumer task out of that WhenAny completion task
                            'Dim finalTask As Task = pipelineTask.Result

                            ' Comment to test Option 2 if you want to get the inner completed task whic his producer or consumer
                            Dim finalTask As Task = pipelineTask

                            If (finalTask.IsFaulted) Then
                                Console.ForegroundColor = ConsoleColor.Red
                                Console.WriteLine($"Completed Task in State:{finalTask.Status}")

                                Console.ForegroundColor = ConsoleColor.Cyan

                                Dim innerExceptions As ReadOnlyCollection(Of Exception) = Nothing
                                innerExceptions = finalTask.Exception?.Flatten()?.InnerExceptions

                                If (innerExceptions IsNot Nothing) Then
                                    For Each innerException In innerExceptions
                                        Console.WriteLine($"{innerException.ToString()}")
                                    Next
                                End If

                                Console.ForegroundColor = ConsoleColor.White
                                Console.WriteLine("Exit with Code: -1")
                                Environment.Exit(-1)

                            Else
                                Console.ForegroundColor = ConsoleColor.Green
                                Console.WriteLine($"Completed Task in State:{finalTask.Status}")
                                Console.ForegroundColor = ConsoleColor.White
                                Console.WriteLine("Exit with Code: 0")
                                Environment.Exit(0)

                            End If


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
