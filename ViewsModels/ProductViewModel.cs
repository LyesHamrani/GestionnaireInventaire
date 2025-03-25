using GestionnaireInventaire.Data;
using GestionnaireInventaire.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace GestionnaireInventaire.ViewsModels
{
    public class ProductViewModel : ViewModelBase
    {
        #region Propriétés
        private readonly InventaireDbContext _context;
        private ObservableCollection<Produit> _produits;
        private ObservableCollection<Category> _categories;
        private Produit _produitSelectionne;
        private string _nom;
        private string _description;
        private Category _categorieSelectionnee;
        private int _quantite;

        public ObservableCollection<Produit> Produits
        {
            get => _produits;
            set
            {
                _produits = value;
                OnPropertyChanged(nameof(Produits));
            }
        }

        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        public Produit ProduitSelectionne
        {
            get => _produitSelectionne;
            set
            {
                _produitSelectionne = value;
                if (value != null)
                {
                    Nom = value.Nom;
                    Description = value.Description ?? string.Empty;
                    CategorieSelectionnee = value.IdCategorieNavigation;
                    Quantite = value.Quantity ?? 0;
                }
                OnPropertyChanged(nameof(ProduitSelectionne));
            }
        }

        public string Nom
        {
            get => _nom;
            set
            {
                _nom = value;
                OnPropertyChanged(nameof(Nom));
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public Category CategorieSelectionnee
        {
            get => _categorieSelectionnee;
            set
            {
                _categorieSelectionnee = value;
                OnPropertyChanged(nameof(CategorieSelectionnee));
            }
        }

        public int Quantite
        {
            get => _quantite;
            set
            {
                _quantite = value;
                OnPropertyChanged(nameof(Quantite));
            }
        }
        #endregion

        #region Commandes
        public ICommand NouveauCommand { get; private set; }
        public ICommand EnregistrerCommand { get; private set; }
        public ICommand SupprimerCommand { get; private set; }
        public ICommand AugmenterQuantiteCommand { get; private set; }
        public ICommand DiminuerQuantiteCommand { get; private set; }
        #endregion

        #region Constructeur
        public ProductViewModel()
        {
            _context = new InventaireDbContext();
            ChargerDonnees();
            NouveauCommand = new ViewModelCommand(NouveauProduit);
            EnregistrerCommand = new ViewModelCommand(EnregistrerProduit, PeutEnregistrer);
            SupprimerCommand = new ViewModelCommand(SupprimerProduit, PeutSupprimer);
            AugmenterQuantiteCommand = new ViewModelCommand(AugmenterQuantite);
            DiminuerQuantiteCommand = new ViewModelCommand(DiminuerQuantite);
        }

        #endregion

        #region Méthodes
        /// <summary>
        /// Méthode pour charger les produits et les catégorie de la base de données
        /// </summary>
        private async void ChargerDonnees()
        {
            var produits = await _context.Produits
                .Include(p => p.IdCategorieNavigation)
                .ToListAsync();
            Produits = new ObservableCollection<Produit>(produits);

            var categories = await _context.Categories.ToListAsync();
            Categories = new ObservableCollection<Category>(categories);
        }

        /// <summary>
        /// Méthode pour réinitialiser le formulaire de création de produit
        /// </summary>
        /// <param name="parameter"></param>
        private void NouveauProduit(object parameter)
        {
            ProduitSelectionne = new Produit
            {
                Nom = string.Empty,
                Description = string.Empty,
                Quantity = 0
            };
        }

        /// <summary>
        /// Méthode pour créer un produit dans la base de données
        /// </summary>
        /// <param name="parameter"></param>
        private async void EnregistrerProduit(object parameter)
        {
            if (ProduitSelectionne == null) return;
            ProduitSelectionne.Nom = Nom;
            ProduitSelectionne.Description = Description;
            ProduitSelectionne.IdCategorie = CategorieSelectionnee?.Id;
            ProduitSelectionne.Quantity = Quantite;

            if (ProduitSelectionne.Id == 0)
            {
                _context.Produits.Add(ProduitSelectionne);
                await _context.SaveChangesAsync();
                Produits.Add(ProduitSelectionne);
            }
            else
            {
                _context.Produits.Update(ProduitSelectionne);
                await _context.SaveChangesAsync();
                ChargerDonnees();
            }
        }

        /// <summary>
        /// Méthode pour vérifier la validité des données avant d'enregistrer un produit
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private bool PeutEnregistrer(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Nom) && CategorieSelectionnee != null;
        }

        /// <summary>
        /// Méthode pour supprimer un produit de la base de données
        /// </summary>
        /// <param name="parameter"></param>
        private async void SupprimerProduit(object parameter)
        {
            if (ProduitSelectionne == null || ProduitSelectionne.Id == 0) return;

            _context.Produits.Remove(ProduitSelectionne);
            await _context.SaveChangesAsync();
            Produits.Remove(ProduitSelectionne);
            ProduitSelectionne = null;
        }

        /// <summary>
        /// Méthode pour vérifier si un produit peut être supprimé
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>retourne vrai si le produit est disponible</returns>
        private bool PeutSupprimer(object parameter)
        {
            return ProduitSelectionne != null && ProduitSelectionne.Id != 0;
        }

        /// <summary>
        /// Méthode pour augmenter la quantité d'un produit
        /// </summary>
        /// <param name="parameter"></param>
        private void AugmenterQuantite(object parameter)
        {
            Quantite++;
        }

        /// <summary>
        /// Méthode pour diminuer la quantité d'un produit
        /// </summary>
        /// <param name="parameter"></param>
        private void DiminuerQuantite(object parameter)
        {
            if (Quantite > 0)
                Quantite--;
        }
        #endregion
    }
}