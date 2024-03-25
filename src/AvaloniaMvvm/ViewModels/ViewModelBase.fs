namespace AvaloniaMvvm.ViewModels

open System
open System.Collections.ObjectModel
open System.Reactive
open AvaloniaMvvm.Models
open ReactiveUI

type ViewModelBase() =
    inherit ReactiveObject()

type TodoListViewModel(items: TodoItem seq) =
    inherit ViewModelBase()

    let list_items = ObservableCollection<TodoItem> items

    member _.ListItems = list_items

type private State = {
    OkCommand: ReactiveCommand<Unit, TodoItem>
    CancelCommand: ReactiveCommand<Unit, Unit>
}

type AddItemViewModel() as this =
    inherit ViewModelBase()

    let mutable description = String.Empty
    let mutable state = None

    do this.ActivationTime()

    member _.ActivationTime() =
        state <- let is_valid_observable = this.WhenAnyValue((fun x -> x.Description), not << String.IsNullOrWhiteSpace)
                 in Some {
                     OkCommand = let newTodoItem() = TodoItem.create description
                                  in ReactiveCommand.Create<TodoItem>(newTodoItem, is_valid_observable)
                     CancelCommand = ReactiveCommand.Create(Action(fun _ -> ()))
                 }

    member this.Description with get() = description
                            and set value = IReactiveObjectExtensions.RaiseAndSetIfChanged(this, &description, value) |> ignore

    member _.OkCommand = state.Value.OkCommand
    member _.CancelCommand = state.Value.CancelCommand
