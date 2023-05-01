module BytezBank.Client.Pages.CreateUser

open System
open Elmish
open Bolero

open BytezBank.Client.Store.Store

type CreateUser = Template<"Pages/CreateUser/createUser.html">

let createUserPage model dispatch =
  CreateUser.CreateUser().Elt()
