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
 
#region Imports

using System;

#endregion

namespace Hyperstore.Modeling
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///  Additional information for session completing events.
    /// </summary>
    /// <seealso cref="T:System.EventArgs"/>
    ///-------------------------------------------------------------------------------------------------
    public class SessionCompletingEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SessionCompletingEventArgs" /> class.
        /// </summary>
        /// <param name="session">The session.</param>
        internal SessionCompletingEventArgs(ISessionInformation session)
        {
            DebugContract.Requires(session);
            Session = session;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Gets the session.
        /// </summary>
        /// <value>
        ///  The session id.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public ISessionInformation Session { get; private set; }
    }
}