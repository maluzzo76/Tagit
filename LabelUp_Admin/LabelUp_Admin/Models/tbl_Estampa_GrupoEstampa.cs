//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LabelUp_Admin.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_Estampa_GrupoEstampa
    {
        public int Id { get; set; }
        public int Id_GrupoEstampa { get; set; }
        public int Id_Estampa { get; set; }
        public bool Activo { get; set; }
        public bool Eliminado { get; set; }
        public Nullable<int> Id_Imagen { get; set; }
    
        public virtual tbl_Estampas tbl_Estampas { get; set; }
        public virtual tbl_Grupo_Estampas tbl_Grupo_Estampas { get; set; }
        public virtual tbl_Imagenes tbl_Imagenes { get; set; }
    }
}