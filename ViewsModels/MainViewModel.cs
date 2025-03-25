using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GestionnaireInventaire.ViewsModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Propriétés
        private ViewModelBase _currentChildView;
        private string _caption;
        private IconChar _icon;

        public ViewModelBase CurrentChildView
        {
            get
            {
                return _currentChildView;
            }
            set
            {
                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }
        }
        public string Caption
        {
            get
            {
                return _caption;
            }
            set
            {
                _caption = value;
                OnPropertyChanged(nameof(Caption));
            }
        }
        public IconChar Icon
        {
            get
            {
                return _icon;
            }
            set
            {
                _icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }
        #endregion

        #region Commandes
        public ICommand ShowHomeViewCommand { get; }
        public ICommand ShowProductViewCommand { get; }
        public ICommand ShowCategoryViewCommand { get; }
        #endregion

        #region Constructeur
        public MainViewModel()
        {
            // Initialisation des vues enfants
            ShowHomeViewCommand = new ViewModelCommand(ExecuteShowHomeViewCommand);
            ShowProductViewCommand = new ViewModelCommand(ExecuteShowProductViewCommand);
            ShowCategoryViewCommand = new ViewModelCommand(ExecuteShowCategoryViewCommand);

            // Initialisation de la vue enfant par défaut
            ExecuteShowHomeViewCommand(null);
        }
        #endregion

        #region Méthodes
        /// <summary>
        /// Affiche la vue Home
        /// </summary>
        /// <param name="obj"></param>
        private void ExecuteShowHomeViewCommand(object obj)
        {
            CurrentChildView = new HomeViewModel();
            Caption = "Accueil";
            Icon = IconChar.Home;
        }
        /// <summary>
        /// Affiche la vue Produit
        /// </summary>
        /// <param name="obj"></param>
        private void ExecuteShowProductViewCommand(object obj)
        {
            CurrentChildView = new ProductViewModel();
            Caption = "Produits";
            Icon = IconChar.Box;
        }
        /// <summary>
        /// Affiche la vue Catégorie
        /// </summary>
        /// <param name="obj"></param>
        private void ExecuteShowCategoryViewCommand(object obj)
        {
            CurrentChildView = new CategoryViewModel();
            Caption = "Catégories";
            Icon = IconChar.PieChart;
        }
        #endregion
    }
}
