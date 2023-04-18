module BytezBank.Client.Pages.Login

open System
open System.Net
open System.Net.Http
open System.Net.Http.Headers
open System.Threading.Tasks
open Elmish
open Bolero
open Bolero.Html
open Bolero.Remoting
open Bolero.Remoting.Client
open Bolero.Templating.Client

open BytezBank.Client.Store

type Login = Template<"Pages/Login/login.html">

type Model = {
  username: string
  password: string
}

type Message =
  | SetUsername of string
  | SetPassword of string

let login =
  NotImplementedException


let loginPage model dispatch =
  Login
    .Login()
    .Username( model.username, fun user -> SetUsername user |> dispatch )
    .Password( model.password, fun pass -> SetPassword pass |> dispatch )
    .LoginUser(fun e -> printfn "NOT IMPLEMENTED")
    .Elt()
