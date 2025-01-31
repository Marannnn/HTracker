﻿using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTracker.Model
{
    public class Habit
    {
        public string? content {  get; set; }
        public bool isCompleted { get; set; } = false;
    }
}
