module BytezBank.Client.Store.Store

open System
open Elmish
open Bolero
open Bolero.Html
open Bolero.Remoting
open Bolero.Remoting.Client
open Bolero.Templating.Client

open BytezBank.Client.Store.Login


/// Routing endpoints definition.
type Page =
  | [<EndPoint "/">]                              About
  | [<EndPoint "/createuser">]                    CreateUser
  | [<EndPoint "/login">]                         Login      of PageModel<LoginState.Model>
  // | [<EndPoint "/user/{userID}/home">]            AccountHome        of userID: int
  // | [<EndPoint "/user/{userID}/overview">]        AccountOverview    of userID: int
  // | [<EndPoint "/user/{userID}/depositwithdraw">] DepositWithdraw    of userID: int
  // | [<EndPoint "/user/{userID}/transfer">]        TransferMoney      of userID: int
  // | [<EndPoint "/user/{userID}/interest">]        ManageInterest     of userID: int
  // | [<EndPoint "/user/{userID}/history">]         TransactionHistroy of userID: int

/// The Elmish application's model.
type Model = {
  page: Page
}

let initModel = {
  page = About
}

type PageMessage =
  | LoginMessage of LoginState.Message

// The Elmish application's update messages.
type Message =
  | SetPage of Page
  | SetPageMessage of PageMessage

let updateSetPage (page: Page) (model: Model) = { model with page = page }, Cmd.none

let updatePageModel message model =
  match (model.page, message) with
  | Login login, LoginMessage msg -> {
      model with page = Login { Model = LoginState.update msg login.Model }
    }
  | _ -> model

let update message model =
  match message with
  | SetPage        page -> { model with page = page }
  | SetPageMessage mes  -> updatePageModel mes model

