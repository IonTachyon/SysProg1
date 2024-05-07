using Sistemsko_Projekat_1;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

class Program
{
    static async Task Main()
    {
        const int capacity = 1000;
        const int concurrency = 32;
        const string url = "http://localhost:5050/";
        HttpServer server = new HttpServer(url, concurrency, capacity);
        await server.Start();
    }
}