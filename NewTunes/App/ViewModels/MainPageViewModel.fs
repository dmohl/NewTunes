namespace NewTunes

    open System.Collections.ObjectModel
    open Caliburn.Micro
    open System
    open System.IO
    open System.Net
    open System.Runtime.Serialization
    open Newtonsoft.Json

    type MainPageViewModel() =
        inherit Screen()        
               
        let urlToSearchByArtistIds = "http://ax.phobos.apple.com.edgesuite.net/WebObjects/MZStoreServices.woa/wa/wsLookup?id={0}&entity=album&sort=recent"
        let urlToSearchBySearchTerm = "http://ax.phobos.apple.com.edgesuite.net/WebObjects/MZStoreServices.woa/wa/wsSearch?term=%s&media=music&entity=album&attribute=artistTerm&limit=10"
        
        let BuildITunesUrl urlTemplate searchTerm =
            String.Format(urlTemplate, [|searchTerm|])
        let artists = new ObservableCollection<ItemViewModel>()
        let artistCollections = new ObservableCollection<ItemViewModel>()

        member x.GetAll<'a> urlTemplate searchTerm =
            let url = BuildITunesUrl urlTemplate (Uri.EscapeDataString searchTerm)  
            let webClient = new WebClient()
            webClient.DownloadStringCompleted.Add(fun result -> let res = JsonConvert.DeserializeObject<iTunesJsonResults>(result.Result)
                                                                res.JsonResults 
                                                                |> Seq.filter(fun row -> row.CollectionName.Trim() <> "")
                                                                |> Seq.iter(fun artistCollection -> artistCollections.Add(artistCollection)) 
                                                                artistCollections |> Seq.distinctBy(fun a -> a.ArtistName)
                                                                |> Seq.iter(fun artist -> artists.Add(artist)))
            webClient.DownloadStringAsync(Uri url)

        member x.BuildArtists() =
            artistCollections.Clear()
            artists.Clear()
            x.GetAll<ItemViewModel> urlToSearchByArtistIds "273179909,277228393,159260351,264712928"
            
        member x.FindArtists searchTerm =
            x.GetAll<ItemViewModel> urlToSearchBySearchTerm searchTerm

        member x.AllArtists 
            with get() = 
                match artists.Count with
                | 0 -> x.BuildArtists(); artists
                | _ -> artists
                