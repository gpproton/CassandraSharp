// Copyright (c) 2023 - 2024 drolx Solutions
// 
// Licensed under the MIT License;
// you may not use this file except in compliance with the License.
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// Author: Godwin peter .O (me@godwin.dev)
// Created At: Wednesday, 3rd Jan 2024
// Modified By: Godwin peter .O
// Modified At: Wed Jan 03 2024

using Cassandra;
using Cassandra.Data.Linq;
using CassandraSharp.Extensions;
using CassandraSharp.Interfaces;
using CassandraSharp.Model;

namespace CassandraSharp.Service;

public class CassandraHostedService(ILogger<CassandraHostedService> logger, ICluster cluster, IServiceScopeFactory factory, CassandraOptions options) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var scope = factory.CreateScope().ServiceProvider;
        var bootSession = cluster.Connect();
        var cassandraProvider = scope.GetRequiredService<ICassandraProvider>();

        bootSession.CreateKeyspaceIfNotExists(options.Keyspace);
        logger.LogInformation("Creating cassandra default keyspace");

        try
        {
            var session = cassandraProvider.GetSession();
            await new Table<User>(session).CreateIfNotExistsAsync();
            await new Table<Address>(session).CreateIfNotExistsAsync();
        }
        catch (Exception)
        {
            logger.LogInformation("Tables creation failed");
            throw;
        }

        var keyspaces = new List<string>(cluster.Metadata.GetKeyspaces());
        keyspaces.ForEach((value) =>
        {
            if (value == options.Keyspace)
            {
                logger.LogInformation("KeySpace: " + value);
                new List<string>(cluster.Metadata.GetKeyspace(value).GetTablesNames()).ForEach((tableName) =>
                {
                    Console.WriteLine("Table: " + tableName);
                });
            }
        });
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Finishing cassandra background activities");
        return Task.CompletedTask;
    }
}
