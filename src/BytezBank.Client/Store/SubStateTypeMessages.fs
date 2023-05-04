module BytezBank.Client.Store.SubStateTypeMessages

open System
open Elmish
open Bolero

open BytezBank.Client.Store.Login
open BytezBank.Client.Store.UserAccount
open BytezBank.Client.Store.BankAccount

type PageMessage =
  | LoginMessage of LoginState.Message

type ServiceRequestMessage =
  | UserAccountRequest of UserAccountState.RequestMessage
  | BankAccountRequest of BankAccountState.RequestMessage

type ServiceUpdateMessage =
  | UserAccountUpdate of UserAccountState.UpdateMessage
  | BankAccountUpdate of BankAccountState.UpdateMessage
