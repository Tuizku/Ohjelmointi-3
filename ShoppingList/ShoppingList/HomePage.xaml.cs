using System.Collections.ObjectModel;

namespace ShoppingList;

public partial class HomePage : ContentPage
{
    public string SelectedName = "";

    public HomePage()
	{
		InitializeComponent();

        ShoplistCollectionView.SelectionChanged += ShoppingListSelected;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        UpdateShoplistCollection();
    }

    private void UpdateShoplistCollection()
    {
        ShoplistCollectionView.SelectedItem = null;

        // Load past shopping lists.
        List<string> shoppingListNames = SavingSystem.GetSavedShoppingListNames();
        ObservableCollection<string> namesCollection = new();
        foreach (string name in shoppingListNames)
        {
            namesCollection.Add(name);
        }

        // Bind the names list to the CollectionView
        ShoplistCollectionView.ItemsSource = namesCollection;
    }

    private void ShoppingListSelected(object sender, SelectionChangedEventArgs e)
    {
        // Saves the selected name into a variable, so we can open/delete that shopping list.
        if (e.CurrentSelection.FirstOrDefault() is string selectedName)
        {
            SelectedName = selectedName;
        }
    }

    private async void CreateListBtn_Clicked(object sender, EventArgs e)
    {
		await Navigation.PushAsync(new ShoplistPage(""));
    }

    private async void OpenListBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ShoplistPage(SelectedName));
    }

    private void DeleteListBtn_Clicked(object sender, EventArgs e)
    {
        SavingSystem.DeleteShoppingList(SelectedName);
        UpdateShoplistCollection();
    }
}