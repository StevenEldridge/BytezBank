module BytezBank.Client.Pages.Login

open System
open System.Net
open System.Net.Http
open System.Net.Http.Headers
open System.Threading.Tasks
open System
open Elmish
open Bolero
open Bolero.Html
open Bolero.Remoting
open Bolero.Remoting.Client
open Bolero.Templating.Client

open BytezBank.Client.Store.Store
open BytezBank.Client.Store.Login


type Login = Template<"Pages/Login/login.html">

let loginUser (pageModel: PageModel<LoginState.Model>) =
  printfn "Logging in user"
  pageModel.Model.username |> printfn "User: %s"
  pageModel.Model.password |> printfn "Pass: %s"
  // TODO: Contact API


let loginPage (model: Model) (pageModel: PageModel<LoginState.Model>) (dispatch: Message -> Unit) =
  Login
    .Login()
    .Username( (pageModel.Model.username), fun x ->
      LoginState.SetUsername x |> LoginMessage |> SetPageMessage |> dispatch
    )
    .Password( (pageModel.Model.password), fun x ->
      LoginState.SetPassword x |> LoginMessage |> SetPageMessage |> dispatch
    )
    .LoginUser( fun e -> loginUser pageModel )
    .Elt()

