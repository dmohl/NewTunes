namespace NewTunes

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
        let artists = new ObservableCollection<ItemViewModel>()
        let artistCollections = new ObservableCollection<ItemViewModel>()

        member x.GetAll<'a> url =
            let deserializer = new DataContractJsonSerializer(typeof<iTunesJsonResults>)
            let webClient = new WebClient()
            webClient.OpenReadCompleted.Add(fun result -> let res = deserializer.ReadObject(result.Result) :?> iTunesJsonResults
                                                          res.JsonResults 
                                                          |> Seq.iter(fun artistCollection -> artistCollections.Add(artistCollection)) 
                                                          let allArtists = res.JsonResults |> Seq.distinctBy(fun a -> a.ArtistName) 
                                                          allArtists |> Seq.iter(fun artist -> artists.Add(artist)))
            webClient.OpenReadAsync(Uri url)

        member x.BuildArtists() =
            x.GetAll<ItemViewModel> (BuildITunesUrl "The%20Script") 
            
        member x.AllArtists 
            with get() = 
                match artists.Count with
                | 0 -> x.BuildArtists(); artists
                | _ -> artists
                