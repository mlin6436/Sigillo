using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace Sigillo.App.Models
{
    public class ProcedureModel
    {
        [Required]
        public int ProcedureId { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Benefits { get; set; }

        public string Risks { get; set; }

        public string Followup { get; set; }

        public ObservableCollection<AlternativeProcedureModel> AlternativeProcedures { get; set; }

        public List<MaterialModel> Materials { get; set; }

        public AnaesthesiaModel Anaesthesia { get; set; }

        public ProcedureModel()
        {
            AlternativeProcedures = GetAlternativeProcedures();
        }

        private ObservableCollection<AlternativeProcedureModel> GetAlternativeProcedures()
        {
            var procedures = new ObservableCollection<AlternativeProcedureModel>();
            for (var i = 0; i < 5; i++)
            {
                procedures.Add(new AlternativeProcedureModel { ProcedureId = i, Name = String.Format("{0} {1}", "Alternative", i), Benefits = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.", Risks = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.", Followup = "The follow-up care will be added soon!"});
            }
            return procedures;
        }
    }

    public class AlternativeProcedureModel
    {
        [Required]
        public int ProcedureId { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Benefits { get; set; }

        public string Risks { get; set; }

        public string Followup { get; set; }

        public List<ProcedureModel> AlternativeProcedures { get; set; }

        public List<MaterialModel> Materials { get; set; }

        public AnaesthesiaModel Anaesthesia { get; set; }
    }
}
