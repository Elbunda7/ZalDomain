using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZalApiGateway.Models
{
    public class ActionModel : IModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date_start { get; set; }
        public DateTime Date_end { get; set; }
        public string EventType { get; set; }
        //public String Email_vedouci { get; set; }
        //public int Od_hodnosti { get; set; }
        public bool IsOfficial { get; set; }
        public int? Id_Gallery { get; set; }
        public int? Id_Info { get; set; }
        public int? Id_Report { get; set; }
    }
}
