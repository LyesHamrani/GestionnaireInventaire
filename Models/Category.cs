using System;
using System.Collections.Generic;

namespace GestionnaireInventaire.Models;

public partial class Category
{
    public int Id { get; set; }

    public string Nom { get; set; } = null!;

    public virtual ICollection<Produit> Produits { get; set; } = new List<Produit>();
}
