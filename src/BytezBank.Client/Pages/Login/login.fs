module BytezBank.Client.Pages.Login

open System
open Elmish
open Bolero

open BytezBank.Client.Store.Store
open BytezBank.Client.Store.Login
open BytezBank.Client.Services.UserAccount


type Login = Template<"Pages/Login/login.html">

let loginUser (pageModel: PageModel<LoginState.Model>) dispatch =
  printfn "Logging in user"
  pageModel.Model.username |> printfn "User: %s"
  pageModel.Model.password |> printfn "Pass: %s"

  (pageModel.Model.username, pageModel.Model.password)
  |> APILogin
  |> dispatch



let loginPage (model: Model) (pageModel: PageModel<LoginState.Model>) dispatch =
    Login
      .Login()
      .Username( (pageModel.Model.username), fun x ->
        LoginState.SetUsername x |> LoginMessage |> SetPageMessage |> dispatch
      )
      .Password( (pageModel.Model.password), fun x ->
        LoginState.SetPassword x |> LoginMessage |> SetPageMessage |> dispatch
      )
      .LoginUser( fun e -> loginUser pageModel dispatch)
      .Elt()

