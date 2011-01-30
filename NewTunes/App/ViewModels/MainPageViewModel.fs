namespace WindowsPhonePanoramaApp

    open System.Collections.ObjectModel
    open Caliburn.Micro
    open System
    open System.IO
    open System.Net
    open System.Runtime.Serialization.Json

    type MainPageViewModel() =
        inherit Screen()        
                
        let BuildITunesUrl searchTerm =
            sprintf "http://ax.phobos.apple.com.edgesuite.net/WebObjects/MZStoreServices.woa/wa/wsSearch?term=%s&media=music&entity=album&attribute=artistTerm&limit=5" searchTerm
        let items = new ObservableCollection<ItemViewModel>()

        member x.GetAll<'a> url =
            let deserializer = new DataContractJsonSerializer(typeof<iTunesJsonResults>)
            let webClient = new WebClient()
            webClient.OpenReadCompleted.Add(fun result -> let res = deserializer.ReadObject(result.Result) :?> iTunesJsonResults
                                                          res.JsonResults |> Seq.iter(fun x -> items.Add(x)))
            webClient.OpenReadAsync(Uri url)

        member x.BuildItems() =
            x.GetAll<ItemViewModel> (BuildITunesUrl "The%20Script") 
            
        member x.Items 
            with get() = 
                match items.Count with
                | 0 -> x.BuildItems(); items
                | _ -> items
