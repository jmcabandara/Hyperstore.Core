﻿// Copyright 2014 Zenasoft.  All rights reserved.
//
// This file is part of Hyperstore.
//
//    Hyperstore is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Hyperstore is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with Hyperstore.  If not, see <http://www.gnu.org/licenses/>.
 
namespace Hyperstore.Modeling.Commands
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///  Values that represent TrackingState.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public enum TrackingState
    {
        /// <summary>
        /// Unknow element - Should be remove from cache by an eviction policy)
        /// </summary>
        Unknown,
        /// <summary>
        /// This element has been added
        /// </summary>
        Added,
        /// <summary>
        /// This element has been updated
        /// </summary>
        Updated,
        /// <summary>
        /// This element has been removed
        /// </summary>
        Removed
    }
}