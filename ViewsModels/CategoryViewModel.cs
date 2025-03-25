using System.Collections.ObjectModel;
using GestionnaireInventaire.Data;


namespace GestionnaireInventaire.ViewsModels
{
    public class CategoryViewModel : ViewModelBase
    {
        #region Propriétés
        private readonly InventaireDbContext _context;
        private ObservableCollection<CategoryDisplay> _categories;

        public ObservableCollection<CategoryDisplay> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }
        #endregion

        #region Constructeur
        public CategoryViewModel()
        {
            _context = new InventaireDbContext();
            LoadCategories();
        }
        #endregion

        #region Méthodes
        private async void LoadCategories()
        {
            try
            {
                var categories = _context.Categories
                    .Select(c => new CategoryDisplay
                    {
                        Id = c.Id,
                        Nom = c.Nom, 
                        NombreProduits = c.Produits.Count() 
                    })
                    .ToList();

                Categories = new ObservableCollection<CategoryDisplay>(categories);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement des catégories: {ex.Message}");
                Categories = new ObservableCollection<CategoryDisplay>();
            }
        }
        #endregion
    }

    // Classe pour l'affichage des catégories (comme une interface en React)
    public class CategoryDisplay
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public int NombreProduits { get; set; }
    }
}