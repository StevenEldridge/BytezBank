module BytezBank.Client.Store.SubStateTypeMessages

open System
open Elmish
open Bolero

open BytezBank.Client.Store.Login
open BytezBank.Client.Store.UserAccount

type PageMessage =
  | LoginMessage of LoginState.Message

type ServiceRequestMessage =
  | UserAccountRequest of UserAccountState.RequestMessage

type ServiceUpdateMessage =
  | UserAccountUpdate of UserAccountState.UpdateMessage
