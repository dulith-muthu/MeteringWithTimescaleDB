using MeteringTest.Database;
using Microsoft.EntityFrameworkCore;

namespace MeteringTest.Services
{
    public class BackroundCreateDummyDataJob : IHostedService, IDisposable
    {
        private Timer? _timer;
        private readonly IDummyDataService _dummyDataService;

        public BackroundCreateDummyDataJob(IDummyDataService dummyDataService)
        {
            this._dummyDataService = dummyDataService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Set the interval for the job (in milliseconds)
            int intervalMilliseconds = 5000; // 5 seconds

            _timer = new Timer(DoJob, cancellationToken, 0, intervalMilliseconds);

            return Task.CompletedTask;
        }

        private async void DoJob(object? state)
        {
            await _dummyDataService.CreateDummySamplesForCustAsync(state != null ? (CancellationToken) state : CancellationToken.None);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Dispose();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
