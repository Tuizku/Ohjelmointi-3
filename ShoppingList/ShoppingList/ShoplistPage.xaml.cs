using System.ComponentModel;

namespace ShoppingList;

public class Item
{
    private string name;
    private bool check;

    public string Name
    {
        get => name;
        set
        {
            if (name != value)
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
    }
    public bool Check
    {
        get => check;
        set
        {
            if (check != value)
            {
                check = value;
                OnPropertyChanged(nameof(Check));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        Console.WriteLine($"PropertyChanged: {propertyName}");
    }
}

public partial class ShoplistPage : ContentPage
{
    public DataTemplate ItemTemplate;
    public ShoppingListData Data;

    public ShoplistPage(string name)
	{
		InitializeComponent();
        ItemTemplate = (DataTemplate)listLayout.Resources["itemTemplate"];

        // Load shopping list by name, if not empty
        if (!string.IsNullOrEmpty(name))
        {
            Data = SavingSystem.LoadShoppingList(name);

            if (Data != null)
            {
                // Creates a view to screen for every item.
                foreach (Item item in Data.Items)
                {
                    // Creates a new content for the item, and sets it's binding.
                    View newItemContent = (View)ItemTemplate.CreateContent();
                    listLayout.Children.Add(newItemContent);
                    newItemContent.BindingContext = item;

                    // Changes grid's color based on it's check value
                    Grid grid = newItemContent as Grid;
                    if (item.Check) grid.BackgroundColor = Colors.Green;
                    else grid.BackgroundColor = Color.FromArgb("#512BD4");
                }
            }
        }

        // If name was empty or loading failed, create a new data.
        if (Data == null)
        {
            Data = new()
            {
                Name = SavingSystem.GetFreeShoppingListName(),
                Items = new()
            };
        }

        // Update list name entry
        ShoppingListNameEntry.Text = Data.Name;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        SavingSystem.SaveShoppingList(Data);
    }

    private void AddItem(object sender, EventArgs e)
    {
        // Creates a new instance of item
        Item newItem = new() { Name = "", Check = false };
        Data.Items.Add(newItem);

        // Creates a new content for the item, and sets it's binding.
        View newItemContent = (View)ItemTemplate.CreateContent();
        listLayout.Children.Add(newItemContent);
        newItemContent.BindingContext = newItem;
    }

    private void ToggleCheckAll(object sender, EventArgs e)
    {
        // See if we want all checked or all checks cleared.
        bool checkStatusForAll = false;
        for (int i = 0; i < Data.Items.Count; i++)
        {
            if (Data.Items[i].Check == false)
            {
                checkStatusForAll = true;
                break;
            }
        }

        // Update the check status for all items in the list.
        foreach (Item item in Data.Items)
        {
            Grid grid = null;
            foreach (Grid g in listLayout.Children)
            {
                if (g.BindingContext == item)
                {
                    grid = g;
                    break;
                }
            }

            if (grid == null) continue;

            item.Check = checkStatusForAll;
            if (item.Check) grid.BackgroundColor = Colors.Green;
            else grid.BackgroundColor = Color.FromArgb("#512BD4"); // blue
        }
    }

    private void Button_SizeChanged(object sender, EventArgs e)
    {
        // Adjusts the button's corner radius.
        if (sender is Button button)
        {
            double min = Math.Min(button.Width, button.Height);
            button.CornerRadius = (int)(min * 0.5f);
        }
        if (sender is ImageButton imgButton)
        {
            double min = Math.Min(imgButton.Width, imgButton.Height);
            imgButton.CornerRadius = (int)(min * 0.5f);
        }
    }

    private void CheckButtonClicked(object sender, EventArgs e)
    {
        try
        {
            ImageButton button = sender as ImageButton;
            Grid grid = button.Parent as Grid;
            Item item = button.BindingContext as Item;

            item.Check = !item.Check;
            if (item.Check) grid.BackgroundColor = Colors.Green;
            else grid.BackgroundColor = Color.FromArgb("#512BD4"); // blue
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void DeleteButtonClicked(object sender, EventArgs e)
    {
        try
        {
            ImageButton button = sender as ImageButton;
            Grid grid = button.Parent as Grid;
            VerticalStackLayout vsl = grid.Parent as VerticalStackLayout;
            Item item = button.BindingContext as Item;

            Data.Items.Remove(item);
            vsl.Children.Remove(grid);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        // Deletes the shopping list with the old name, and saves it with the new name.
        SavingSystem.DeleteShoppingList(Data.Name);
        Data.Name = ShoppingListNameEntry.Text;
        SavingSystem.SaveShoppingList(Data);
    }
}