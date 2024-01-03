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
// Created At: Sunday, 31st Dec 2023
// Modified By: Godwin peter .O
// Modified At: Wed Jan 03 2024

using Cassandra.Data.Linq;
using CassandraSharp.Constants;
using CassandraSharp.Extensions;
using CassandraSharp.Interfaces;
using CassandraSharp.Model;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var keyspace = CanssandraConst.Keyspace;
builder.Services.AddSingleton(provider => new CassandraOptions
{
  Keyspace = "trace",
  Config = [
      User.GetConfig(keyspace),
      Address.GetConfig(keyspace)
    ]
});
builder.Services.RegisterCassandra();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello");

app.MapGet("/users", async (ICassandraProvider provider) =>
{
  var session = provider.GetSession();
  var users = new Table<User>(session);

  return await users.ExecuteAsync();
});

app.MapPost("/users", async (User request, [FromServices] ICassandraProvider provider) =>
{
  var session = provider.GetSession();
  var users = new Table<User>(session);

  return await users.Insert(request).ExecuteAsync(); ;
});

app.Run();
