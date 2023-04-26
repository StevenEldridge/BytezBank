module BytezBank.Client.Main

open System
open Elmish
open Bolero
open Bolero.Html
open Bolero.Remoting
open Bolero.Remoting.Client
open Bolero.Templating.Client

open BytezBank.Client.Store.Store
open BytezBank.Client.Store.Login
open BytezBank.Client.Pages.About
open BytezBank.Client.Pages.Login
open BytezBank.Client.Pages.CreateUser

let defaultModel = function
  | About       -> ()
  | Login model -> Router.definePageModel model LoginState.initModel
  | CreateUser  -> ()

/// Connects the routing system to the Elmish application.
let router = Router.inferWithModel SetPage (fun model -> model.page) defaultModel

type Main  = Template<"wwwroot/main.html">

let view (model: Model) (dispatch: Message -> Unit) =
  Main.Main()
    .Body(
      match model.page with
      | About                 -> aboutPage      model dispatch
      | Login      pageModel  -> loginPage      model pageModel dispatch
      | CreateUser            -> createUserPage model dispatch
    )
    .Elt()

type MyApp() =
  inherit ProgramComponent<Model, Message>()

  override this.Program =
    Program.mkSimple (fun _ -> initModel) update view
    |> Program.withRouter router
#if DEBUG
    |> Program.withHotReload
#endif
