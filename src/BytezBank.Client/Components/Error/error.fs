module BytezBank.Client.Components.Error

open System
open Elmish
open Bolero

open BytezBank.Client.Store.Store


type ErrorComponent = Template<"Components/Error/error.html">


let errorComponent (errorText: string) =
    ErrorComponent
      .ErrorComponent()
      .ErrorText( errorText )
      .Elt()
