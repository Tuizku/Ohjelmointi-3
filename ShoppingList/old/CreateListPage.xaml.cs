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

public partial class CreateListPage : ContentPage
{
	public DataTemplate ItemTemplate;

	public List<Item> Items = new();

	public CreateListPage()
	{
		InitializeComponent();
		ItemTemplate = (DataTemplate)listLayout.Resources["itemTemplate"];
	}

    private void AddItem(object sender, EventArgs e)
    {
		// Creates a new instance of item
		Item newItem = new() { Name = "", Check = false };
		Items.Add(newItem);

		// Creates a new content for the item, and sets it's binding.
		View newItemContent = (View)ItemTemplate.CreateContent();
		listLayout.Children.Add(newItemContent);
        newItemContent.BindingContext = newItem;
    }

	private void ItemNameChanged(object sender, EventArgs e)
	{
		test.Text = Items[0].Name;
	}
	
    private void Button_SizeChanged(object sender, EventArgs e)
    {
		if (sender is Button button)
		{
			double min = Math.Min(button.Width, button.Height);
			button.CornerRadius = (int)(min * 0.5f);
		}
    }

	private void CheckButtonClicked(object sender, EventArgs e)
	{
		try
		{
			Button button = sender as Button;
			Grid grid = button.Parent as Grid;
			Item item = button.BindingContext as Item;

			item.Check = !item.Check;
			if (item.Check)
			{
				grid.BackgroundColor = Colors.Green;
			}
			else
			{
				grid.BackgroundColor = Color.FromArgb("#512BD4");
            }
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
            Button button = sender as Button;
            Grid grid = button.Parent as Grid;
			VerticalStackLayout vsl = grid.Parent as VerticalStackLayout;
            Item item = button.BindingContext as Item;

            Items.Remove(item);
            vsl.Children.Remove(grid);
        }
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
		}
	}
}