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
    
    public partial class tbl_Imagenes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_Imagenes()
        {
            this.tbl_Estampa_GrupoEstampa = new HashSet<tbl_Estampa_GrupoEstampa>();
            this.tbl_Estampas = new HashSet<tbl_Estampas>();
            this.tbl_Grupo_Estampas = new HashSet<tbl_Grupo_Estampas>();
            this.tbl_Plantillas_Estampas = new HashSet<tbl_Plantillas_Estampas>();
            this.tbl_Plantillas = new HashSet<tbl_Plantillas>();
            this.tbl_Plantillas1 = new HashSet<tbl_Plantillas>();
            this.tbl_Productos = new HashSet<tbl_Productos>();
        }
    
        public int Id { get; set; }
        public string Nombre { get; set; }
        public Nullable<bool> Actio { get; set; }
        public string URI { get; set; }
        public Nullable<int> Tamanio { get; set; }
        public string Extencion { get; set; }
        public Nullable<int> Id_GrupoImagen { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Estampa_GrupoEstampa> tbl_Estampa_GrupoEstampa { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Estampas> tbl_Estampas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Grupo_Estampas> tbl_Grupo_Estampas { get; set; }
        public virtual tbl_grupoImagen tbl_grupoImagen { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Plantillas_Estampas> tbl_Plantillas_Estampas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Plantillas> tbl_Plantillas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Plantillas> tbl_Plantillas1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Productos> tbl_Productos { get; set; }
    }
}
