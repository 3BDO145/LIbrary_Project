open System
open System.Windows.Forms
open System.Drawing


type Book = {
    Id: int  
    Title: string
    Author: string
    Genre: string
    IsBorrowed: bool
    BorrowDate: DateTime option
}


let mutable library: Map<int, Book> = Map.empty


let generateBookId () =

    let random = new Random()
    random.Next(1000000, 10000000)

let addBook title author genre =
    let bookId = generateBookId()
    let book = { 
        Id = bookId
        Title = title
        Author = author
        Genre = genre
        IsBorrowed = false
        BorrowDate = None 
    }
    library <- library.Add(book.Id, book)
    book

let searchBooks (title : string) =
    library |> Map.filter (fun _ book -> book.Title.ToLower().Contains(title.ToLower()))


let borrowBook id =
    match library.TryFind id with
    | Some book when not book.IsBorrowed -> 
        let updatedBook = { book with IsBorrowed = true; BorrowDate = Some DateTime.UtcNow }
        library <- library.Add(id, updatedBook)
        true
    | _ -> false

let returnBook id =
    match library.TryFind id with
    | Some book when book.IsBorrowed -> 
        let updatedBook = { book with IsBorrowed = false; BorrowDate = None }
        library <- library.Add(id, updatedBook)
        true
    | _ -> false
