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
    
    public partial class tbl_Grupo_Estampas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_Grupo_Estampas()
        {
            this.tbl_Estampa_GrupoEstampa = new HashSet<tbl_Estampa_GrupoEstampa>();
        }
    
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Nullable<int> Id_Imagen { get; set; }
        public Nullable<bool> Visible { get; set; }
        public Nullable<bool> Eliminado { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Estampa_GrupoEstampa> tbl_Estampa_GrupoEstampa { get; set; }
        public virtual tbl_Imagenes tbl_Imagenes { get; set; }
    }
}
