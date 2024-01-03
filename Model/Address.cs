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
// Created At: Tuesday, 2nd Jan 2024
// Modified By: Godwin peter .O
// Modified At: Wed Jan 03 2024

using Cassandra.Mapping;

namespace CassandraSharp.Model;

public class Address : BaseModel<Guid>
{
    public string? Street { get; set; }
    public string? City { get; set; }
    public int ZipCode { get; set; }
    public IEnumerable<string> Phones { get; set; } = [];
    public static Map<Address> GetConfig(string keyspace)
    {
        return new Map<Address>()
            .KeyspaceName(keyspace)
            .TableName("address")
            .PartitionKey(x => x.Id)
            .Column(x => x.Id, x => x.WithName("id"))
            .Column(x => x.Street, x => x.WithName("street"))
            .Column(x => x.City, x => x.WithName("city"))
            .Column(x => x.ZipCode, x => x.WithName("zip_code"))
            .Column(x => x.Phones, x => x.WithName("phones"));
    }
}
