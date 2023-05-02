module BytezBank.Client.Services.UserAccount


open System
open System.Net.Http
open HttpFs.Client
open Hopac
open Elmish
open Bolero

open BytezBank.Client.Store.SubStateTypeMessages
open BytezBank.Client.Store.UserAccount.UserAccountState


module UserAccount =
  // TODO: Pull from appsettings.json
  let private connStr = "https://localhost:7259/"

  type UserAccountService (httpClient: HttpClient) =

      member this.login: string * string -> Async<Result<ServiceUpdateMessage,string>> =
        fun (username: string, password: string) ->
          async {
            use content = new StringContent("")
            content.Headers.Add("username", username)
            content.Headers.Add("password", password)
            let! response = httpClient.PostAsync(connStr + "login", content) |> Async.AwaitTask
            let! body     = response.Content.ReadAsStringAsync() |> Async.AwaitTask
            let result =
              match response.IsSuccessStatusCode with
              | true  -> body |> UpdateMessage.SetToken |> UserAccountUpdate |> Ok
              | false -> body |> Error
            return result
          }
