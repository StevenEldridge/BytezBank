module BytezBank.Client.Store

open System
open Elmish
open Bolero
open Bolero.Html
open Bolero.Remoting
open Bolero.Remoting.Client
open Bolero.Templating.Client


/// Routing endpoints definition.
type Page =
  | [<EndPoint "/">]                              About
  | [<EndPoint "/createuser">]                    CreateUser
  | [<EndPoint "/login">]                         Login
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

/// The Elmish application's update messages.
type Message =
  | SetPage of Page

let updateSetPage (page: Page) (model: Model) = { model with page = page }, Cmd.none

let update message model =
  match message with
  | SetPage page ->
    { model with page = page }

