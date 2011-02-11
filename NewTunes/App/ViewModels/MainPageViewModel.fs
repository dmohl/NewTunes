namespace NewTunes

    open System.Collections.ObjectModel
    open Caliburn.Micro
    open System
    open System.IO
    open Newtonsoft.Json

    type MainPageViewModel(repository : iTunesRepository) =
        inherit Screen()        
        let mutable artists = new ObservableCollection<ItemViewModel>()
        let artistCollections = new ObservableCollection<ItemViewModel>()
        let ParseArtistCollection (json:string) =
            let jsonResults = JsonConvert.DeserializeObject<iTunesJsonResults> json 
            jsonResults.JsonResults 
            |> Seq.filter(fun row -> row.CollectionName.Trim() <> "")
            |> Seq.iter(fun artistCollection -> artistCollections.Add(artistCollection)) 
            artistCollections |> Seq.distinctBy(fun a -> a.ArtistName)
            |> Seq.iter(fun artist -> artists.Add(artist))

        let jsonProcessingAgent = MailboxProcessor.Start(fun inbox ->
                                                            async { while true do
                                                                    let! json = inbox.Receive()
                                                                    ParseArtistCollection json })
        
        let retrieveArtistCollections artistIds =
            artistCollections.Clear()
            artists.Clear()
            repository.GetAllByArtistIds jsonProcessingAgent artistIds
        
        do retrieveArtistCollections "273179909,277228393,159260351,264712928"

        member x.BuildArtists artistIds =
            retrieveArtistCollections "273179909,277228393,159260351,264712928"
            
        member x.FindArtists searchTerm =
            repository.GetAllBySearch jsonProcessingAgent searchTerm

        member x.AllArtists 
            with get() = artists
            and set v = artists <- v; base.NotifyOfPropertyChange "AllArtists"
                