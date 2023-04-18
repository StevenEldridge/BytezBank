module BytezBank.Client.Main

open System
open Elmish
open Bolero
open Bolero.Html
open Bolero.Remoting
open Bolero.Remoting.Client
open Bolero.Templating.Client

open BytezBank.Client.Store
open BytezBank.Client.Pages.About
open BytezBank.Client.Pages.Login
open BytezBank.Client.Pages.CreateUser

/// Connects the routing system to the Elmish application.
let router = Router.infer SetPage (fun model -> model.page)

type Main  = Template<"wwwroot/main.html">

let view model dispatch =
  Main.Main()
    .Body(
      match model.page with
      | About      -> aboutPage      model dispatch
      | Login      -> loginPage      model dispatch
      | CreateUser -> createUserPage model dispatch
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
