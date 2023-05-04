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
open BytezBank.Client.Store.BankAccount
open BytezBank.Client.Store.SubStateTypeMessages
open BytezBank.Client.Services.UserAccount
open BytezBank.Client.Services.BankAccount


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
  bankAccountModel: BankAccountState.Model
  nextMessage:      Unit -> Message option
  error:            string option
}


// The Elmish application's update messages.
and Message =
  | SetPage        of Page
  | PageMessage    of PageMessage
  | ServiceRequest of ServiceRequestMessage * (Unit -> Message option)
  | ServiceUpdate  of Result<ServiceUpdateMessage, string>
  | SetErrorMsg    of exn
  | ClearErrorMsg


let initModel = {
  page             = About
  userAccountModel = UserAccountState.initModel
  bankAccountModel = BankAccountState.initModel
  nextMessage      = fun () -> None
  error            = None
}

let updatePageModel message model =
  match (model.page, message) with
  | Login login, LoginMessage msg -> {
      model with page = Login { Model = LoginState.update msg login.Model }
    }
  | _ ->
    printfn "Error: Trying to dispatch message for page that isn't active"
    model


let requestUserAccount
  (service:    UserAccount.UserAccountService)
  (requestMsg: UserAccountState.RequestMessage) =
    match requestMsg with
    | UserAccountState.LoginUser (user, pass) ->
      printfn "Request LoginUser triggered"
      Cmd.OfAsync.either service.login (user, pass) ServiceUpdate SetErrorMsg

let requestBankAccount
  (service:    BankAccount.BankAccountService)
  (requestMsg: BankAccountState.RequestMessage) =
    match requestMsg with
    | BankAccountState.GetBankAccountIds token ->
      printfn "Request GetBankAccountIds triggered"
      Cmd.OfAsync.either service.getBankAccountIds token ServiceUpdate SetErrorMsg
    | BankAccountState.GetBankAccount (token, bankAccountId) ->
      printfn "Request GetBankAccount triggered"
      Cmd.OfAsync.either service.getBankAccount (token, bankAccountId) ServiceUpdate SetErrorMsg


let requestService
  (userService: UserAccount.UserAccountService)
  (bankService: BankAccount.BankAccountService)
  (message: ServiceRequestMessage) =
    match message with
    | UserAccountRequest msg -> requestUserAccount userService msg
    | BankAccountRequest msg -> requestBankAccount bankService msg


let updateServiceModel (message: ServiceUpdateMessage) model =
  let executeNextMessage =
    match model.nextMessage() with
    | Some msg -> Cmd.ofMsg msg
    | None     -> Cmd.none

  match message with
  | UserAccountUpdate msg ->
    (msg, model.nextMessage) ||> printfn "UserAccountUpdate triggered with\nmsg: %A\ncmd: %A"
    { model with userAccountModel = UserAccountState.update msg model.userAccountModel },
    executeNextMessage

  | BankAccountUpdate msg ->
    (msg, model.nextMessage) ||> printfn "BankAccountUpdate triggered with\nmsg: %A\ncmd: %A"
    { model with bankAccountModel = BankAccountState.update msg model.bankAccountModel },
    executeNextMessage


let update
  (userAccountService: UserAccount.UserAccountService)
  (bankAccountService: BankAccount.BankAccountService)
  message
  model =
    match message with
    | SetPage        page       -> { model with page = page }, Cmd.ofMsg ClearErrorMsg
    | PageMessage    msg        -> updatePageModel msg model, Cmd.none
    | SetErrorMsg    exn        -> { model with error = Some exn.Message; nextMessage = fun () -> None }, Cmd.none
    | ClearErrorMsg             -> { model with error = None }, Cmd.none
    | ServiceRequest (msg, cmd) ->
      { model with nextMessage = cmd },
      requestService userAccountService bankAccountService msg
    | ServiceUpdate  result     ->
      match result with
      | Ok res -> updateServiceModel res model
      | Error err ->
        err |> printfn "ERROR: %s"
        let e = new exn(err)
        { model with error = Some e.Message; nextMessage = fun () -> None }, Cmd.none

