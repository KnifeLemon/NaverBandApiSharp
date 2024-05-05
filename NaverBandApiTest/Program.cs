// See https://aka.ms/new-console-template for more information
using NaverBandApiSharp.API;

Console.WriteLine("Hello, World!");

BandApi bandApi = new BandApi();
bandApi.login(NaverBandApiSharp.Enums.BandApiAccountType.EMAIL, "test", "1234");

Console.ReadLine();