module AvaloniaMvvm.Services

open AvaloniaMvvm.Models

type TodoListService() =
    member _.getItems() =
        seq {
            TodoItem.create "Walk the dog"
            TodoItem.create "Buy some milk"
            TodoItem.create ("Learn Avalonia", is_checked=true)
        }