using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LabelUp_Admin.Models;

namespace LabelUp_Admin.Controllers
{
    public class Pagination<T> where T: class
    {
        private readonly int _RegistrosPorPagina = 6;
        
        public object paginado(IEnumerable<T> oList, int pagina)
        {
            int _TotalRegistros = 0;
            _TotalRegistros = oList.Count();

            var _TotalPaginas = (int)Math.Ceiling((double)_TotalRegistros / _RegistrosPorPagina);

            PaginadorGenerico<T> _Paginador = new PaginadorGenerico<T>()
            {
                RegistrosPorPagina = _RegistrosPorPagina, 
                TotalRegistros = _TotalRegistros,
                TotalPaginas = _TotalPaginas,
                PaginaActual = pagina,
                Resultado = oList.Skip((pagina - 1) * _RegistrosPorPagina).Take(_RegistrosPorPagina).ToList()
            };

            return _Paginador;
        }
    }

    public class PaginadorGenerico<T> where T : class
    {
        public int PaginaActual { get; set; }
        public int RegistrosPorPagina { get; set; }
        public int TotalRegistros { get; set; }
        public int TotalPaginas { get; set; }
        public IEnumerable<T> Resultado { get; set; }
    }
}

#region <Ejemplo para Vista>
/*
 * @*CÓDIGO PARA EL PAGINADOR DE REGISTROS*@
<div class="container">
    <ul class="pagination  pull-right">
        @for (int p = 1; p < Model.TotalPaginas + 1; p++)
        {
            if (p == Model.PaginaActual)
            {
                <li class="active">@Html.ActionLink(p.ToString(), "Index", new { pagina = p })</li>
            }
            else
            {
                <li>@Html.ActionLink(p.ToString(), "Index", new { pagina = p })</li>
            }
        }
    </ul>
</div>
*/
#endregion
