module BytezBank.Server.Index

open Bolero
open Bolero.Html
open Bolero.Server.Html
open BytezBank

let page = doctypeHtml {
    head {
        meta {
          attr.charset "UTF-8"
        }
        meta {
          attr.name "viewport";
          attr.content "width=device-width, initial-scale=1.0"
        }
        title {
          "Bytez Bank"
        }
        ``base`` {
          attr.href "/"
        }
        link {
          attr.href        "https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha2/dist/css/bootstrap.min.css";
          attr.rel         "stylesheet";
          attr.integrity   "sha384-aFq/bzH65dt+w6FI2ooMVUpc+21e0SRygnTpmBvdBgSdnuTN7QbdgL+OapgHtvPp";
          attr.crossorigin "anonymous"
        }
        // link {
        //   attr.rel "stylesheet";
        //   attr.href "BytezBank.Client.styles.css"
        // }
        link {
          attr.rel "stylesheet";
          attr.href "css/index.css"
        }
    }

    body {
      div {
        attr.id "main";
        comp<Client.Main.MyApp>
      }
      boleroScript
    }
}
