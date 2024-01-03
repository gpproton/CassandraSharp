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

using Cassandra.Mapping;

namespace CassandraSharp.Model;

public class User : BaseModel<Guid>
{
    public string Name { get; set; } = string.Empty;
    public static Map<User> GetConfig(string keyspace) {
        return new Map<User>()
            .KeyspaceName(keyspace)
            .TableName("users")
            .PartitionKey(x => x.Id)
            .Column(x => x.Id, x => x.WithName("id"))
            .Column(x => x.Name, x => x.WithName("name"));
    }
}
