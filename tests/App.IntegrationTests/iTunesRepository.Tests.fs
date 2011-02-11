namespace NewTunes.Tests
open FsUnit
open NUnit.Framework
open NewTunes
open System.Collections.ObjectModel


[<TestFixture>]
type ``When getting all artist collections from iTunes``() =
    let mutable jsonString = ""
    let agent = MailboxProcessor<string>.Start(fun inbox ->
                                            async { while true do
                                                        let! json = inbox.Receive() 
                                                        jsonString <- json})

    [<TestFixtureSetUp>] 
    member test.``Because Of``() =
        let repository = new iTunesRepository()
        repository.GetAllByArtistIds agent "277228393"
    [<Test>] 
    member test.``Should return more than one result``() =
        jsonString.Length > 0 |> should be True 
