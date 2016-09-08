#if HOPAC
printfn "Using Hopac"
#I "packages/FParsec/lib/net40-client"
#r "FParsecCS.dll"
#r "FParsec.dll"
#I "packages/Hopac/lib/net45"
#r "Hopac.Core.dll"
#r "Hopac.Platform.dll"
#r "Hopac.dll"
#I "packages/Aether/lib/portable-net45+netcore45+wpa81+wp8+MonoAndroid1+MonoTouch1"
#r "Aether.dll"
#I "packages/Hephaestus.Hopac/lib/net452"
#r "Hephaestus.dll"
#I "packages/Hekate/lib/portable-net45+netcore45+wpa81+wp8+MonoAndroid1+MonoTouch1"
#r "Hekate.dll"
#I "packages/Anat/lib/portable-net45+netcore45+wpa81+wp8+MonoAndroid1+MonoTouch1"
#r "Anat.dll"
#I "packages/Freya.Core.Hopac/lib/net452"
#r "Freya.Core.Hopac.dll"
#I "packages/Freya.Types/lib/net452"
#r "Freya.Types.dll"
#I "packages/Freya.Routers/lib/net452"
#r "Freya.Routers.dll"
#I "packages/Freya.Types.Http/lib/net452"
#r "Freya.Types.Http.dll"
#I "packages/Freya.Types.Language/lib/net452"
#r "Freya.Types.Language.dll"
#I "packages/Freya.Types.Uri/lib/net452"
#r "Freya.Types.Uri.dll"
#I "packages/Freya.Types.Uri.Template/lib/net452"
#r "Freya.Types.Uri.Template.dll"
#I "packages/Freya.Machines.Hopac/lib/net452"
#r "Freya.Machines.Hopac.dll"
#I "packages/Freya.Machines.Http.Hopac/lib/net452"
#r "Freya.Machines.Http.Hopac.dll"
#I "packages/Freya.Routers.Uri.Template.Hopac/lib/net452"
#r "Freya.Routers.Uri.Template.Hopac.dll"
#I "packages/Freya.Optics.Http.Hopac/lib/net452"
#r "Freya.Optics.Http.Hopac.dll"
#I "packages/Owin/lib/net40"
#r "Owin.dll"
#I "packages/Microsoft.Owin/lib/net45"
#r "Microsoft.Owin.dll"
#I "packages/Microsoft.Owin.Hosting/lib/net45"
#r "Microsoft.Owin.Hosting.dll"
#I "packages/Microsoft.Owin.Host.HttpListener/lib/net45"
#r "Microsoft.Owin.Host.HttpListener.dll"
#else
printfn "Using Async"
#I "packages/FParsec/lib/net40-client"
#r "FParsecCS.dll"
#r "FParsec.dll"
#I "packages/Aether/lib/portable-net45+netcore45+wpa81+wp8+MonoAndroid1+MonoTouch1"
#r "Aether.dll"
#I "packages/Hephaestus/lib/net452"
#r "Hephaestus.dll"
#I "packages/Hekate/lib/portable-net45+netcore45+wpa81+wp8+MonoAndroid1+MonoTouch1"
#r "Hekate.dll"
#I "packages/Anat/lib/portable-net45+netcore45+wpa81+wp8+MonoAndroid1+MonoTouch1"
#r "Anat.dll"
#I "packages/Freya.Core/lib/net452"
#r "Freya.Core.dll"
#I "packages/Freya.Types/lib/net452"
#r "Freya.Types.dll"
#I "packages/Freya.Routers/lib/net452"
#r "Freya.Routers.dll"
#I "packages/Freya.Types.Http/lib/net452"
#r "Freya.Types.Http.dll"
#I "packages/Freya.Types.Language/lib/net452"
#r "Freya.Types.Language.dll"
#I "packages/Freya.Types.Uri/lib/net452"
#r "Freya.Types.Uri.dll"
#I "packages/Freya.Types.Uri.Template/lib/net452"
#r "Freya.Types.Uri.Template.dll"
#I "packages/Freya.Machines/lib/net452"
#r "Freya.Machines.dll"
#I "packages/Freya.Machines.Http/lib/net452"
#r "Freya.Machines.Http.dll"
#I "packages/Freya.Routers.Uri.Template/lib/net452"
#r "Freya.Routers.Uri.Template.dll"
#I "packages/Freya.Optics.Http/lib/net452"
#r "Freya.Optics.Http.dll"
#I "packages/Owin/lib/net40"
#r "Owin.dll"
#I "packages/Microsoft.Owin/lib/net45"
#r "Microsoft.Owin.dll"
#I "packages/Microsoft.Owin.Hosting/lib/net45"
#r "Microsoft.Owin.Hosting.dll"
#I "packages/Microsoft.Owin.Host.HttpListener/lib/net45"
#r "Microsoft.Owin.Host.HttpListener.dll"
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
