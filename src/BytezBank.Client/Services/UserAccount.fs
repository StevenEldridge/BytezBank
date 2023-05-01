module BytezBank.Client.Services.UserAccount


open System
open System.Net.Http
open HttpFs.Client
open Hopac
open Elmish
open Bolero

// module UserAccount =
//   // TODO: Pull from appsettings.json
//   let private connStr = "http://localhost:5259/"
//
//   // let httpClientWithNoCookies () =
//   //   let handler = new HttpClientHandler(UseCookies = true)
//   //   let client = new HttpClient(handler)
//   //   client.DefaultRequestHeaders.Clear()
//   //   client
//
//   // we can trivially extend request to add convenience functions for common operations
//   // module Request =
//   //   let disableCookiesFromHttpClient h = { h with httpClient = httpClientWithNoCookies () }
//
//   let login (username: string) (password: string): string option =
//     try
//       use client = new HttpClient()
//       use content = new StringContent("")
//       content.Headers.Add("username", username)
//       content.Headers.Add("password", password)
//       let response = client.PostAsync(connStr + "login", content).Result
//       let result   = response.Content.ReadAsStringAsync().Result
//       result |> Some
//     with ex ->
//       ex.ToString() |> printfn "%s"
//       None


module UserAccount =
  // TODO: Pull from appsettings.json
  let private connStr = "https://localhost:7259/"

  // type IUserAccountService =
  //   abstract login: string -> string -> Async<string>

  type UserAccountService (httpClient: HttpClient) =

    // interface IUserAccountService with

      member this.login: string * string -> Async<Result<string,string>> = fun (username: string, password: string) ->
          async {
            use content = new StringContent("")
            content.Headers.Add("username", username)
            content.Headers.Add("password", password)
            let! response = httpClient.PostAsync(connStr + "login", content) |> Async.AwaitTask
            let! body     = response.Content.ReadAsStringAsync() |> Async.AwaitTask
            let result =
              match response.IsSuccessStatusCode with
                | true  -> body |> Ok
                | false -> body |> Error
            return result
          }
