module BytezBank.Client.Pages.CreateUser

open System
open Elmish
open Bolero
open Bolero.Html
open Bolero.Remoting
open Bolero.Remoting.Client
open Bolero.Templating.Client

type CreateUser = Template<"Pages/CreateUser/createUser.html">

let createUserPage model dispatch =
  CreateUser.CreateUser().Elt()
