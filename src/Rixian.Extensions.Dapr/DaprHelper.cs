// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.Dapr
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    /// <summary>
    /// Helper methods for using Dapr.
    /// </summary>
    public class DaprHelper
    {
        private static string DaprPort => Environment.GetEnvironmentVariable("DAPR_GRPC_PORT") ?? "50001";

        private static string DaprHostIp => Environment.GetEnvironmentVariable("DAPR_HOST_IP") ?? "127.0.0.1";

        /// <summary>
        /// Blocking call that waits for Dapr to be available. Should be used when relying on Dapr secrets to bootstrap IConfiguration.
        /// </summary>
        /// <param name="retryCount">The number of times to retry connecting to Dapr.</param>
        public static void WaitForDapr(int retryCount = 3)
        {
            try
            {
                var ipadress = IPAddress.Parse(DaprHostIp);
                Console.WriteLine($"Starting to wait for Dapr: {DaprHostIp}:{DaprPort}");

                // Wait for Dapr Grpc port to be available
                var currentRetry = 0;
                while (true)
                {
                    try
                    {
                        using (var tcpClient = new TcpClient())
                        {
                            Console.WriteLine($"Attempting to connect to Dapr: {DaprHostIp}:{DaprPort}");
                            tcpClient.ConnectAsync(ipadress, int.Parse(DaprPort)).Wait(10000);
                        }

                        break;
                    }
                    catch
                    {
                        Console.WriteLine($"Failed to connect to Dapr. Trying again: {DaprHostIp}:{DaprPort}");
                        currentRetry++;

                        if (currentRetry > retryCount)
                        {
                            Console.WriteLine($"Attempted to connect to Dapr {retryCount} times. Failing: {DaprHostIp}:{DaprPort}");
                            throw;
                        }
                    }

                    Console.WriteLine($"Waiting 1000ms before attempting to connect to Dapr: {DaprHostIp}:{DaprPort}");
                    Task.Delay(1000).Wait();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception connecting to Dapr: {ex.Message}");
                throw;
            }
        }
    }
}
