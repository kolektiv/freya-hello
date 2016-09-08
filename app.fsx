#if HOPAC
printfn "Using Hopac"
#I "packages/Aether/lib/portable-net45+netcore45+wpa81+wp8+MonoAndroid1+MonoTouch1"
#I "packages/Anat/lib/portable-net45+netcore45+wpa81+wp8+MonoAndroid1+MonoTouch1"
#I "packages/FParsec/lib/net40-client"
#I "packages/Freya.Core.Hopac/lib/net452"
#I "packages/Freya.Machines.Hopac/lib/net452"
#I "packages/Freya.Machines.Http.Hopac/lib/net452"
#I "packages/Freya.Optics/lib/net452"
#I "packages/Freya.Optics.Http.Hopac/lib/net452"
#I "packages/Freya.Polyfills.Hopac/lib/net452"
#I "packages/Freya.Routers/lib/net452"
#I "packages/Freya.Routers.Uri.Template.Hopac/lib/net452"
#I "packages/Freya.Types/lib/net452"
#I "packages/Freya.Types.Http/lib/net452"
#I "packages/Freya.Types.Language/lib/net452"
#I "packages/Freya.Types.Uri/lib/net452"
#I "packages/Freya.Types.Uri.Template/lib/net452"
#I "packages/Hekate/lib/portable-net45+netcore45+wpa81+wp8+MonoAndroid1+MonoTouch1"
#I "packages/Hephaestus.Hopac/lib/net452"
#I "packages/Hopac/lib/net45"
#I "packages/Microsoft.Owin/lib/net45"
#I "packages/Microsoft.Owin.Diagnostics/lib/net45"
#I "packages/Microsoft.Owin.Host.HttpListener/lib/net45"
#I "packages/Microsoft.Owin.Hosting/lib/net45"
#I "packages/Owin/lib/net40"
#r "Aether.dll"
#r "Anat.dll"
#r "FParsec.dll"
#r "FParsecCS.dll"
#r "Freya.Core.Hopac.dll"
#r "Freya.Machines.Hopac.dll"
#r "Freya.Machines.Http.Hopac.dll"
#r "Freya.Optics.dll"
#r "Freya.Optics.Http.Hopac.dll"
#r "Freya.Polyfills.Hopac.dll"
#r "Freya.Routers.dll"
#r "Freya.Routers.Uri.Template.Hopac.dll"
#r "Freya.Types.dll"
#r "Freya.Types.Http.dll"
#r "Freya.Types.Language.dll"
#r "Freya.Types.Uri.dll"
#r "Freya.Types.Uri.Template.dll"
#r "Hekate.dll"
#r "Hephaestus.dll"
#r "Hopac.dll"
#r "Hopac.Core.dll"
#r "Hopac.Platform.dll"
#r "Microsoft.Owin.dll"
#r "Microsoft.Owin.Diagnostics.dll"
#r "Microsoft.Owin.Host.HttpListener.dll"
#r "Microsoft.Owin.Hosting.dll"
#r "Owin.dll"
#else
printfn "Using Async"
#I "packages/Aether/lib/portable-net45+netcore45+wpa81+wp8+MonoAndroid1+MonoTouch1"
#I "packages/Anat/lib/portable-net45+netcore45+wpa81+wp8+MonoAndroid1+MonoTouch1"
#I "packages/FParsec/lib/net40-client"
#I "packages/Freya.Core/lib/net452"
#I "packages/Freya.Machines/lib/net452"
#I "packages/Freya.Machines.Http/lib/net452"
#I "packages/Freya.Optics/lib/net452"
#I "packages/Freya.Optics.Http/lib/net452"
#I "packages/Freya.Polyfills/lib/net452"
#I "packages/Freya.Routers/lib/net452"
#I "packages/Freya.Routers.Uri.Template/lib/net452"
#I "packages/Freya.Types/lib/net452"
#I "packages/Freya.Types.Http/lib/net452"
#I "packages/Freya.Types.Language/lib/net452"
#I "packages/Freya.Types.Uri/lib/net452"
#I "packages/Freya.Types.Uri.Template/lib/net452"
#I "packages/Hekate/lib/portable-net45+netcore45+wpa81+wp8+MonoAndroid1+MonoTouch1"
#I "packages/Hephaestus/lib/net452"
#I "packages/Microsoft.Owin/lib/net45"
#I "packages/Microsoft.Owin.Diagnostics/lib/net45"
#I "packages/Microsoft.Owin.Host.HttpListener/lib/net45"
#I "packages/Microsoft.Owin.Hosting/lib/net45"
#I "packages/Owin/lib/net40"
#r "Aether.dll"
#r "Anat.dll"
#r "FParsec.dll"
#r "FParsecCS.dll"
#r "Freya.Core.dll"
#r "Freya.Machines.dll"
#r "Freya.Machines.Http.dll"
#r "Freya.Optics.dll"
#r "Freya.Optics.Http.dll"
#r "Freya.Polyfills.dll"
#r "Freya.Routers.dll"
#r "Freya.Routers.Uri.Template.dll"
#r "Freya.Types.dll"
#r "Freya.Types.Http.dll"
#r "Freya.Types.Language.dll"
#r "Freya.Types.Uri.dll"
#r "Freya.Types.Uri.Template.dll"
#r "Hekate.dll"
#r "Hephaestus.dll"
#r "Microsoft.Owin.dll"
#r "Microsoft.Owin.Diagnostics.dll"
#r "Microsoft.Owin.Host.HttpListener.dll"
#r "Microsoft.Owin.Hosting.dll"
#r "Owin.dll"
#endif

open Freya.Core
open Freya.Machines.Http
open Freya.Routers.Uri.Template

let name =
    freya {
        let! name = Freya.Optic.get (Route.atom_ "name")

        match name with
        | Some name -> return name
        | _ -> return "World" }

let hello =
    freya {
        let! name = name
        return Represent.text (sprintf "Hello %s!" name) }

let machine =
    freyaMachine {
        handleOk hello }

let router =
    freyaRouter {
        resource "/hello{/name}" machine }

type HelloWorld () =
    member __.Configuration () =
        OwinAppFunc.ofFreya (router)

open System
open Microsoft.Owin.Hosting

let url = "http://localhost:7000"

do printfn "WebApp.Start: %s" url
   let d = WebApp.Start<HelloWorld> url
   try
     printf "Press enter to exit"
     Console.ReadLine () |> ignore
   finally d.Dispose ()
