module BytezBank.Client.Store.UserAccount

open System
open Elmish
open Bolero


module UserAccountState =

  type Model = {
    token: string option
  }

  let initModel = {
    token = None
  }

  type RequestMessage =
    | LoginUser of string * string

  type UpdateMessage =
    | SetToken of string
    | ClearModel

  let update message model =
    match message with
    | SetToken newToken -> { model with token = Some newToken }
    | ClearModel        -> {
      model with
        token = None
    }
