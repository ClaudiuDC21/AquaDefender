using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AquaDefender_Backend.Domain
{
    public class WaterInfo
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountyId { get; set; }
        public County County { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public DateTime DateReported { get; set; }
        public List<WaterValues> WaterValues { get; set; }
        public string AdditionalNotes { get; set; }

          // Indicatori calitate apă
        // public string EscherichiaColi { get; set; } // E. coli 0/250 ml
        // public string Enterococi { get; set; } // 0/250 ml
        // public string PseudomonasAeruginosa { get; set; } // 0/250 ml
        // public string NumarColonii22C { get; set; } // 100/ml
        // public string NumarColonii37C { get; set; } // 20/ml
        // public string TotalitateaSolidelorDizolvate { get; set; }
        // public string Acrilamida { get; set; } // 0,10 µg/l
        // public string Arsen { get; set; } // 10 µg/l
        // public string Benzen { get; set; } // 0 µg/l
        // public string BenzAPiren { get; set; } // 0,01 µg/l
        // public string Bor { get; set; } // 1,0 mg/l
        // public string Bromati { get; set; } // 10 µg/l
        // public string Cadmiu { get; set; } // 5,0 µg/l
        // public string CloruraDeVinil { get; set; } // 0,50 µg/l
        // public string CianuriTotale { get; set; } // 50 µg/l
        // public string CianuriLibere { get; set; } // 10 µg/l
        // public string CromTotal { get; set; } // 50 µg/l
        // public string Cupru { get; set; } // 0,1 mg/l
        // public string Dicloretan { get; set; } // 3,0 µg/l
        // public string Epiclorhidrina { get; set; } // 0,10 µg/l
        // public string Fluoruri { get; set; } // 1,2 mg/l
        // public string HidrocarburiPolicicliceAromatice { get; set; } // 0,10 µg/l
        // public string Mercur { get; set; } // 1,0 µg/l
        // public string Nichel { get; set; } // 20 µg/l
        // public string Nitrati { get; set; } // 50 mg/l
        // public string Nitriti { get; set; } // 0,50 mg/l
        // public string Pesticide { get; set; } // 0,10 µg/l
        // public string PesticideTotal { get; set; } // 0,50 µg/l
        // public string Plumb { get; set; } // 10 µg/l
        // public string Seleniu { get; set; } // 10 µg/l
        // public string Stibiu { get; set; } // 5,0 µg/l
        // public string TetracloretenaSiTricloretena { get; set; } // 10 µg/l
        // public string TrihalometaniTotal { get; set; } // 100 µg/l
        // public string Aluminiu { get; set; } // 200 µg/l
        // public string Amoniu { get; set; } // 0,50 mg/l
        // public string BacteriiColiforme { get; set; } // 0 numar/100 ml
        // public string CarbonOrganicTotal { get; set; } // Nicio modificare anormala
        // public string Cloruri { get; set; } // 250 mg/l
        // public string ClostridiumPerfringens { get; set; } // 0 numar/100 ml
        // public string ClorRezidualLiber { get; set; } // ≥ 0,1 - ≤ 0,5 mg/l
        // public string Conductivitate { get; set; } // 2.500 µS cm la 20°C
        // public string Culoare { get; set; } // Acceptabila consumatorilor
        // public string DuritateTotala { get; set; } // minim 5 grade germane
        // public string Fier { get; set; } // 200 µg/l
        // public string Gust { get; set; } // Acceptabil consumatorilor
        // public string Mangan { get; set; } // 50 µg/l
        // public string Miros { get; set; } // Acceptabil consumatorilor
        // public string Oxidabilitate { get; set; } // 5,0 mg O2/l
        // public string PH { get; set; } // ≥ 6,5; ≤ 9,5 unitati de pH
        // public string Sodiu { get; set; } // 200 mg/l
        // public string Sulfat { get; set; } // 250 mg/l
        // public string SulfuriSiHidrogenSulfurat { get; set; } // 100 µg/l
        // public string Turbiditate { get; set; } // ≤ UNT
        // public string Zinc { get; set; } // 5.000 µg/l
        // public string Tritiu { get; set; } // 100 Bq/l
        // public string DozaEfectivaTotalaDeReferinta { get; set; } // 0,10 mSv/an
        // public string ActivitateaAlfaGlobala { get; set; } // 0,1 Bq/l
        // public string ActivitateaBetaGlobala { get; set; } // 1 Bq/l

    }

}