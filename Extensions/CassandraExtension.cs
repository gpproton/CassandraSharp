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
using CassandraSharp.Implementation;
using Cassandra.Mapping;
using CassandraSharp.Interfaces;
using CassandraSharp.Service;
using Microsoft.Extensions.Options;

namespace CassandraSharp.Extensions;

public class CassandraOptions
{
    public string? Keyspace { get; set; }
    public ITypeDefinition[]? Config { get; set; }
};

public static class CassandraExtension
{
    public static IServiceCollection RegisterCassandra(this IServiceCollection services)
    {
        var scope = services.BuildServiceProvider();
        var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
        var cassandraOptions = scope.GetRequiredService<CassandraOptions>();
        var cassandraConfig = config.GetSection("Cassandra");
        services.Configure<CassandraConfig>(cassandraConfig);

        services.AddSingleton<ICluster>(provider => {
            var conf = provider.GetRequiredService<IOptions<CassandraConfig>>();
            var queryOptions = new QueryOptions().SetConsistencyLevel(ConsistencyLevel.One);

            return Cluster.Builder()
                .AddContactPoint(conf.Value.Host)
                .WithPort(conf.Value.Port)
                .WithCompression(CompressionType.LZ4)
                .WithCredentials(conf.Value.Username, conf.Value.Password)
                .WithQueryOptions(queryOptions)
                .WithRetryPolicy(new LoggingRetryPolicy(new DefaultRetryPolicy()))
                .Build();
        });
        services.AddScoped<ICassandraProvider, CassandraProvider>();

        // NOTE: Alternative way of mapping entity configurations
        // MappingConfiguration.Global.Define<CassandraMappings>();
        
        MappingConfiguration.Global.Define(cassandraOptions.Config);
        services.AddHostedService<CassandraHostedService>();

        return services;
    }
}
