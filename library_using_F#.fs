open System
open System.Windows.Forms
open System.Drawing

// Define a record for Book
type Book = {
    Id: int  // Change Id type to int
    Title: string
    Author: string
    Genre: string
    IsBorrowed: bool
    BorrowDate: DateTime option
}

// Define a library as a map of books by their ID
let mutable library: Map<int, Book> = Map.empty

// Logic Functions
let generateBookId () =
    // Generate a random 7 digit number for id 
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

