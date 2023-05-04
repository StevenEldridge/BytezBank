module BytezBank.Client.Store.BankAccount

open System

open BytezBank.Client.Models.Models


module BankAccountState =

  type Model = {
    bankAccountIds: int         list option
    bankAccounts:   BankAccount list option
    transactions:   Transaction list option
  }

  let initModel = {
    bankAccountIds = None
    bankAccounts   = None
    transactions   = None
  }

  type RequestMessage =
    | GetBankAccountIds of string
    | GetBankAccount    of string * int

  type UpdateMessage =
    | SetBankAccountIds of int list
    | ClearModel

  let update message model =
    match message with
    | SetBankAccountIds ids -> { model with bankAccountIds = Some ids }
    | ClearModel            -> {
      model with
        bankAccountIds = None
        bankAccounts   = None
        transactions   = None
    }
