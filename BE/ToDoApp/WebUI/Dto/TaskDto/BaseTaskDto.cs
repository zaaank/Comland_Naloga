using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoApp.WebUI.Dto.TaskDto
{
    public abstract class BaseTaskDto
    {
        public BaseTaskDto()
        {
            Naslov = string.Empty;
            Opis = string.Empty;
        }
        public string Naslov { get; set; }
        public string Opis { get; set; }
        public DateTime DatumUstvarjanja { get; set; }
        public bool Opravljeno { get; set; }

    }
}