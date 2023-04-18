module BytezBank.Client.Pages.About

open System
open Elmish
open Bolero
open Bolero.Html
open Bolero.Remoting
open Bolero.Remoting.Client
open Bolero.Templating.Client

open BytezBank.Client.Store


type About = Template<"Pages/About/about.html">

let aboutPage model dispatch =
  About
    .About()
    .GotoLogin(      fun e -> SetPage Page.Login      |> dispatch )
    .GotoCreateUser( fun e -> SetPage Page.CreateUser |> dispatch )
    .Elt()
