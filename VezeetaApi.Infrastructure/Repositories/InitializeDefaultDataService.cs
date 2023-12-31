﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VezeetaApi.Domain.Services;

namespace VezeetaApi.Infrastructure.Repositories
{
    public class InitializeDefaultDataService : IHostedService
    {
        private readonly IServiceScopeFactory ServiceScopeFactory;

        public InitializeDefaultDataService(IServiceScopeFactory serviceScopeFactory)
        {
            ServiceScopeFactory = serviceScopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var initializeDefaultData = scope.ServiceProvider.GetRequiredService<IInitializeDefaultData>();
            await initializeDefaultData.InitializeData();
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
