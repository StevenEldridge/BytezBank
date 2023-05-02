module BytezBank.Client.Store.Store

open System
open System.Net.Http
open Elmish
open Bolero
open Bolero.Html
open Bolero.Remoting
open Bolero.Remoting.Client
open Bolero.Templating.Client

open BytezBank.Client.Store.Login
open BytezBank.Client.Store.UserAccount
open BytezBank.Client.Store.SubStateTypeMessages
open BytezBank.Client.Services.UserAccount


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
  page:             Page
  userAccountModel: UserAccountState.Model
  error:            string option
}

let initModel = {
  page             = About
  userAccountModel = UserAccountState.initModel
  error            = None
}

// The Elmish application's update messages.
type Message =
  | SetPage        of Page
  | PageMessage    of PageMessage
  | ServiceRequest of ServiceRequestMessage
  | ServiceUpdate  of Result<ServiceUpdateMessage, string>
  | SetErrorMsg    of exn
  | ClearErrorMsg


let updatePageModel message model =
  match (model.page, message) with
  | Login login, LoginMessage msg -> {
      model with page = Login { Model = LoginState.update msg login.Model }
    }
  | _ ->
    printfn "Error: Trying to dispatch message for page that isn't active"
    model


let requestUserAccount
  (service: UserAccount.UserAccountService)
  (requestMsg:            UserAccountState.RequestMessage) =
    match requestMsg with
    | UserAccountState.LoginUser (user, pass) ->
      Cmd.OfAsync.either service.login (user, pass) ServiceUpdate SetErrorMsg

let requestService (service: UserAccount.UserAccountService) (message: ServiceRequestMessage) =
  match message with
  | UserAccountRequest msg -> requestUserAccount service msg

let updateServiceModel (message: ServiceUpdateMessage) model =
  match message with
  | UserAccountUpdate msg ->
    { model with userAccountModel = UserAccountState.update msg model.userAccountModel }


let update (userAccountService: UserAccount.UserAccountService) message model =
  match message with
  | SetPage        page -> { model with page = page }, Cmd.none
  | PageMessage    msg  -> updatePageModel msg model, Cmd.none
  | SetErrorMsg    exn  -> { model with error = Some exn.Message }, Cmd.none
  | ClearErrorMsg       -> { model with error = None }, Cmd.none
  | ServiceRequest msg  -> model, requestService userAccountService msg
  | ServiceUpdate  result ->
    match result with
    | Ok res -> updateServiceModel res model, Cmd.none
    | Error err ->
      err |> printfn "ERROR: %s"
      let e = new exn(err)
      { model with error = Some e.Message }, Cmd.none

