﻿using DemoWebApi.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DemoWebApi.Models
{
    public class ClassRoomCreateModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
       
    }
}
