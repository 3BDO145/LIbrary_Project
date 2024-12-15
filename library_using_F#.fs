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


// UI Setup
let form = new Form(Text = "Library Management System", Size = new Size(800, 600), BackColor = Color.GhostWhite)

let listView = new ListView(Dock = DockStyle.Top, Height = 300, View = View.Details, Font = new Font("Arial", 10.0f))
listView.BackColor <- Color.LightSteelBlue

listView.Columns.Add("ID", 150)
listView.Columns.Add("Title", 200)
listView.Columns.Add("Author", 150)
listView.Columns.Add("Genre", 100)
listView.Columns.Add("Status", 100)

// Create Labels for the TextBoxes
let lblTitle = new Label(Text = "Title", Top = 310, Width = 50, Font = new Font("Arial", 10.0f))
let lblAuthor = new Label(Text = "Author", Top = 340, Width = 50, Font = new Font("Arial", 10.0f))
let lblGenre = new Label(Text = "Genre", Top = 370, Width = 50, Font = new Font("Arial", 10.0f))
let lblSearch = new Label(Text = "Search", Top = 400, Width = 50, Font = new Font("Arial", 10.0f))
let lblBookId = new Label(Text = "Book ID", Top = 430, Width = 50,Height = 50, Font = new Font("Arial", 10.0f))

let txtTitle = new TextBox(Top = 310, Left = 60, Width = 250, Font = new Font("Arial", 10.0f))
let txtAuthor = new TextBox(Top = 340, Left = 60, Width = 250, Font = new Font("Arial", 10.0f))
let txtGenre = new TextBox(Top = 370, Left = 60, Width = 250, Font = new Font("Arial", 10.0f))
let txtSearch = new TextBox(Top = 400, Left = 60, Width = 250, Font = new Font("Arial", 10.0f))
let txtBookId = new TextBox(Top = 430, Left = 60, Width = 250, Font = new Font("Arial", 10.0f))

// Helper function to style buttons
let styleButton (btn: Button) =
    btn.BackColor <- Color.LightSteelBlue
    btn.FlatStyle <- FlatStyle.Flat
    btn.Font <- new Font("Arial", 10.0f, FontStyle.Bold)
    btn.ForeColor <- Color.White
    btn.Width <- 120
    btn.Height <- 40
    btn.FlatAppearance.BorderSize <- 0
    btn.Cursor <- Cursors.Hand
    btn.TextAlign <- ContentAlignment.MiddleCenter

let btnAdd = new Button(Text = "Add Book", Top = 310, Left = 380)
let btnSearch = new Button(Text = "Search Book", Top = 400, Left = 380)
let btnBorrow = new Button(Text = "Borrow Book", Top = 430, Left = 380)
let btnReturn = new Button(Text = "Return Book", Top = 460, Left = 380)
let btnDisplay = new Button(Text = "Display All", Top = 490, Left = 380)

[btnAdd; btnSearch; btnBorrow; btnReturn; btnDisplay] |> List.iter styleButton

// Event Handlers
btnAdd.Click.Add(fun _ -> 
    let title = txtTitle.Text
    let author = txtAuthor.Text
    let genre = txtGenre.Text
    if not (String.IsNullOrWhiteSpace(title) || String.IsNullOrWhiteSpace(author) || String.IsNullOrWhiteSpace(genre)) then
        let book = addBook title author genre
        MessageBox.Show(sprintf "Book '%s' added successfully with ID: %d" book.Title book.Id, "Success") |> ignore
        txtTitle.Clear()
        txtAuthor.Clear()
        txtGenre.Clear()
)

btnSearch.Click.Add(fun _ -> 
    let title = txtSearch.Text
    let results = searchBooks title
    listView.Items.Clear()
    results |> Map.iter (fun _ book -> 
        let status = if book.IsBorrowed then "Borrowed" else "Available"
        listView.Items.Add(new ListViewItem([| book.Id.ToString(); book.Title; book.Author; book.Genre; status |])) |> ignore
    )
)
