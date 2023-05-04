module BytezBank.Client.Pages.Login

open System
open Elmish
open Bolero

open BytezBank.Client.Store.Store
open BytezBank.Client.Store.Login
open BytezBank.Client.Store.UserAccount
open BytezBank.Client.Store.BankAccount
open BytezBank.Client.Store.SubStateTypeMessages


type Login = Template<"Pages/Login/login.html">

let loginUser model (pageModel: PageModel<LoginState.Model>) dispatch =

  printfn "Logging in user"
  pageModel.Model.username |> printfn "User: %s"
  pageModel.Model.password |> printfn "Pass: %s"

  let login cmd: Message =
    (pageModel.Model.username, pageModel.Model.password)
    |> UserAccountState.LoginUser
    |> UserAccountRequest
    |> fun req -> ServiceRequest (req, cmd)
    |> fun x -> x |> printfn "Login:\n%A"; x

  let getBankAccountIds cmd: Model -> Message option = fun (model: Model) ->
    model.userAccountModel.token |> printfn "Current Token: %A"
    match model.userAccountModel.token with
    | None       -> SetErrorMsg (new exn("Not authenticated")) |> Some
    | Some token ->
      token
      |> BankAccountState.GetBankAccountIds
      |> BankAccountRequest
      |> fun req -> ServiceRequest (req, cmd)
      |> fun x -> x |> printfn "getBankAccountIds:\n%A"; x |> Some

  let getBankAccount cmd: Model -> Message option = fun (model: Model) ->
    match (model.userAccountModel.token, model.bankAccountModel.bankAccountIds) with
    | None  , _                        -> SetErrorMsg (new exn("Not authenticated")) |> Some
    | Some _, None                     -> SetErrorMsg (new exn("You own no bank accounts")) |> Some
    | Some _, Some x when x.Length = 0 -> SetErrorMsg (new exn("You own no bank accounts")) |> Some
    | Some token, Some bankAccountIds  ->
      (token, bankAccountIds.Head)
      |> BankAccountState.GetBankAccount
      |> BankAccountRequest
      |> fun req -> ServiceRequest (req, cmd)
      |> fun x -> x |> printfn "getBankAccount:\n%A"; x |> Some

  login (getBankAccountIds(getBankAccount(fun (model) -> SetPage About |> Some)))
  |> dispatch
  // SetPage About
  // |> fun x -> getBankAccount x
  // |> fun x -> getBankAccountIds x
  // |> login
  // |> fun x -> x |> printfn"%A"; x
  // |> dispatch




  // |> fun req -> ServiceRequest (req, Cmd.ofMsg(
  //   ServiceRequest (
  // )
  // |> dispatch



let loginPage (model: Model) (pageModel: PageModel<LoginState.Model>) dispatch =
    Login
      .Login()
      .Username( (pageModel.Model.username), fun x ->
        LoginState.SetUsername x |> LoginMessage |> PageMessage |> dispatch
      )
      .Password( (pageModel.Model.password), fun x ->
        LoginState.SetPassword x |> LoginMessage |> PageMessage |> dispatch
      )
      .LoginUser( fun e -> loginUser model pageModel dispatch)
      .Elt()

