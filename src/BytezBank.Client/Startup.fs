namespace BytezBank.Client

open System.Net.Http
open Microsoft.AspNetCore.Components.WebAssembly.Hosting
open Microsoft.Extensions.DependencyInjection
open Bolero.Remoting.Client

open BytezBank.Client.Services.UserAccount
open BytezBank.Client.Services.BankAccount



open System
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection

module Program =

    let Inject (services: IServiceCollection) =
      let config = services.BuildServiceProvider().GetService<IConfiguration>()

      // let connStr: string         = config.["APIConnectionString"]
      // let httpClient: HttpClient  = services.AddHttpClient()
      // let userAccountService = UserAccount.UserAccountService(httpClient, connStr)

      services.AddHttpClient() |> ignore
      services.AddSingleton<UserAccount.UserAccountService>() |> ignore
      services.AddSingleton<BankAccount.BankAccountService>() |> ignore

    [<EntryPoint>]
    let Main args =
      let builder = WebAssemblyHostBuilder.CreateDefault(args)

      let configureServices(services: IServiceCollection) =
        let config  = services.BuildServiceProvider().GetService<IConfiguration>()
        let connStr = config.["App:APIConnectionString"]
        ignore



      builder.RootComponents.Add<Main.MyApp>("#main")
      Inject builder.Services
      // builder.Services.AddBoleroRemoting(builder.HostEnvironment) |> ignore
      builder.Build().RunAsync() |> ignore
      0
