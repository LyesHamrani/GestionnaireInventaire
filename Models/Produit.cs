using System;
using System.Collections.Generic;

namespace GestionnaireInventaire.Models;

public partial class Produit
{
    public int Id { get; set; }

    public string Nom { get; set; } = null!;

    public string? Description { get; set; }

    public int? IdCategorie { get; set; }

    public int? Quantity { get; set; }

    public virtual Category? IdCategorieNavigation { get; set; }
}
