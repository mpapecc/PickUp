using System;
using System.Collections.Generic;
using System.Text;

namespace PickUp.Common.Application
{
    public static class RetrierService
    {
        public static async Task<T> RetryOnExceptionAsync<T>(Func<Task<T>> action, int maxRetries = 3, int delayInSeconds = 10)
        {
            int retryCount = 0;
            while (true)
            {
                try
                {
                    return await action();
                }
                catch (Exception ex) when (retryCount < maxRetries)
                {
                    retryCount++;
                    Console.WriteLine($"Attempt {retryCount} failed: {ex.Message}. Retrying in {delayInSeconds}s...");
                    await Task.Delay(TimeSpan.FromSeconds(delayInSeconds));
                }
            }
        }
    }
}
