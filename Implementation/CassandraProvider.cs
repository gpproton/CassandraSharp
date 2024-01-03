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
using CassandraSharp.Extensions;
using CassandraSharp.Interfaces;

namespace CassandraSharp.Implementation;

public class CassandraProvider : ICassandraProvider, IAsyncDisposable
{
    private readonly ILogger<CassandraProvider> _logger;
    private readonly ICluster _cluster;
    private readonly IClusterSession _session;

    public CassandraProvider(ILogger<CassandraProvider> logger, ICluster cluster, CassandraOptions options)
    {
        _logger = logger;
        _cluster = cluster;
        _session = _cluster.Connect(options.Keyspace);
    }

    public IClusterSession GetSession() => _session;

    public IClusterSession GetSession(string keyspace)
    {
        try
        {
            _cluster.Connect(keyspace);
        }
        catch (Exception)
        {
            _logger.LogCritical("Error connecting to cassandra session");
            throw;
        }

        return _session;
    }

    public async ValueTask DisposeAsync()
    {
        await _session.ShutdownAsync();
    }
}
