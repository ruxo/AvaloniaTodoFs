namespace AvaloniaMvvm.ViewModels

open FSharp.Control.Reactive
open AvaloniaMvvm.Services
open ReactiveUI

type MainWindowViewModel() =
    inherit ViewModelBase()

    let service = TodoListService()
    let todo_list = TodoListViewModel <| service.getItems()
    let mutable content_view_model :ViewModelBase = todo_list

    member _.ToDoList = todo_list

    member this.ContentViewModel
        with get() = content_view_model
        and set (v: ViewModelBase) = IReactiveObjectExtensions.RaiseAndSetIfChanged(this, &content_view_model, v) |> ignore

    member this.AddItem() =
        let add_item_view_model = AddItemViewModel()

        let addItem item =
            item |> Option.iter todo_list.ListItems.Add
            this.ContentViewModel <- todo_list

        let ok_command = add_item_view_model.OkCommand |> Observable.map Some
        let cancel_command = add_item_view_model.CancelCommand |> Observable.map (fun _ -> None)

        ok_command |> Observable.merge cancel_command
                   |> Observable.take 1
                   |> Observable.subscribe addItem
                   |> ignore

        this.ContentViewModel <- add_item_view_model
