// Copyright 2015 gRPC authors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using Grpc.Core;
using Helloworld;

namespace GreeterClient
{
  class Program
  {
    public static void Main(string[] args)
    {
      string sReply;
      Channel channel = new Channel("127.0.0.1:50051", ChannelCredentials.Insecure);

      var client = new Greeter.GreeterClient(channel);
      while (true)
      {
        Console.WriteLine("Greeting. Please enter your name or empty line to exit: ");
        string sUser = Console.ReadLine();
        if (string.IsNullOrEmpty(sUser)) break;

        if (GetGreeting(client, sUser, out sReply))
        {
          Console.WriteLine("Greeting: " + sReply);
        }
        else
        {
          Console.WriteLine("Error Connecting to server: " + sReply);
        }
      }

      channel.ShutdownAsync().Wait();
      Console.WriteLine("Press any key to exit...");
      Console.ReadKey();
    }

    private static bool GetGreeting(Greeter.GreeterClient client, string sUser, out string sReply)
    {
      sReply = string.Empty;
      try
      {
        var reply = client.SayHello(new HelloRequest { Name = sUser });
        sReply = $"1. {reply.Message}\r\n";
        reply = client.SayHelloAgain(new HelloRequest { Name = sUser });
        sReply += $"2. {reply.Message}";
        return true;
      }catch(Exception ex)
      {
        sReply += ex.Message;
        return false;
      }
    }
  }
}
