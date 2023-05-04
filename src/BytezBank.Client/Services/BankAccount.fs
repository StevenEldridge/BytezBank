module BytezBank.Client.Services.BankAccount


open System
open System.Net.Http
open System.Net.Http.Headers
open HttpFs.Client
open Hopac
open Elmish
open Bolero

open BytezBank.Client.Store.SubStateTypeMessages
open BytezBank.Client.Store.BankAccount.BankAccountState


module BankAccount =
  // TODO: Pull from appsettings.json
  let private connStr = "https://localhost:7259/"

  type BankAccountService (httpClient: HttpClient) =

    member this.getBankAccountIds: string -> Async<Result<ServiceUpdateMessage,string>> =
      fun (token: string) ->
        async {
          let client = httpClient
          client.DefaultRequestHeaders.Authorization <- new AuthenticationHeaderValue("Bearer", token)

          let! response = client.GetAsync(connStr + "bankaccountsowned") |> Async.AwaitTask
          let! body     = response.Content.ReadAsStringAsync() |> Async.AwaitTask

          let result =
            match response.IsSuccessStatusCode with
            | true  -> List.empty |> UpdateMessage.SetBankAccountIds |> BankAccountUpdate |> Ok
            | false -> body |> Error
          return result
        }

    member this.getBankAccount: string * int -> Async<Result<ServiceUpdateMessage,string>> =
      fun (token: string, bankAccountId: int) ->
        async {
          let client = httpClient
          client.DefaultRequestHeaders.Authorization <- new AuthenticationHeaderValue("Bearer", token)

          let! response =
            client.GetAsync(connStr + bankAccountId.ToString() + "/bankaccount")
            |> Async.AwaitTask
          let! body = response.Content.ReadAsStringAsync() |> Async.AwaitTask
          body |> printfn "%s"

          let result =
            match response.IsSuccessStatusCode with
            | true  -> List.empty |> UpdateMessage.SetBankAccountIds |> BankAccountUpdate |> Ok
            | false -> body |> Error
          return result
        }
