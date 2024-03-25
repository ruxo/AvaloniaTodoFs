module AvaloniaMvvm.Models

type TodoItem = {
    Description: string
    IsChecked: bool
}
with
    static member create(description, ?is_checked: bool) = { Description = description; IsChecked = is_checked |> Option.defaultValue false }