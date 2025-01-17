﻿using System;
using System.Collections.Generic;
using CyberNinja.Views;

namespace CyberNinja.Models
{
    [Serializable]
    public class MineCircle
    {
        //public Dictionary<int, MineRoom> rooms = new();
        
        public List<MineRoom> rooms = new List<MineRoom>();

        public void Add(int index, EMineCellState cellState)
        {
            rooms.Add(new MineRoom
            {
                index = index,
                level = cellState
            });
        }

        public void Update(int index, EMineCellState cellState)
        {
            foreach (var item in rooms)
            {
                if (item.index == index)
                {
                    item.level = cellState;
                    break;
                }
            }
        }

        public EMineCellState Get(int index)
        {
            foreach (var item in rooms)
            {
                if (item.index == index)
                    return item.level;
            }

            return EMineCellState.None;
        }
    }
}