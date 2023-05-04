module BytezBank.Client.Models.Models

open System

type BankAccount = {
  Ussn:       string
  Accountid:  int
  Checkbal:   double
  Savebal:    double
  Mpr:        double
  Mpr_enable: bool
}

type UserAccount = {
  Name:      string
  Username:  string
  Birthdate: string
  Addr:      string
  Phone:     string
  Snn:       string
}

type Transaction = {
  Acntid:   int
  Act:      string
  Amount:   double
  Account:  string
  Newbal:   double
  TDate:    string
}
