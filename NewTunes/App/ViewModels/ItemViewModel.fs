namespace WindowsPhonePanoramaApp

open System.Runtime.Serialization
open System.Runtime.Serialization.Json
open Caliburn.Micro

[<DataContract()>]
type ItemViewModel() =
    let mutable artistId = ""
    let mutable artistName = ""
    let mutable collectionName = ""
    let mutable artistViewUrl = ""
    let mutable releaseDate = ""
    let mutable collectionViewUrl = ""
    [<DataMember(Name="artistId")>] member x.ArtistId with get() = artistId and set(v) = artistId <- v
    [<DataMember(Name="artistName")>] member x.ArtistName with get() = artistName and set(v) = artistName <- v
    [<DataMember(Name="collectionName")>] member x.CollectionName with get() = collectionName and set(v) = collectionName <- v
    [<DataMember(Name="artistViewUrl")>] member x.ArtistViewUrl with get() = artistViewUrl and set(v) = artistViewUrl <- v
    [<DataMember(Name="releaseDate")>] member x.ReleaseDate with get() = releaseDate and set(v) = releaseDate <- v
    [<DataMember(Name="collectionViewUrl")>] member x.CollectionViewUrl with get() = collectionViewUrl and set(v) = collectionViewUrl <- v
    
[<DataContract()>]
type iTunesJsonResults() =
    let mutable jsonResults:ItemViewModel[] = [||]
    [<DataMember(Name="results")>] member x.JsonResults with get() = jsonResults and set(v) = jsonResults <- v
