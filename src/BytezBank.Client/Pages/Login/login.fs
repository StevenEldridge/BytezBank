module BytezBank.Client.Pages.Login

open System
open Elmish
open Bolero

open BytezBank.Client.Store.Store
open BytezBank.Client.Store.Login
open BytezBank.Client.Store.UserAccount
open BytezBank.Client.Store.SubStateTypeMessages


type Login = Template<"Pages/Login/login.html">

let loginUser (pageModel: PageModel<LoginState.Model>) dispatch =
  printfn "Logging in user"
  pageModel.Model.username |> printfn "User: %s"
  pageModel.Model.password |> printfn "Pass: %s"

  (pageModel.Model.username, pageModel.Model.password)
  |> UserAccountState.LoginUser
  |> UserAccountRequest
  |> ServiceRequest
  |> dispatch



let loginPage (model: Model) (pageModel: PageModel<LoginState.Model>) dispatch =
    Login
      .Login()
      .Username( (pageModel.Model.username), fun x ->
        LoginState.SetUsername x |> LoginMessage |> PageMessage |> dispatch
      )
      .Password( (pageModel.Model.password), fun x ->
        LoginState.SetPassword x |> LoginMessage |> PageMessage |> dispatch
      )
      .LoginUser( fun e -> loginUser pageModel dispatch)
      .Elt()

