module BytezBank.Client.Store.Login

open System
open Elmish
open Bolero


module LoginState =

  type Model = {
    username: string
    password: string
  }

  let initModel = {
    username = ""
    password = ""
  }

  type Message =
    | SetUsername of string
    | SetPassword of string

  let update message model =
    match message with
    | SetUsername user -> { model with username = user }
    | SetPassword pass -> { model with password = pass }
