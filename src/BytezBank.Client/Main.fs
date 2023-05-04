module BytezBank.Client.Main

open System
open System.Net.Http
open Microsoft.AspNetCore.Components
open Elmish
open Bolero
open Bolero.Html
open Bolero.Templating.Client

open BytezBank.Client.Components.Error
open BytezBank.Client.Store.Store
open BytezBank.Client.Store.Login
open BytezBank.Client.Pages.About
open BytezBank.Client.Pages.Login
open BytezBank.Client.Pages.CreateUser
open BytezBank.Client.Services.UserAccount
open BytezBank.Client.Services.BankAccount

let defaultModel = function
  | About       -> ()
  | Login model -> Router.definePageModel model LoginState.initModel
  | CreateUser  -> ()

/// Connects the routing system to the Elmish application.
let router = Router.inferWithModel SetPage (fun model -> model.page) defaultModel

type Main  = Template<"wwwroot/main.html">

let view (model: Model) (dispatch: Message -> Unit) =
  Main.Main()
    .ErrorComponent(
      match model.error with
      | Some error -> errorComponent (error.ToString())
      | None       -> empty()
    )
    .Body(
      match model.page with
      | About                 -> aboutPage      model dispatch
      | Login      pageModel  -> loginPage      model pageModel dispatch
      | CreateUser            -> createUserPage model dispatch
    )
    .Elt()

type MyApp() =
  inherit ProgramComponent<Model, Message>()

  [<Inject>]
  member val UserAccountService = Unchecked.defaultof<UserAccount.UserAccountService> with get, set

  [<Inject>]
  member val BankAccountService = Unchecked.defaultof<BankAccount.BankAccountService> with get, set

  override this.Program =
    let update = update this.UserAccountService this.BankAccountService

    Program.mkProgram (fun _ -> initModel, Cmd.none) update view
    |> Program.withRouter router
#if DEBUG
    |> Program.withHotReload
#endif
